using BL.Api;
using BL.Models;
using Dal.Api;
using Dal.Models;

namespace BL.Services;

public class BLTherapistService : IBLTherapist
{
    private readonly IBusyAppointment _busyAppointment;
    private readonly ITherapist _therapist;
    private readonly IClient _clients;
    private readonly ITherapist _blTherapist;
    private readonly ITherapistWorkingHours _therapistWorkingHours;

    public BLTherapistService(
        IBusyAppointment busyAppointment,
        ITherapist therapist,
        IClient clients,
        ITherapist blTherapist,
        ITherapistWorkingHours therapistWorkingHours)
    {
        _busyAppointment = busyAppointment;
        _therapist = therapist;
        _clients = clients;
        _blTherapist = blTherapist;
        _therapistWorkingHours = therapistWorkingHours;
    }

    public async Task<List<Therapist>> GetAllTherapists()
    {
        var result = await _blTherapist.ReadAllAsync();
        return result.ToList();
    }

    public async Task<Therapist?> AuthenticateTherapist(string id, string name)
    {
        var therapists = await _therapist.ReadAllAsync();
        return therapists.FirstOrDefault(t => t.Id.Trim() == id.Trim());
    }

    // שיטה חדשה לקבלת מטפלים זמינים לפי תאריך עם שעות עבודה
    public async Task<List<Therapist>> GetAvailableTherapistsByDateWithWorkingHours(DateOnly date)
    {
        var dayOfWeek = (int)date.DayOfWeek;

        // קבל מטפלים שעובדים ביום זה
        var workingTherapistIds = await _therapistWorkingHours.GetTherapistIdsByDay(dayOfWeek);

        if (!workingTherapistIds.Any())
            return new List<Therapist>();

        // קבל את כל התורים התפוסים ביום זה
        var busyAppointments = await _busyAppointment.ReadAllAsync();
        var busyTherapistsByDate = busyAppointments
            .Where(ba => ba.Date == date)
            .GroupBy(ba => ba.TherapistId.Trim())
            .ToDictionary(g => g.Key, g => g.Select(ba => ba.Time).ToList());

        // בדוק לכל מטפל אם יש לו לפחות תור פנוי אחד
        var availableTherapistIds = new List<string>();

        foreach (var therapistId in workingTherapistIds)
        {
            var workingHours = await _therapistWorkingHours.GetWorkingHoursByTherapistAndDay(therapistId, dayOfWeek);
            if (workingHours == null) continue;

            // חשב כמה תורים יכולים להיות ביום
            var totalPossibleAppointments = CalculatePossibleAppointments(workingHours.StartTime, workingHours.EndTime);

            // קבל כמה תורים תפוסים
            var busyCount = busyTherapistsByDate.ContainsKey(therapistId.Trim())
                ? busyTherapistsByDate[therapistId.Trim()].Count
                : 0;

            // אם יש לפחות תור פנוי אחד
            if (busyCount < totalPossibleAppointments)
            {
                availableTherapistIds.Add(therapistId);
            }
        }

        // החזר את המטפלים הזמינים
        var allTherapists = await _blTherapist.ReadAllAsync();
        return allTherapists
            .Where(t => availableTherapistIds.Contains(t.Id.Trim()))
            .ToList();
    }

    private int CalculatePossibleAppointments(TimeOnly startTime, TimeOnly endTime)
    {
        const int appointmentDurationMinutes = 60;
        var totalMinutes = (endTime - startTime).TotalMinutes;
        return (int)(totalMinutes / appointmentDurationMinutes);
    }
}