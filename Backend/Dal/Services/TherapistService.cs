using Dal.Api;
using Dal.Models;
using Microsoft.EntityFrameworkCore;

namespace Dal.Services;

public class TherapistService : ITherapist
{
    private readonly DatabaseManager _db;

    public TherapistService(DatabaseManager db)
    {
        _db = db;
    }

    public async Task<IEnumerable<Therapist>> ReadAllAsync() =>
        await _db.Therapists.ToListAsync();

    public async Task<Therapist> ReadByIdAsync(string id) =>
        await _db.Therapists.FindAsync(id);

    public async Task<bool> CreateAsync(Therapist entity)
    {
        await _db.Therapists.AddAsync(entity);
        return await _db.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var entity = await _db.Therapists.FindAsync(id);
        if (entity == null) return false;
        _db.Therapists.Remove(entity);
        return await _db.SaveChangesAsync() > 0;
    }

    public async Task<bool> UpdateAsync(Therapist entity)
    {
        _db.Therapists.Update(entity);
        return await _db.SaveChangesAsync() > 0;
    }
}