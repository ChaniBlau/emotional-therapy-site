using Dal.Api;
using Dal.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal.Services;

public class ClientService : IClient
{
    private readonly DatabaseManager _databaseManager;
    public ClientService(DatabaseManager databaseManager)
    {
        _databaseManager = databaseManager;
    }
    public async Task<bool> CreateAsync(Client entity)
    {
        bool isExists = await _databaseManager.Clients.AnyAsync(x => x.Id == entity.Id);
        if (isExists)
        {
            throw new ArgumentException("Client already exists");
        }
        else
        {
            await _databaseManager.Clients.AddAsync(entity);
            var result = await _databaseManager.SaveChangesAsync();
            return result > 0;
        }
    }


    //we don't need really to use this func cause in a real site the clients became inactive
    public async Task<bool> DeleteAsync(string id)
    {
        if (id == null)
        {
            throw new ArgumentNullException("Id cannot be null");
        }
        await _databaseManager.Clients.Where(x => x.Id == id).ExecuteDeleteAsync();
        var result = await _databaseManager.SaveChangesAsync();
        return result > 0;
    }

    public async Task<IEnumerable<Client>> ReadAllAsync() => await _databaseManager.Clients.ToListAsync();


    public async Task<Client> ReadByIdAsync(string id) => await _databaseManager.Clients.FindAsync(id) ?? throw new ArgumentNullException("Client not found");

    public async Task<bool> UpdateAsync(Client entity)
    {
        await _databaseManager.Clients.Where(x => x.Id == entity.Id).ExecuteUpdateAsync(setters => setters
        .SetProperty(x => x.FirstName, entity.FirstName)
        .SetProperty(x => x.LastName, entity.LastName)
        .SetProperty(x => x.YearOfBirth, entity.YearOfBirth)
        .SetProperty(x => x.Email, entity.Email)
        .SetProperty(x => x.City, entity.City)
        .SetProperty(x => x.PhoneNumber, entity.PhoneNumber)
        .SetProperty(x => x.TherapistId, entity.TherapistId));
        var result = await _databaseManager.SaveChangesAsync();
        return result > 0;

    }


}
