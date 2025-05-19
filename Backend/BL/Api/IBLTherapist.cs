using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Models;

namespace BL.Api;

public interface IBLTherapist
{
    Task<List<BusyAppointmentForUser>> GetBusyAppointmentsForTherapist(string id, string name);

}
