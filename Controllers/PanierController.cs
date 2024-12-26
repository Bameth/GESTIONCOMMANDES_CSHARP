using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GESTIONCOMMANDES.data;
using GESTIONCOMMANDES.Models.Entities;

namespace GESTIONCOMMANDES.Controllers
{
    public class PanierController : Controller
    {
        private readonly AppDbContext _context;

        public PanierController(AppDbContext context)
        {
            _context = context;
        }

        // Affichage du panier
        public IActionResult Index()
        {
            if (TempData.Peek("PanierId") != null)
            {
                using (var panier = new Panier(_context, TempData.Peek("PanierId").ToString()))
                {
                    var lignes = panier.Lignes();
                    ViewBag.Total = panier.TotalPanier();
                    return View(lignes);
                }
            }
            return View(new List<LignePanier>());
        }

        // Ajouter un produit au panier
        public IActionResult Ajouter(int produitId)
        {
            if (TempData.Peek("PanierId") == null)
            {
                TempData["PanierId"] = Guid.NewGuid().ToString();
            }

            using (var panier = new Panier(_context, TempData.Peek("PanierId").ToString()))
            {
                panier.Ajouter(produitId);
            }
            var count = GetPanierCount();
            return Json(new { count });
        }
        private int GetPanierCount()
        {
            if (TempData.Peek("PanierId") != null)
            {
                using (var panier = new Panier(_context, TempData.Peek("PanierId").ToString()))
                {
                    return panier.Lignes().Count();
                }
            }
            return 0;
        }

        // Retirer un produit du panier
        public IActionResult Retirer(int ligneId)
        {
            if (TempData.Peek("PanierId") != null)
            {
                using (var panier = new Panier(_context, TempData.Peek("PanierId").ToString()))
                {
                    panier.RetirerInPanier(ligneId);
                }
            }
            return RedirectToAction(nameof(Index));
        }
        public IActionResult CountArticles()
        {
            if (TempData.Peek("PanierId") != null)
            {
                using (var panier = new Panier(_context, TempData.Peek("PanierId").ToString()))
                {
                    var totalArticles = panier.Lignes().Sum(l => l.Quantite);
                    return Json(totalArticles);
                }
            }
            return Json(0);
        }

        [HttpPost]
        public IActionResult ModifierQuantite(int ligneId, int increment)
        {
            var lignePanier = _context.LignePanier.SingleOrDefault(l => l.Id == ligneId);

            if (lignePanier == null)
                return RedirectToAction("Index");

            lignePanier.Quantite += increment;

            if (lignePanier.Quantite <= 0)
            {
                _context.LignePanier.Remove(lignePanier);
            }
            else
            {
                _context.LignePanier.Update(lignePanier);
            }

            _context.SaveChanges();

            return RedirectToAction("Index");
        }



    }


}
