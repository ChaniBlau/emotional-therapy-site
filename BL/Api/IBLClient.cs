using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Models;
using Dal.Models;

namespace BL.Api;

public interface IBLClient
{
    Task<bool> CreateNewClient(Client client);
    Task<List<BusyAppointmentForUser>> GetBusyAppointmentsForClient(string id, string name);
}


