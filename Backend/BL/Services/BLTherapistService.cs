using BL.Api;
using BL.Models;
using Dal.Api;
using Dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services;

public class BLTherapistService : IBLTherapist
{
    private readonly IBusyAppointment _busyAppointment;
    private readonly ITherapist _therapist;
    private readonly IClient _clients;
    private readonly ITherapist _blTherapist;


    public BLTherapistService(IBusyAppointment busyAppointment, ITherapist therapist, IClient clients, ITherapist blTherapist)
    {
        _busyAppointment = busyAppointment;
        _therapist = therapist;
        _clients = clients;
        _blTherapist = blTherapist;
    }
    //public async Task<List<BusyAppointmentForUser>> GetBusyAppointmentsForTherapist(string id, string name)
    //{
    //    var busyAppointments = await _blBusyAppointments.ReadAllAsync();
    //    var clients = await _blClients.ReadAllAsync();
    //    if (busyAppointments == null)
    //    {
    //        return new List<BusyAppointmentForUser>();
    //    }
    //    return busyAppointments
    //        .Where(app => app.TherapistId.Trim().Equals(id.Trim(),StringComparison.OrdinalIgnoreCase))
    //            .Select(appointment =>
    //            {
    //                var clientForDetails = clients.FirstOrDefault(c => c.Id.Equals(appointment.ClientId));
    //                DateTime appointmentDateTime = appointment.Date.ToDateTime(appointment.Time);
    //                return new BusyAppointmentForUser
    //                {
    //                    Role = "Therapist",
    //                    Id = clientForDetails?.Id,
    //                    Date = appointmentDateTime,
    //                    Name = clientForDetails.FirstName + " " + clientForDetails.LastName,
    //                    Email = clientForDetails.Email,
    //                    PhoneNumber = clientForDetails.PhoneNumber,
    //                    Age = DateTime.Now.Year - clientForDetails.YearOfBirth


    //                };
    //            }
    //                ).ToList();
    //}
    public async Task<List<Therapist>> GetAllTherapists()
    {
        var result = await _blTherapist.ReadAllAsync();
        return result.ToList();
    }


}

