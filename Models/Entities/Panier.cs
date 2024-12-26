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

            var commande = new Commande
            {
                UserName = userName,
                Date = DateTime.UtcNow,
                EtatCommande = StatutCommande.ENCOURS,
                MontantTotal = TotalPanier(),
                ClientId = client.Id
            };

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
            }

            _context.SaveChanges();

            ViderPanier();
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

            decimal total = lignes.Sum(l => l.Montant());
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