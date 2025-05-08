using BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Api;

public interface IBLUser
{
    Task<List<BusyAppointmentForUser>> LogInSpecificUser(string name, string id);
    
}
