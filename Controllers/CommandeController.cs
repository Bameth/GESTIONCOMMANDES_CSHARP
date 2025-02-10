using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GESTIONCOMMANDES.Models.Entities;
using GESTIONCOMMANDES.data;
using Microsoft.AspNetCore.Authorization;
using GESTIONCOMMANDES.enums;

namespace GESTIONCOMMANDES.Controllers
{
    [Authorize]
    public class CommandeController : Controller
    {
        private readonly AppDbContext _context;

        public CommandeController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Commande
        public async Task<IActionResult> Index()
        {
            var userName = User.Identity?.Name;
            if (string.IsNullOrEmpty(userName))
            {
                return Unauthorized();
            }


            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserName == userName);

            if (user == null)
            {
                return Unauthorized();
            }

            IQueryable<Commande> commandesQuery = _context.Commandes;

            // Si l'utilisateur est un RS, on récupère toutes les commandes
            if (user.Role == Role.RS)
            {
                commandesQuery = commandesQuery.Include(c => c.Client);
            }
            else
            {
                commandesQuery = commandesQuery
                    .Include(c => c.Client)
                    .Where(c => c.Client != null && c.Client.User != null && c.Client.User.UserName == userName);
            }

            return View(await commandesQuery.ToListAsync());
        }


        [HttpGet]
        public IActionResult PasserCommande()
        {
            try
            {
                var panierId = TempData.Peek("PanierId")?.ToString();
                if (string.IsNullOrEmpty(panierId))
                {
                    TempData["ErrorMessage"] = "Votre panier est vide ou introuvable.";
                    return RedirectToAction("Index", "Panier");
                }

                using (var panier = new Panier(_context, panierId))
                {
                    var userName = User.Identity?.Name ?? "UtilisateurAnonyme";
                    panier.PasserCommande(userName);
                }

                TempData["SuccessMessage"] = "Commande passée avec succès !";
                return RedirectToAction("Index", "Commande");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Une erreur s'est produite : {ex.Message}";
                return RedirectToAction("Index", "Panier");
            }
        }
        // GET: Commande/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var commande = await _context.Commandes
                .Include(c => c.Client)
                .Include(c => c.Livraison)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (commande == null)
            {
                return NotFound();
            }

            // Charger les livreurs disponibles pour le formulaire de livraison
            ViewData["Livreurs"] = await _context.Livreurs.Where(l => l.EstDisponible).ToListAsync();

            return View(commande);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PlanifierLivraison(int? commandeId, int? livreurId, DateTime dateLivraison, string? adresse)
        {
            Console.WriteLine("----------------------------------------------------------------------------------------------------------------------");
            Console.WriteLine("CommandeId: " + commandeId + ", LivreurId: " + livreurId + ", DateLivraison: " + dateLivraison + ", adresse: " + adresse);
            Console.WriteLine("----------------------------------------------------------------------------------------------------------------------");
            try
            {
                var commande = await _context.Commandes
                    .Include(c => c.Livraison)
                    .FirstOrDefaultAsync(c => c.Id == commandeId);

                if (commande == null)
                {
                    return BadRequest("Commande introuvable.");
                }

                if (commande.Livraison == null)
                {
                    var livraison = new Livraison
                    {
                        CommandeId = commande.Id,
                        LivreurId = (int)livreurId,
                        DateLivraison = Convert.ToDateTime(dateLivraison).ToUniversalTime(),
                        Adresse = adresse
                    };
                    _context.Livraisons.Add(livraison);
                }
                else
                {
                    commande.Livraison.LivreurId = (int)livreurId;
                    commande.Livraison.Adresse = adresse;
                    commande.Livraison.DateLivraison = Convert.ToDateTime(dateLivraison).ToUniversalTime();
                }
                commande.EtatCommande = StatutCommande.PRET_A_LIVRER;
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Livraison planifiée avec succès.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Loggez l'erreur ici
                Console.WriteLine($"Erreur : {ex.Message}");
                TempData["ErrorMessage"] = "Une erreur est survenue lors de la planification.";
                return RedirectToAction("Details", new { id = commandeId });
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Paiement(int commandeId, TypePaiement typePaiement, string reference)
        {
            Console.WriteLine("---------------------------------------------------------------------------------------------");
            Console.WriteLine("CommandeId: " + commandeId + ", TypePaiement: " + typePaiement + ", Reference: " + reference);
            Console.WriteLine("---------------------------------------------------------------------------------------------");
            if (commandeId <= 0 || string.IsNullOrWhiteSpace(reference))
            {
                TempData["ErrorMessage"] = "Informations de paiement invalides.";
                return RedirectToAction(nameof(Details), new { id = commandeId });
            }

            try
            {
                var commande = await _context.Commandes
                    .FirstOrDefaultAsync(c => c.Id == commandeId);

                if (commande == null)
                {
                    TempData["ErrorMessage"] = "Commande introuvable.";
                    return RedirectToAction(nameof(Details), new { id = commandeId });
                }

                // Mettre à jour les états de la commande
                commande.EtatCommande = StatutCommande.LIVREE;
                commande.StatutPaiement = StatutPaiement.PAYEE;

                // Ajouter un nouveau paiement
                var paiement = new Paiement
                {
                    CommandeId = commande.Id,
                    TypePaiement = typePaiement,
                    Reference = reference,
                    Date = DateTime.UtcNow
                };

                await _context.Paiements.AddAsync(paiement);
                await _context.SaveChangesAsync();
                Console.WriteLine("Données recu: " + reference + " " + typePaiement);

                TempData["SuccessMessage"] = "Paiement effectué avec succès.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Une erreur s'est produite lors du paiement : {ex.Message}");
                TempData["ErrorMessage"] = "Une erreur s'est produite lors du paiement.";
                return RedirectToAction(nameof(Details), new { id = commandeId });
            }
        }


        // GET: Commande/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var commande = await _context.Commandes.FindAsync(id);
            if (commande == null)
            {
                return NotFound();
            }
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Id", commande.ClientId);
            ViewData["LivreurId"] = new SelectList(_context.Livreurs, "Id", "Id", commande.Livraison);
            return View(commande);
        }

        // POST: Commande/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Date,MontantTotal,ClientId,LivreurId,Id,CreateAt,UpdateAt")] Commande commande)
        {
            if (id != commande.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(commande);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommandeExists(commande.Id))
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
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Id", commande.ClientId);
            return View(commande);
        }

        // GET: Commande/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var commande = await _context.Commandes
                .Include(c => c.Client)
                .Include(c => c.Livraison)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (commande == null)
            {
                return NotFound();
            }

            return View(commande);
        }

        // POST: Commande/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var commande = await _context.Commandes.FindAsync(id);
            if (commande != null)
            {
                _context.Commandes.Remove(commande);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CommandeExists(int id)
        {
            return _context.Commandes.Any(e => e.Id == id);
        }
    }
}
