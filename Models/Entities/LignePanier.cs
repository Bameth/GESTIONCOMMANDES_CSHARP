namespace GESTIONCOMMANDES.Models.Entities
{
    public class LignePanier
    {
        public int Id { get; set; }
        public int Quantite { get; set; }
        public string PanierId { get; set; }
        public int ProduitId { get; set; }
        public Produit Produit { get; set; }

        public decimal Montant()
        {
            return Quantite * Produit.Prix;
        }
    }
}