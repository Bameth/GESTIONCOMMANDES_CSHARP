namespace GESTIONCOMMANDES.Models.Entities
{
    public class Client : Personne
    {
        public double Solde { get; set; }
        public string? Adresse { get; set; }
        public List<Commande>? Commandes { get; set; }
        public User? User { get; set; }
    }
}