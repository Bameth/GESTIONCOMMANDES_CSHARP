using GESTIONCOMMANDES.enums;
using GESTIONCOMMANDES.Models.Entities;
using Microsoft.AspNetCore.Identity;

namespace GESTIONCOMMANDES.data.fixtures
{
    public class Fixtures
    {
        public static void Initialize(AppDbContext context, IPasswordHasher<User> passwordHasher)
        {
            context.Database.EnsureCreated();

            if (!context.Users.Any())
            {
                // Créer des utilisateurs
                var user1 = CreateUser("Bobo", "Bobo", "Bobo@example.com", "771001111", Role.CLIENT, passwordHasher);
                var user2 = CreateUser("WANE", "BAILA", "bbw@example.com", "771001010", Role.RS, passwordHasher);
                var user3 = CreateUser("FAYE", "Mohamed", "mohamed@example.com", "771001012", Role.LIVREUR, passwordHasher);
                var user4 = CreateUser("BA", "Ameth", "amethba8826@gmail.com", "781069049", Role.RS, passwordHasher, true);

                context.Users.AddRange(user1, user2, user3, user4);

                // Créer des clients
                var client1 = CreateClient(user1, "POINT-E", 1000);
                var client2 = CreateClient(user2, "Mbao", 1000);
                var client3 = CreateClient(user3, "Mariste", 1000);

                context.Clients.AddRange(client1, client2, client3);

                // Créer des produits
                var produit1 = CreateProduit("Produit A", 500, 100);
                var produit2 = CreateProduit("Produit B", 5000, 100);

                context.Produits.AddRange(produit1, produit2);

                // Paiements
                var paiement1 = new Paiement { Date = DateTime.UtcNow, TypePaiement = TypePaiement.OM };
                var paiement2 = new Paiement { Date = DateTime.UtcNow, TypePaiement = TypePaiement.WAVE };

                // Livreurs
                var livreur1 = CreateLivreur("NDIAYE", "Birane", "0712345678");
                var livreur2 = CreateLivreur("YANGO", "Assane", "0712345679");

                context.Livreurs.AddRange(livreur1, livreur2);

                // Commandes
                var commande1 = new Commande
                {
                    UserName = user1.Email ?? string.Empty,
                    Client = client1,
                    MontantTotal = produit1.Prix * 2,
                    Date = DateTime.UtcNow,
                    Paiement = paiement1,
                    EtatCommande = StatutCommande.ENCOURS
                };

                var commande2 = new Commande
                {
                    UserName = user2.Email ?? string.Empty,
                    Client = client2,
                    MontantTotal = produit2.Prix * 2,
                    Date = DateTime.UtcNow,
                    Paiement = paiement2,
                    EtatCommande = StatutCommande.ANNULEE
                };

                context.Commandes.AddRange(commande1, commande2);

                // Détails de commande
                var detail1 = CreateDetailCommande(commande1, produit1, 2);
                var detail2 = CreateDetailCommande(commande2, produit2, 2);

                context.DetailCommandes.AddRange(detail1, detail2);

                // Livraisons
                var livraison1 = CreateLivraison(commande1, livreur1, "POINT-E");
                var livraison2 = CreateLivraison(commande2, livreur2, "KMF");

                context.Livraisons.AddRange(livraison1, livraison2);

                context.SaveChanges();
            }
        }

        // Méthodes d'assistance pour factoriser
        private static User CreateUser(string nom, string prenom, string email, string phone, Role role, IPasswordHasher<User> hasher, bool normalize = false)
        {
            var user = new User
            {
                Nom = nom,
                Prenom = prenom,
                UserName = email,
                PhoneNumber = phone,
                Email = email,
                Role = role
            };

            if (normalize)
            {
                user.NormalizedUserName = email.ToUpperInvariant();
                user.NormalizedEmail = email.ToUpperInvariant();
            }

            user.PasswordHash = hasher.HashPassword(user, "passer123");
            return user;
        }

        private static Client CreateClient(User user, string adresse, decimal solde)
        {
            return new Client
            {
                Nom = user.Nom,
                Prenom = user.Prenom,
                Telephone = user.PhoneNumber ?? string.Empty,
                Adresse = adresse,
                Solde = (double)solde,
                User = user
            };
        }

        private static Produit CreateProduit(string libelle, decimal prix, int stock)
        {
            return new Produit
            {
                Libelle = libelle,
                Prix = prix,
                QteStock = stock,
                Description = "Produit de qualité supérieure avec une bonne description.",
                ImageFileName = "/produits/default.jpg"
            };
        }

        private static Livreur CreateLivreur(string nom, string prenom, string tel)
        {
            return new Livreur
            {
                Nom = nom,
                Prenom = prenom,
                Telephone = tel,
                EstDisponible = true
            };
        }

        private static DetailCommande CreateDetailCommande(Commande commande, Produit produit, int qte)
        {
            return new DetailCommande
            {
                Commande = commande,
                Produit = produit,
                QuantiteCmd = qte,
                Prix = produit.Prix,
                Montant = produit.Prix * qte
            };
        }

        private static Livraison CreateLivraison(Commande commande, Livreur livreur, string adresse)
        {
            return new Livraison
            {
                Commande = commande,
                Livreur = livreur,
                Adresse = adresse,
                DateLivraison = DateTime.UtcNow
            };
        }
    }
}
