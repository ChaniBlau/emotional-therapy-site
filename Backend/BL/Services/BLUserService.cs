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
        var appointments = new List<BusyAppointmentForUser>();

        var client = await _client.ReadByIdAsync(id);
        if (client != null)
        {
            var clientAppointments = await _busyAppointmentService.GetAllAppointmentsForClient(id);
            appointments.AddRange(clientAppointments);
        }

        var therapist = await _therapist.ReadByIdAsync(id);
        if (therapist != null)
        {
            var therapistAppointments = await _busyAppointmentService.GetAllAppointmentsForTherapist(id);
            appointments.AddRange(therapistAppointments);
        }

        return appointments;
    }
}
