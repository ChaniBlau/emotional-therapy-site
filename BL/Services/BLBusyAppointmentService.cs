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

public class BLBusyAppointmentService : IBLBusyAppointment
{

    private readonly IBusyAppointment _busyAppointment;
    private readonly ITherapist _therapist;
    public BLBusyAppointmentService(IDal dal)
    {
        _busyAppointment = dal.BusyAppointments;
        _therapist = dal.Therapists;
    }

   
}
