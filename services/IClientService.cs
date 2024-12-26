using GESTIONCOMMANDES.Models.Entities;

namespace GESTIONCOMMANDES.services
{
    public interface IClientService
    {
        Task<IEnumerable<Client>> GetClientsAsync();
        Task<Client> Create(Client client);
        Task<(List<Client> Clients, int TotalPages)> GetClientsAsync(string telephone, string Prenom, int page = 1, int pageSize = 4);
    }
}
