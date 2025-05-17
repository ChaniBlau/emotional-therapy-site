using BL.Api;
using BL.Models;
using Dal.Api;
using Dal.Models;
using Dal.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services;

public class BLUserService : IBLUser
{
    private readonly IClient _client;
    private readonly ITherapist _therapist;
    private readonly IBusyAppointment _busyAppointment;
    private readonly IBLBusyAppointment _busyAppointmentService;
    private readonly IBLClient _blClient;
    private readonly IBLTherapist _blTherapist;


    public BLUserService(IDal dal)
    {
        _client = dal.Clients;
        _therapist = dal.Therapists;
        _busyAppointment = dal.BusyAppointments;
        _busyAppointmentService = new BLBusyAppointmentService(dal);
        _blClient = new BLClientService(dal);
        _blTherapist = new BLTherapistService(dal);


    }
    public async Task<List<BusyAppointmentForUser>> LogInSpecificUser(string id, string name)
    {
        var busyAppointments = await _busyAppointment.ReadAllAsync();

        //bool isTherapist = busyAppointments.Any(t => t.TherapistId.Equals(id));
        //bool isClient = busyAppointments.Any(t => t.ClientId.Equals(id));


        bool isTherapist = busyAppointments.Any(t => t.TherapistId.Trim().Equals(id.Trim(), StringComparison.OrdinalIgnoreCase));
        bool isClient = busyAppointments.Any(t => t.ClientId.Trim().Equals(id.Trim(), StringComparison.OrdinalIgnoreCase));

        if (!isClient && !isTherapist)
        {
            throw new Exception("User or therapist not found or credentials are incorrect.");
        }

        if (isTherapist)
        {
            return await _blTherapist.GetBusyAppointmentsForTherapist(id, name);
        }
        if (isClient)
        {
            return await _blClient.GetBusyAppointmentsForClient(id,name);
        }
        else return null;
    }
}
