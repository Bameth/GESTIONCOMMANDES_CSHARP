using GESTIONCOMMANDES.enums;
using GESTIONCOMMANDES.Models.Entities;
using BCrypt.Net;

namespace GESTIONCOMMANDES.data.fixtures
{
    public class Fixtures
    {
        public static void Initialize(AppDbContext context)
        {
            // S'assurer que la base de données est créée
            context.Database.EnsureCreated();

            // Si les utilisateurs n'existent pas encore
            if (!context.Users.Any())
            {
                // Créer des utilisateurs de base
                var user1 = new User
                {
                    Nom = "Bobo",
                    UserName = "Bobo@example.com",
                    Prenom = "Bobo",
                    PhoneNumber = "771001111",
                    Email = "Bobo@example.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("passer123"),
                    Role = Role.CLIENT
                };

                var user2 = new User
                {
                    Nom = "WANE",
                    Prenom = "BAILA",
                    UserName = "bbw@example.com",
                    PhoneNumber = "771001010",
                    Email = "bbw@example.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("passer123"),
                    Role = Role.RS
                };
                var user3 = new User
                {
                    Nom = "FAYE",
                    UserName = "mohamed_login",
                    Prenom = "Mohamed",
                    PhoneNumber = "771001012",
                    Email = "mohamed_login",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("passer123"),
                    Role = Role.LIVREUR
                };
                var user4 = new User
                {
                    Nom = "BA",
                    UserName = "amethba8826@gmail.com",
                    NormalizedUserName = "AMETHBA8826@GMAIL.COM",
                    Prenom = "Ameth",
                    PhoneNumber = "781069049",
                    Email = "amethba8826@gmail.com",
                    NormalizedEmail = "AMETHBA8826@GMAIL.COM",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("passer123"),
                    Role = Role.RS
                };

                context.Users.AddRange(user1, user2, user3, user4);

                // Créer des clients
                var client1 = new Client
                {
                    Nom = "Bobo",
                    Prenom = "Bobo",
                    Telephone = "771001111",
                    Solde = 1000,
                    Adresse = "POINT-E",
                    User = user1
                };
                var client2 = new Client
                {
                    Nom = "WANE",
                    Prenom = "BAILA",
                    Telephone = "771001010",
                    Solde = 1000,
                    Adresse = "Mbao",
                    User = user2
                };
                var client3 = new Client
                {
                    Nom = "FAYE",
                    Prenom = "Mohamed",
                    Telephone = "771001012",
                    Solde = 1000,
                    Adresse = "Mariste",
                    User = user1
                };

                context.Clients.AddRange(client1, client2, client3);

                // Créer des produits
                var produit1 = new Produit
                {
                    Libelle = "Produit A",
                    Prix = 500,
                    QteStock = 100,
                    Description = "Produit de Qualité superior avec une bonne description",
                    ImageFileName = "/produits/20241213000632.jpg"
                };
                var produit2 = new Produit
                {
                    Libelle = "Produit B",
                    Prix = 5000,
                    Description = "Produit de Qualité superior avec une bonne description",
                    QteStock = 100,
                    ImageFileName = "/produits/20241213000632.jpg"
                };

                context.Produits.AddRange(produit1, produit2);

                // Créer des paiements
                var paiement1 = new Paiement
                {
                    Date = DateTime.UtcNow,
                    TypePaiement = TypePaiement.OM, // Exemple : paiement OM
                    Commande = null // L'associer après la création de la commande
                };
                var paiement2 = new Paiement
                {
                    Date = DateTime.UtcNow,
                    TypePaiement = TypePaiement.WAVE, // Exemple : paiement OM
                    Commande = null // L'associer après la création de la commande
                };

                // Créer un livreur
                var livreur1 = new Livreur
                {
                    Nom = "YANGO",
                    Prenom = "Birane",
                    Telephone = "0712345678",
                    EstDisponible = true, ///
                };
                // Créer un livreur
                var livreur2 = new Livreur
                {
                    Nom = "YANGO",
                    Prenom = "Assane",
                    Telephone = "0712345679",
                    EstDisponible = true,
                };


                context.Livreurs.AddRange(livreur1, livreur2);

                // Créer une commande
                var commande1 = new Commande
                {
                    UserName = user1.Email,
                    Client = client1,
                    MontantTotal = produit1.Prix * 2,
                    Date = DateTime.UtcNow,
                    Livraison = null, // Associer apres la creation de la livraison
                    Paiement = paiement1, // Associer le paiement
                    EtatCommande = StatutCommande.ENCOURS
                };
                var commande2 = new Commande
                {
                    UserName = user2.Email,
                    Client = client2,
                    MontantTotal = produit2.Prix * 2,
                    Date = DateTime.UtcNow,
                    Livraison = null, // Associer apres la creation de la livraison
                    Paiement = paiement2, // Associer le paiement
                    EtatCommande = StatutCommande.ANNULEE
                };

                context.Commandes.AddRange(commande1, commande2);

                // Créer des détails de commande
                var detailCommande1 = new DetailCommande
                {
                    Commande = commande1,
                    Montant = produit1.Prix * 2,
                    Produit = produit1,
                    Prix = produit1.Prix,
                    QuantiteCmd = 2
                };
                var detailCommande2 = new DetailCommande
                {
                    Commande = commande2,
                    Montant = produit2.Prix * 2,
                    Produit = produit2,
                    Prix = produit2.Prix,
                    QuantiteCmd = 2
                };
                context.DetailCommandes.AddRange(detailCommande1, detailCommande2);
                // Créer une livraison
                var livraison1 = new Livraison
                {
                    DateLivraison = DateTime.UtcNow,
                    Livreur = livreur1,
                    Commande = commande1,
                    Adresse = "POINT-E",
                };
                var livraison2 = new Livraison
                {
                    DateLivraison = DateTime.UtcNow,
                    Livreur = livreur2,
                    Commande = commande2,
                    Adresse = "KMF",
                };
                context.Livraisons.AddRange(livraison1, livraison2);

                // Associer le paiement à la commande
                paiement1.Commande = commande1;
                paiement2.Commande = commande2;

                // Associer la livraison à la commande
                livraison1.Commande = commande1;
                livraison2.Commande = commande2;

                // Sauvegarder les changements dans la base de données
                context.SaveChanges();
            }
        }
    }
}
