namespace GESTIONCOMMANDES.Models.Entities
{
    public class Produit : AbstractEntity
    {
        public string Libelle { get; set; }
        public decimal Prix { get; set; }
        public int QteStock { get; set; }
        public List<DetailCommande> DetailCommandes { get; set; }
        public bool IsDisponible { get; set; }
        public string ImageFileName { get; set; }

        public bool EtatProduit()
        {
            IsDisponible = QteStock > 0;
            return IsDisponible;
        }
    }
}
