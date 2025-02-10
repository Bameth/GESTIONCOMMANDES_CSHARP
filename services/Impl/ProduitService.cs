using GESTIONCOMMANDES.data;
using GESTIONCOMMANDES.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace GESTIONCOMMANDES.services.Impl
{
    public class ProduitService : IProduitService
    {
        private readonly AppDbContext _context;

        public ProduitService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Produit> Create(Produit produit)
        {
            _context.Produits.Add(produit);
            await _context.SaveChangesAsync();
            return produit;
        }

        public async Task<IEnumerable<Produit>> GetProduitsAsync()
        {
            return await _context.Produits.ToListAsync();
        }

        public async Task<(List<Produit> Produits, int TotalPages)> GetProduitsAsync(string libelle, int page = 1, int pageSize = 6)
        {
            var query = _context.Produits.AsQueryable();

            if (!string.IsNullOrEmpty(libelle))
            {
                query = query.Where(p => p.Libelle.Contains(libelle));
            }

            var totalProduits = await query.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalProduits / pageSize);

            var produits = await query
                .OrderBy(p => p.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (produits, totalPages);
        }
    }
}
