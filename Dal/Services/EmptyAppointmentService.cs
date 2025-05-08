using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dal.Api;
using Dal.Models;
using Microsoft.EntityFrameworkCore;

namespace Dal.Services;

public class EmptyAppointmentService : IEmptyAppointment
{
    private readonly DatabaseManager _databaseManager;
    public EmptyAppointmentService(DatabaseManager databaseManager)
    {
        _databaseManager = databaseManager;
    }
    public async Task<bool> CreateAsync(EmptyAppointment entity)
    {
        await _databaseManager.EmptyAppointments.AddAsync(entity);
        var result = await _databaseManager.SaveChangesAsync();
        return result > 0;
        
    }

    public async Task<bool> DeleteAsync(string id)
    {
        if (id == null)
        {
            throw new ArgumentNullException("Id cannot be null");
        }
        await _databaseManager.EmptyAppointments.Where(x => x.Code == int.Parse(id)).ExecuteDeleteAsync();
        var result = await _databaseManager.SaveChangesAsync();
        return result > 0;

    }

    public async Task<IEnumerable<EmptyAppointment>> ReadAllAsync() => await _databaseManager.EmptyAppointments.ToListAsync();


    public async Task<EmptyAppointment> ReadByIdAsync(string id) => await _databaseManager.EmptyAppointments.FindAsync(int.Parse(id)) ?? throw new ArgumentNullException("Empty appointment not found");


    public async Task<bool> UpdateAsync(EmptyAppointment entity)
    {
        await _databaseManager.EmptyAppointments.Where(x => x.Code == entity.Code).ExecuteUpdateAsync(setters => setters
        .SetProperty(x => x.Code, entity.Code)
        .SetProperty(x => x.Date, entity.Date)
        .SetProperty(x => x.Time, entity.Time)
        .SetProperty(x => x.TherapistId, entity.TherapistId));
        var result = await _databaseManager.SaveChangesAsync();
        return result > 0;

    }
}
