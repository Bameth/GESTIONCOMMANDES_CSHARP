namespace GESTIONCOMMANDES.Models.Entities
{
    public class Livraison : AbstractEntity
    {
        public DateTime DateLivraison { get; set; }
        public int CommandeId { get; set; }
        public Commande? Commande { get; set; }
        public string? Adresse { get; set; }
        public int LivreurId { get; set; }
        public Livreur? Livreur { get; set; }
    }
}