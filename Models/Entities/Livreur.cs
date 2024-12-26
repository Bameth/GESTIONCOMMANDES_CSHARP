using System.ComponentModel.DataAnnotations.Schema;

namespace GESTIONCOMMANDES.Models.Entities
{
    public class Livreur : Personne
    {
        public Boolean EstDisponible { get; set; } = true;
        public List<Livraison>? Livraisons { get; set; }
    }
}