using GESTIONCOMMANDES.data;
using GESTIONCOMMANDES.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace GESTIONCOMMANDES.services.Impl
{
    public class ClientService : IClientService
    {
        private readonly AppDbContext _context;

        public ClientService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Client> Create(Client client)
        {
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();
            return client;
        }

        public async Task<(List<Client> Clients, int TotalPages)> GetClientsAsync(string telephone, string Prenom, int page = 1, int pageSize = 4)
        {
            var query = _context.Clients.AsQueryable();

            if (!string.IsNullOrEmpty(telephone))
            {
                query = query.Where(c => c.Telephone.Contains(telephone));
            }

            if (!string.IsNullOrEmpty(Prenom))
            {
                query = query.Where(c => c.Prenom.Contains(Prenom));
            }

            var totalClients = await query.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalClients / pageSize);

            var clients = await query
                .OrderBy(c => c.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (clients, totalPages);
        }

        public async Task<IEnumerable<Client>> GetClientsAsync()
        {
            return await _context.Clients.ToListAsync();
        }
    }
}
