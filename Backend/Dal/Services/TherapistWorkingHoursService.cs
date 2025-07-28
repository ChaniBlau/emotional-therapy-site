using Dal.Api;
using Dal.Models;
using Microsoft.EntityFrameworkCore;

namespace Dal.Services
{
    public class TherapistWorkingHoursService : ITherapistWorkingHours
    {
        private readonly DatabaseManager _db;

        public TherapistWorkingHoursService(DatabaseManager databaseManager)
        {
            _db = databaseManager;
        }

        public async Task<TherapistWorkingHour?> GetWorkingHoursByTherapistAndDay(string therapistId, int dayOfWeek)
        {
            return await _db.TherapistWorkingHours
                .FirstOrDefaultAsync(wh => wh.TherapistId.Trim() == therapistId.Trim() && wh.DayOfWeek == dayOfWeek);
        }

        public async Task<List<string>> GetTherapistIdsByDay(int dayOfWeek)
        {
            return await _db.TherapistWorkingHours
                .Where(wh => wh.DayOfWeek == dayOfWeek)
                .Select(wh => wh.TherapistId)
                .Distinct()
                .ToListAsync();
        }

        public async Task<List<TherapistWorkingHour>> GetAllWorkingHours()
        {
            return await _db.TherapistWorkingHours.ToListAsync();
        }

        public async Task<bool> CreateAsync(TherapistWorkingHour entity)
        {
            await _db.TherapistWorkingHours.AddAsync(entity);
            return await _db.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(TherapistWorkingHour entity)
        {
            _db.TherapistWorkingHours.Update(entity);
            return await _db.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _db.TherapistWorkingHours.FindAsync(id);
            if (entity == null) return false;

            _db.TherapistWorkingHours.Remove(entity);
            return await _db.SaveChangesAsync() > 0;
        }
        
    }
}