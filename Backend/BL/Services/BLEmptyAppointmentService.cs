using BL.Api;
using Dal.Api;
using Dal.Models;
using Microsoft.EntityFrameworkCore;

namespace BL.Services
{
    public class BLEmptyAppointmentService : IBLEmptyAppointment
    {
        private readonly IEmptyAppointment _emptyAppointment;
        private readonly IBusyAppointment _busyAppointment;
        private readonly ITherapistWorkingHours _therapistWorkingHours;
        private readonly int _appointmentDurationMinutes = 60; // משך תור בדקות

        public BLEmptyAppointmentService(
            IEmptyAppointment emptyAppointment,
            IBusyAppointment busyAppointment,
            ITherapistWorkingHours therapistWorkingHours)
        {
            _emptyAppointment = emptyAppointment;
            _busyAppointment = busyAppointment;
            _therapistWorkingHours = therapistWorkingHours;
        }

        public async Task<List<EmptyAppointment>> GetAllEmptyAppointments()
        {
            var result = await _emptyAppointment.ReadAllAsync();
            return result.ToList();
        }

        public async Task<List<TimeOnly>> GetAvailableHours(string therapistId, DateOnly date)
        {
            // קבל את יום השבוע (0=ראשון, 1=שני, וכו')
            var dayOfWeek = (int)date.DayOfWeek;

            // קבל את שעות העבודה של המטפל ביום זה
            var workingHours = await _therapistWorkingHours.GetWorkingHoursByTherapistAndDay(therapistId, dayOfWeek);

            if (workingHours == null)
                return new List<TimeOnly>(); // אין שעות עבודה ביום זה

            // קבל את כל התורים התפוסים ביום זה
            var busyAppointments = await _busyAppointment.ReadAllAsync();
            var busyTimes = busyAppointments
                .Where(ba => ba.TherapistId.Trim() == therapistId.Trim() && ba.Date == date)
                .Select(ba => ba.Time)
                .ToList();

            // צור רשימת שעות פנויות
            var availableHours = new List<TimeOnly>();
            var currentTime = workingHours.StartTime;

            while (currentTime.AddMinutes(_appointmentDurationMinutes) <= workingHours.EndTime)
            {
                // בדוק אם השעה הזו לא תפוסה
                if (!busyTimes.Contains(currentTime))
                {
                    availableHours.Add(currentTime);
                }

                // עבור לשעה הבאה
                currentTime = currentTime.AddMinutes(_appointmentDurationMinutes);
            }

            return availableHours;
        }

        // שיטה נוספת לקבלת מטפלים זמינים לפי תאריך
        public async Task<List<string>> GetAvailableTherapistIdsByDate(DateOnly date)
        {
            var dayOfWeek = (int)date.DayOfWeek;

            // קבל מטפלים שעובדים ביום זה
            var workingTherapistIds = await _therapistWorkingHours.GetTherapistIdsByDay(dayOfWeek);

            // קבל מטפלים שיש להם לפחות תור פנוי אחד ביום זה
            var availableTherapistIds = new List<string>();

            foreach (var therapistId in workingTherapistIds)
            {
                var availableHours = await GetAvailableHours(therapistId, date);
                if (availableHours.Any())
                {
                    availableTherapistIds.Add(therapistId);
                }
            }

            return availableTherapistIds;
        }
    }
}