using Dal.Api;
using Dal.Models;
using Microsoft.EntityFrameworkCore;

namespace Dal.Services;

public class BusyAppoitmentService : IBusyAppointment
{
    private readonly DatabaseManager _db;

    public BusyAppoitmentService(DatabaseManager databaseManager)
    {
        _db = databaseManager;
    }

    public async Task<bool> CreateAsync(BusyAppointment entity)
    {
        await _db.BusyAppointments.AddAsync(entity);
        return await _db.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var appointment = await _db.BusyAppointments.FindAsync(id);
        if (appointment == null) return false;
        _db.BusyAppointments.Remove(appointment);
        return await _db.SaveChangesAsync() > 0;
    }

    public async Task<IEnumerable<BusyAppointment>> ReadAllAsync() =>
        await _db.BusyAppointments.ToListAsync();

    public async Task<BusyAppointment> ReadByIdAsync(string id) =>
        await _db.BusyAppointments.FindAsync(id);

    public async Task<bool> UpdateAsync(BusyAppointment entity)
    {
        _db.BusyAppointments.Update(entity);
        return await _db.SaveChangesAsync() > 0;
    }
}
