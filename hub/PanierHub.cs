using Microsoft.AspNetCore.SignalR;

namespace GESTIONCOMMANDES.hub
{
    public class PanierHub : Hub
    {
        // Méthode pour informer tous les clients connectés de la mise à jour du panier
        public async Task UpdatePanier(string panierId)
        {
            await Clients.All.SendAsync("ReceivePanierUpdate", panierId);  // Envoie à tous les clients connectés
        }
    }
}