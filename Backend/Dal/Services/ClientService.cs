using Dal.Api;
using Dal.Models;
using Microsoft.EntityFrameworkCore;

namespace Dal.Services;

public class ClientService : IClient
{
    private readonly DatabaseManager _db;

    public ClientService(DatabaseManager db)
    {
        _db = db;
    }

    public async Task<IEnumerable<Client>> ReadAllAsync() =>
        await _db.Clients.ToListAsync();

    public async Task<Client> ReadByIdAsync(string id) =>
        await _db.Clients.FindAsync(id);

    public async Task<bool> CreateAsync(Client entity)
    {
        await _db.Clients.AddAsync(entity);
        return await _db.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var entity = await _db.Clients.FindAsync(id);
        if (entity == null) return false;
        _db.Clients.Remove(entity);
        return await _db.SaveChangesAsync() > 0;
    }

    public async Task<bool> UpdateAsync(Client entity)
    {
        _db.Clients.Update(entity);
        return await _db.SaveChangesAsync() > 0;
    }
}
