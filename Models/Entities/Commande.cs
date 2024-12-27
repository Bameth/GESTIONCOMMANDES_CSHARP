using System.ComponentModel.DataAnnotations.Schema;
using GESTIONCOMMANDES.enums;

namespace GESTIONCOMMANDES.Models.Entities
{
    public class Commande : AbstractEntity
    {
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public string UserName { get; set; }
        public decimal MontantTotal { get; set; }
        public int? ClientId { get; set; }
        public Client? Client { get; set; }
        public List<DetailCommande>? DetailsCommandes { get; set; }
        public Livraison? Livraison { get; set; }
        public StatutCommande EtatCommande { get; set; }
        public Paiement? Paiement { get; set; }
        public StatutPaiement? StatutPaiement { get; set; }

    }
}