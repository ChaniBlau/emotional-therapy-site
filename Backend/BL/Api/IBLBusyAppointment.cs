using BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Api;

public interface IBLBusyAppointment
{
    Task<List<BusyAppointmentForUser>> GetAllAppointmentsForClient(string clientId);
    Task<List<BusyAppointmentForUser>> GetAllAppointmentsForTherapist(string therapistId);

}
