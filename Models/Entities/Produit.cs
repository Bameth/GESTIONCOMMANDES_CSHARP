namespace GESTIONCOMMANDES.Models.Entities
{
    public class Produit : AbstractEntity
    {
        public string Libelle { get; set; }
        public decimal Prix { get; set; }
        public int QteStock { get; set; }
        public bool IsDisponible { get; set; }
        public string ImageFileName { get; set; }
        public string Description { get; set; }
        public List<string> Images { get; set; } = new List<string>();

        public decimal? PourcentageSolde { get; set; }

        public decimal PrixSolde
        {
            get
            {
                return PourcentageSolde.HasValue ? Prix * (1 - PourcentageSolde.Value / 100) : Prix;
            }
        }

        public bool EtatProduit()
        {
            IsDisponible = QteStock > 0;
            return IsDisponible;
        }
    }
}