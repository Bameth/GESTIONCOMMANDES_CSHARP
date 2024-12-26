using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GESTIONCOMMANDES.Models.Entities;
using GESTIONCOMMANDES.data;

namespace GESTIONCOMMANDES.Controllers
{
    public class LivraisonController : Controller
    {
        private readonly AppDbContext _context;

        public LivraisonController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Livraison
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Livraisons.Include(l => l.Commande).Include(l => l.Livreur);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Livraison/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var livraison = await _context.Livraisons
                .Include(l => l.Commande)
                .Include(l => l.Livreur)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (livraison == null)
            {
                return NotFound();
            }

            return View(livraison);
        }

        // GET: Livraison/Create
        public IActionResult Create()
        {
            ViewData["CommandeId"] = new SelectList(_context.Commandes, "Id", "Id");
            ViewData["LivreurId"] = new SelectList(_context.Livreurs, "Id", "Id");
            return View();
        }

        // POST: Livraison/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DateLivraison,CommandeId,Adresse,LivreurId,Id,CreateAt,UpdateAt")] Livraison livraison)
        {
            if (ModelState.IsValid)
            {
                _context.Add(livraison);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CommandeId"] = new SelectList(_context.Commandes, "Id", "Id", livraison.CommandeId);
            ViewData["LivreurId"] = new SelectList(_context.Livreurs, "Id", "Id", livraison.LivreurId);
            return View(livraison);
        }

        // GET: Livraison/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var livraison = await _context.Livraisons.FindAsync(id);
            if (livraison == null)
            {
                return NotFound();
            }
            ViewData["CommandeId"] = new SelectList(_context.Commandes, "Id", "Id", livraison.CommandeId);
            ViewData["LivreurId"] = new SelectList(_context.Livreurs, "Id", "Id", livraison.LivreurId);
            return View(livraison);
        }

        // POST: Livraison/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DateLivraison,CommandeId,Adresse,LivreurId,Id,CreateAt,UpdateAt")] Livraison livraison)
        {
            if (id != livraison.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(livraison);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LivraisonExists(livraison.Id))
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
            ViewData["CommandeId"] = new SelectList(_context.Commandes, "Id", "Id", livraison.CommandeId);
            ViewData["LivreurId"] = new SelectList(_context.Livreurs, "Id", "Id", livraison.LivreurId);
            return View(livraison);
        }

        // GET: Livraison/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var livraison = await _context.Livraisons
                .Include(l => l.Commande)
                .Include(l => l.Livreur)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (livraison == null)
            {
                return NotFound();
            }

            return View(livraison);
        }

        // POST: Livraison/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var livraison = await _context.Livraisons.FindAsync(id);
            if (livraison != null)
            {
                _context.Livraisons.Remove(livraison);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LivraisonExists(int id)
        {
            return _context.Livraisons.Any(e => e.Id == id);
        }
    }
}
