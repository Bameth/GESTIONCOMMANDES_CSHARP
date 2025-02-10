using GESTIONCOMMANDES.data;
using GESTIONCOMMANDES.enums;
using Microsoft.EntityFrameworkCore;

namespace GESTIONCOMMANDES.Models.Entities
{
    public class Panier : IDisposable
    {
        private AppDbContext _context;
        private string _panierId;

        public Panier(AppDbContext context, string PanierId)
        {
            _context = context;
            _panierId = PanierId;
        }
        public void Ajouter(int produitId)
        {
            LignePanier ligne = _context.LignePanier.SingleOrDefault(s => s.PanierId == _panierId && s.ProduitId == produitId);

            if (ligne == null)
            {
                ligne = new LignePanier
                {
                    PanierId = _panierId,
                    ProduitId = produitId,
                    Quantite = 1
                };
                _context.LignePanier.Add(ligne);
            }
            else
            {
                ligne.Quantite++;
            }
            _context.SaveChanges();
        }

        public void PasserCommande(string userName)
        {
            if (Nombre() == 0)
            {
                throw new InvalidOperationException("Le panier est vide.");
            }

            using var transaction = _context.Database.BeginTransaction();

            try
            {
                var client = _context.Clients.FirstOrDefault(c => c.User.UserName == userName);
                if (client == null)
                {
                    throw new InvalidOperationException("Client introuvable pour cet utilisateur.");
                }

                var lignePaniers = _context.LignePanier
                    .Where(s => s.PanierId == _panierId)
                    .Include(l => l.Produit)
                    .ToList();

                if (!lignePaniers.Any())
                {
                    throw new InvalidOperationException("Impossible de récupérer les articles du panier.");
                }

                foreach (var ligne in lignePaniers)
                {
                    if (ligne.Produit.QteStock < ligne.Quantite)
                    {
                        throw new InvalidOperationException($"Le produit {ligne.Produit.Libelle} n'a pas assez de stock disponible.");
                    }
                }

                var commande = new Commande
                {
                    UserName = userName,
                    Date = DateTime.UtcNow,
                    EtatCommande = StatutCommande.ENCOURS,
                    MontantTotal = TotalPanier(),
                    ClientId = client.Id
                };

                var nombreCommandes = GetNombreCommandesDansLeMois(client.Id).Result;
                commande.MontantTotal = CalculerMontantAvecRemise(commande, nombreCommandes);

                _context.Commandes.Add(commande);
                _context.SaveChanges();

                foreach (var ligne in lignePaniers)
                {
                    var detailCommande = new DetailCommande
                    {
                        CommandeId = commande.Id,
                        ProduitId = ligne.ProduitId,
                        Prix = ligne.Produit.Prix,
                        QuantiteCmd = ligne.Quantite,
                        Montant = ligne.Montant()
                    };

                    _context.DetailCommandes.Add(detailCommande);

                    ligne.Produit.QteStock -= ligne.Quantite;
                    _context.Produits.Update(ligne.Produit);
                }

                _context.SaveChanges();

                ViderPanier();

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new InvalidOperationException($"Une erreur est survenue lors de la validation de la commande : {ex.Message}");
            }
        }
        private async Task<int> GetNombreCommandesDansLeMois(int clientId)
        {
            var dateDebutMois = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1, 0, 0, 0, DateTimeKind.Utc);
            var nombreCommandes = await _context.Commandes
                .Where(c => c.ClientId == clientId && c.Date >= dateDebutMois)
                .CountAsync();

            return nombreCommandes;
        }
        private static decimal CalculerMontantAvecRemise(Commande commande, int nombreCommandes)
        {
            const decimal remise = 0.10m;
            if (nombreCommandes >= 10)
            {
                return commande.MontantTotal * (1 - remise);
            }

            return commande.MontantTotal;
        }
        public void ViderPanier()
        {
            var lignePaniers = _context.LignePanier
                        .Where(s => s.PanierId == _panierId)
                        .ToList();
            if (lignePaniers.Count > 0)
            {
                _context.LignePanier.RemoveRange(lignePaniers);
                _context.SaveChanges();
            }
        }
        public void RetirerInPanier(int LigneId)
        {
            LignePanier Ligne = _context.LignePanier.SingleOrDefault(s => s.Id == LigneId);
            if (Ligne != null)
            {
                _context.LignePanier.Remove(Ligne);
            }
            _context.SaveChanges();
        }

        public LignePanier Ligne(int LigneId)
        {
            return _context.LignePanier.Include(l => l.Produit).FirstOrDefault(m => m.Id == LigneId);
        }
        public IList<LignePanier> Lignes()
        {
            IList<LignePanier> Lignes = _context.LignePanier
            .Where(s => s.PanierId == _panierId)
            .Include(l => l.Produit)
            .ToList();
            return Lignes;
        }
        public decimal TotalPanier()
        {
            var lignes = _context.LignePanier
                .Where(s => s.PanierId == _panierId)
                .Include(l => l.Produit)
                .ToList();

            decimal total = lignes.Sum(l => l.Produit.PourcentageSolde.HasValue
                ? l.Quantite * l.Produit.PrixSolde
                : l.Quantite * l.Produit.Prix);

            return total;
        }
        public void MigrerPanier(string userName)
        {
            if (_panierId != userName)
            {
                _context.LignePanier.Where(s => s.PanierId == _panierId)
                                   .ToList()
                                   .ForEach(l => l.PanierId = userName);
            }
            _context.SaveChanges();
        }
        public int Nombre()
        {
            int Nombre = _context.LignePanier.Where(s => s.PanierId == _panierId).ToList().Count();
            return Nombre;
        }

        public void Dispose()
        {
            if (_context != null)
            {
                _context.Dispose();
                _context = null;
            }
        }
    }
}