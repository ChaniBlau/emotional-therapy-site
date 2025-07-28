using BL.Api;
using BL.Models;
using Dal.Api;
using Dal.Models;
using System.Collections.Generic;

namespace BL.Services;

public class BLUserService : IBLUser
{
    private readonly IClient _client;
    private readonly ITherapist _therapist;
    private readonly IBusyAppointment _busyAppointment;

    private readonly IBLBusyAppointment _busyAppointmentService;
    private readonly IBLClient _blClient;
    private readonly IBLTherapist _blTherapist;

    public BLUserService(
        IClient client,
        ITherapist therapist,
        IBusyAppointment busyAppointment,
        IBLBusyAppointment busyAppointmentService,
        IBLClient blClient,
        IBLTherapist blTherapist)
    {
        _client = client;
        _therapist = therapist;
        _busyAppointment = busyAppointment;
        _busyAppointmentService = busyAppointmentService;
        _blClient = blClient;
        _blTherapist = blTherapist;
    }


    public async Task<List<BusyAppointmentForUser>> LogInSpecificUser(string id, string name)
    {
        var busyAppointments = await _busyAppointment.ReadAllAsync();
        var clients = await _client.ReadAllAsync();
        var therapists = await _therapist.ReadAllAsync();

        if (busyAppointments == null)
        {
            return new List<BusyAppointmentForUser>();
        }

        // בדוק אם זה לקוח
        var isClient = clients.Any(c => c.Id.Trim().Equals(id.Trim(), StringComparison.OrdinalIgnoreCase));

        if (isClient)
        {
            // החזר תורים עבור לקוח
            return busyAppointments
                .Where(app => app.ClientId.Trim().Equals(id.Trim(), StringComparison.OrdinalIgnoreCase))
                .Select(appointment =>
                {
                    var therapistForDetails = therapists.FirstOrDefault(t => t.Id.Equals(appointment.TherapistId.Trim()));
                    DateTime appointmentDateTime = appointment.Date.ToDateTime(appointment.Time);

                    return new BusyAppointmentForUser
                    {
                        Id = appointment.Code.ToString(), // זה המפתח - החזר את מזהה התור!
                        AppointmentId = appointment.Code, // הוסף גם שדה נוסף לוודאות
                        Role = "Client",
                        Date = appointmentDateTime,
                        Name = therapistForDetails?.FirstName + " " + therapistForDetails?.LastName,
                        Email = therapistForDetails?.Email,
                        PhoneNumber = therapistForDetails?.PhoneNumber
                    };
                })
                .ToList();
        }
        else
        {
            // בדוק אם זה מטפל
            var isTherapist = therapists.Any(t => t.Id.Trim().Equals(id.Trim(), StringComparison.OrdinalIgnoreCase));

            if (isTherapist)
            {
                // החזר תורים עבור מטפל
                return busyAppointments
                    .Where(app => app.TherapistId.Trim().Equals(id.Trim(), StringComparison.OrdinalIgnoreCase))
                    .Select(appointment =>
                    {
                        var clientForDetails = clients.FirstOrDefault(c => c.Id.Equals(appointment.ClientId.Trim()));
                        DateTime appointmentDateTime = appointment.Date.ToDateTime(appointment.Time);

                        return new BusyAppointmentForUser
                        {
                            Id = appointment.Code.ToString(),
                            AppointmentId = appointment.Code,
                            Role = "Therapist",
                            Date = appointmentDateTime,
                            Name = clientForDetails?.FirstName + " " + clientForDetails?.LastName,
                            Email = clientForDetails?.Email,
                            PhoneNumber = clientForDetails?.PhoneNumber,
                            Age = clientForDetails != null ? DateTime.Now.Year - clientForDetails.YearOfBirth : 0
                        };
                    })
                    .ToList();
            }
        }

        return new List<BusyAppointmentForUser>();
    }
}
