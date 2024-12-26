using GESTIONCOMMANDES.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GESTIONCOMMANDES.data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Client> Clients { get; set; }
        public DbSet<LignePanier> LignePanier { get; set; }
        public DbSet<Produit> Produits { get; set; }
        public DbSet<Commande> Commandes { get; set; }
        public DbSet<DetailCommande> DetailCommandes { get; set; }
        public DbSet<Livreur> Livreurs { get; set; }
        public DbSet<Livraison> Livraisons { get; set; }
        public DbSet<Paiement> Paiements { get; set; }
    }


}
