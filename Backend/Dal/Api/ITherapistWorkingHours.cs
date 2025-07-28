// Dal/Api/ITherapistWorkingHours.cs
using Dal.Models;

namespace Dal.Api
{
    public interface ITherapistWorkingHours
    {
        Task<TherapistWorkingHour?> GetWorkingHoursByTherapistAndDay(string therapistId, int dayOfWeek);
        Task<List<string>> GetTherapistIdsByDay(int dayOfWeek);
        Task<List<TherapistWorkingHour>> GetAllWorkingHours();
        Task<bool> CreateAsync(TherapistWorkingHour entity);
        Task<bool> UpdateAsync(TherapistWorkingHour entity);
        Task<bool> DeleteAsync(int id);
    }
}