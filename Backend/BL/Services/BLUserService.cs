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
        try
        {
            var busyAppointments = await _busyAppointment.ReadAllAsync();
            var clients = await _client.ReadAllAsync();
            var therapists = await _therapist.ReadAllAsync();

            if (busyAppointments == null || !busyAppointments.Any())
            {
                return new List<BusyAppointmentForUser>();
            }

            var normalizedId = id?.Trim();
            if (string.IsNullOrEmpty(normalizedId))
            {
                return new List<BusyAppointmentForUser>();
            }

            var isClient = clients?.Any(c =>
                c.Id.Trim().Equals(normalizedId, StringComparison.OrdinalIgnoreCase)) ?? false;

            if (isClient)
            {
                return busyAppointments
                    .Where(app => app.ClientId.Trim().Equals(normalizedId, StringComparison.OrdinalIgnoreCase))
                    .OrderByDescending(app => app.Date)
                    .ThenByDescending(app => app.Time)
                    .Select(appointment =>
                    {
                        var therapistForDetails = therapists?.FirstOrDefault(t =>
                            t.Id.Trim().Equals(appointment.TherapistId.Trim(), StringComparison.OrdinalIgnoreCase));

                        DateTime appointmentDateTime = appointment.Date.ToDateTime(appointment.Time);

                        return new BusyAppointmentForUser
                        {
                            Id = appointment.Code.ToString(),
                            AppointmentId = appointment.Code, // מזהה התור למחיקה
                            Role = "Client",
                            Date = appointmentDateTime,
                            Name = therapistForDetails != null ?
                                $"{therapistForDetails.FirstName} {therapistForDetails.LastName}" :
                                "Unknown Therapist",
                            Email = therapistForDetails?.Email,
                            PhoneNumber = therapistForDetails?.PhoneNumber
                        };
                    })
                    .ToList();
            }
            else
            {
                // בדוק אם זה מטפל
                var isTherapist = therapists?.Any(t =>
                    t.Id.Trim().Equals(normalizedId, StringComparison.OrdinalIgnoreCase)) ?? false;

                if (isTherapist)
                {
                    // החזר תורים עבור מטפל
                    return busyAppointments
                        .Where(app => app.TherapistId.Trim().Equals(normalizedId, StringComparison.OrdinalIgnoreCase))
                        .OrderByDescending(app => app.Date)
                        .ThenByDescending(app => app.Time)
                        .Select(appointment =>
                        {
                            var clientForDetails = clients?.FirstOrDefault(c =>
                                c.Id.Trim().Equals(appointment.ClientId.Trim(), StringComparison.OrdinalIgnoreCase));

                            DateTime appointmentDateTime = appointment.Date.ToDateTime(appointment.Time);

                            return new BusyAppointmentForUser
                            {
                                Id = appointment.Code.ToString(),
                                AppointmentId = appointment.Code, // מזהה התור למחיקה
                                Role = "Therapist",
                                Date = appointmentDateTime,
                                Name = clientForDetails != null ?
                                    $"{clientForDetails.FirstName} {clientForDetails.LastName}" :
                                    "Unknown Client",
                                Email = clientForDetails?.Email,
                                PhoneNumber = clientForDetails?.PhoneNumber,
                                Age = clientForDetails != null ?
                                    DateTime.Now.Year - clientForDetails.YearOfBirth : 0
                            };
                        })
                        .ToList();
                }
            }

            return new List<BusyAppointmentForUser>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in LogInSpecificUser: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
            return new List<BusyAppointmentForUser>();
        }
    }
}