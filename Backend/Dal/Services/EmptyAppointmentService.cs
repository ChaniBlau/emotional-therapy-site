using Dal.Api;
using Dal.Models;
using Microsoft.EntityFrameworkCore;

namespace Dal.Services;

public class EmptyAppointmentService : IEmptyAppointment
{
    private readonly DatabaseManager _db;

    public EmptyAppointmentService(DatabaseManager db)
    {
        _db = db;
    }

    public async Task<IEnumerable<EmptyAppointment>> ReadAllAsync() =>
        await _db.EmptyAppointments.ToListAsync();

    public async Task<EmptyAppointment> ReadByIdAsync(string id) =>
        await _db.EmptyAppointments.FindAsync(id);

    public async Task<bool> CreateAsync(EmptyAppointment entity)
    {
        await _db.EmptyAppointments.AddAsync(entity);
        return await _db.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        int code = int.Parse(id);
        var entity = await _db.EmptyAppointments.FindAsync(code);
        if (entity == null) return false;
        _db.EmptyAppointments.Remove(entity);
        return await _db.SaveChangesAsync() > 0;
    }

    public async Task<bool> UpdateAsync(EmptyAppointment entity)
    {
        _db.EmptyAppointments.Update(entity);
        return await _db.SaveChangesAsync() > 0;
    }
}
