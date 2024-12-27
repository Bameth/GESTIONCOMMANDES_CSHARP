using GESTIONCOMMANDES.enums;

namespace GESTIONCOMMANDES.Models.Entities
{
    public class Paiement : AbstractEntity
    {
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public int CommandeId { get; set; }
        public Commande? Commande { get; set; }
        public string? Reference { get; set; }
        public TypePaiement TypePaiement { get; set; }
    }
}