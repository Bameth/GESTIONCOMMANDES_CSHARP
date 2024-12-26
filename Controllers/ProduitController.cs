using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GESTIONCOMMANDES.Models.Entities;
using GESTIONCOMMANDES.data;
using GESTIONCOMMANDES.Models;
using GESTIONCOMMANDES.services;
using Microsoft.AspNetCore.Authorization;
namespace GESTIONCOMMANDES.Controllers
{
    public class ProduitController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IProduitService _produitService;
        private readonly IWebHostEnvironment _environment;
        private const int PageSize = 4;



        public ProduitController(AppDbContext context, IProduitService produitService, IWebHostEnvironment environment)
        {
            _context = context;
            _produitService = produitService;
            _environment = environment;
        }

        // GET: Produit
        public async Task<IActionResult> Index(int? page)
        {
            const byte TAILLE_PAR_PAGE = 20;

            int numeroPage = page ?? 1;
            int totalProduits = await _context.Produits.CountAsync();
            int dernierePage = (int)Math.Ceiling((double)totalProduits / TAILLE_PAR_PAGE);

            // Correction pour éviter les pages hors limites
            numeroPage = Math.Max(1, Math.Min(numeroPage, dernierePage));

            // Récupération des produits paginés avec un tri
            var produits = await _context.Produits
                .OrderBy(p => p.Id)
                .Skip((numeroPage - 1) * TAILLE_PAR_PAGE)
                .Take(TAILLE_PAR_PAGE)
                .ToListAsync();


            // Création du modèle de vue
            ViewData["DernierePage"] = dernierePage;
            ViewData["NumeroPage"] = numeroPage;

            return View(produits);
        }

        public async Task<IActionResult> Liste(string libelle, int page = 1)
        {
            var (produits, totalPages) = await _produitService.GetProduitsAsync(libelle, page, PageSize);

            var model = new PaginationViewModel
            {
                Produits = produits,
                CurrentPage = page,
                TotalPages = totalPages
            };
            return View(model);
        }



        // GET: Produit/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var produit = await _context.Produits.FindAsync(id);
            if (produit == null)
            {
                return NotFound();
            }

            return View(produit);
        }

        public IActionResult AjouterAuPanier(int id)
        {
            if (TempData.Peek("PanierId") == null)
            {
                TempData["PanierId"] = Guid.NewGuid().ToString();
            }

            using (Panier panier = new Panier(_context, TempData.Peek("PanierId").ToString()))
            {
                panier.Ajouter(id);
            }

            TempData["ProduitAjoute"] = true;

            return RedirectToAction("Details", new { id });
        }

        // GET: Produit/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Produit/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProduitDto produitDto)
        {
            if (produitDto.ImageFile == null || produitDto.ImageFile.Length == 0)
            {
                ModelState.AddModelError("ImageFile", "Veuillez sélectionner une image.");
            }

            if (!ModelState.IsValid)
            {
                return View(produitDto);
            }

            string newFileName = $"{DateTime.UtcNow:yyyyMMddHHmmss}{Path.GetExtension(produitDto.ImageFile.FileName)}";

            string uploadsFolder = Path.Combine(_environment.WebRootPath, "produits");
            Directory.CreateDirectory(uploadsFolder);

            string filePath = Path.Combine(uploadsFolder, newFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await produitDto.ImageFile.CopyToAsync(stream);
            }

            // Création du produit
            var produit = new Produit
            {
                Libelle = produitDto.Libelle,
                Prix = produitDto.Prix,
                QteStock = produitDto.QteStock,
                ImageFileName = newFileName,
                CreateAt = DateTime.UtcNow,
                UpdateAt = DateTime.UtcNow
            };

            produit.EtatProduit();

            _context.Produits.Add(produit);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Liste));
        }

        // GET: Produit/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produit = await _context.Produits.FindAsync(id);
            if (produit == null)
            {
                return NotFound();
            }
            return View(produit);
        }

        // POST: Produit/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Libelle,Prix,QteStock,Id,CreateAt,UpdateAt")] Produit produit)
        {
            if (id != produit.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(produit);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProduitExists(produit.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(produit);
        }

        // GET: Produit/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produit = await _context.Produits
                .FirstOrDefaultAsync(m => m.Id == id);
            if (produit == null)
            {
                return NotFound();
            }

            return View(produit);
        }

        // POST: Produit/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var produit = await _context.Produits.FindAsync(id);
            if (produit != null)
            {
                _context.Produits.Remove(produit);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProduitExists(int id)
        {
            return _context.Produits.Any(e => e.Id == id);
        }
        public void OnGet(int id)
        {
            Produit produit = _context.Produits.Find(id);
            if (TempData.Peek("PanierId") != null && !User.Identity.IsAuthenticated)
            {
                TempData["PanierId"] = Guid.NewGuid().ToString();
            }
            {
                using (Panier p = new Panier(_context, TempData.Peek("PanierId").ToString()))
                {
                    p.Ajouter(id);
                }
            }
        }
    }
}
