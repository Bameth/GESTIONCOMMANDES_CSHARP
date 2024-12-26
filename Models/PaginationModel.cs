using GESTIONCOMMANDES.Models.Entities;

namespace GESTIONCOMMANDES.Models
{
    public class PaginationViewModel
    {
        public List<Client>? Clients { get; set; }
        public List<Produit>? Produits { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }

}