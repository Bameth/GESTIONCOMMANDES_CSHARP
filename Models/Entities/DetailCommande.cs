namespace GESTIONCOMMANDES.Models.Entities
{
    public class DetailCommande : AbstractEntity
    {
        public decimal Prix { get; set; }
        public int QuantiteCmd { get; set; }
        public decimal Montant { get; set; }
        public int CommandeId { get; set; }
        public Commande? Commande { get; set; }
        public int ProduitId { get; set; }
        public Produit? Produit { get; set; }
    }
}