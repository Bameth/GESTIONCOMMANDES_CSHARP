using GESTIONCOMMANDES.Models.Entities;

namespace GESTIONCOMMANDES.services
{
    public interface IProduitService
    {
        Task<IEnumerable<Produit>> GetProduitsAsync();
        Task<Produit> Create(Produit produit);
        Task<(List<Produit> Produits, int TotalPages)> GetProduitsAsync(string libelle, int page = 1, int pageSize = 4);
    }
}
