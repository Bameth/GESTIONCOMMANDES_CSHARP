using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GESTIONCOMMANDES.Models.Entities;
using GESTIONCOMMANDES.data;
using Microsoft.AspNetCore.Authorization;
using GESTIONCOMMANDES.services;
using GESTIONCOMMANDES.Models;
using GESTIONCOMMANDES.enums;

namespace GESTIONCOMMANDES.Controllers
{
    public class ClientController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IClientService _clientService;
        private const int PageSize = 4;

        public ClientController(AppDbContext context, IClientService clientService)
        {
            _clientService = clientService;
            _context = context;
        }

        public async Task<IActionResult> Index(string telephone, string Prenom, int page = 1)
        {
            var (clients, totalPages) = await _clientService.GetClientsAsync(telephone, Prenom, page, PageSize);

            var model = new PaginationViewModel
            {
                Clients = clients,
                CurrentPage = page,
                TotalPages = totalPages
            };

            return View(model);
        }
        // GET: Client/Details/5
        public async Task<IActionResult> Details(int? id, string etatFilter)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Clients
            .Include(c => c.Commandes)
            .ThenInclude(cmd => cmd.Livraison)
            .FirstOrDefaultAsync(m => m.Id == id);

            if (client == null)
            {
                return NotFound();
            }

            if (client?.Commandes != null && !string.IsNullOrEmpty(etatFilter))
            {
                var etatCommande = StatutHelper.GetValue(etatFilter);
                if (etatCommande.HasValue)
                {
                    client.Commandes = client.Commandes
                        .Where(c => c.EtatCommande == etatCommande.Value)
                        .ToList();
                }
            }
            return View(client);
        }


        // GET: Client/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Client/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Solde,Adresse,Nom,Prenom,Telephone,Id,CreateAt,UpdateAt")] Client client)
        {
            if (ModelState.IsValid)
            {
                _context.Add(client);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(client);
        }

        // GET: Client/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Clients.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }
            return View(client);
        }

        // POST: Client/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Solde,Adresse,Nom,Prenom,Telephone,Id,CreateAt,UpdateAt")] Client client)
        {
            if (id != client.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(client);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClientExists(client.Id))
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
            return View(client);
        }

        // GET: Client/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Clients
                .FirstOrDefaultAsync(m => m.Id == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // POST: Client/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client != null)
            {
                _context.Clients.Remove(client);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClientExists(int id)
        {
            return _context.Clients.Any(e => e.Id == id);
        }
    }
}
