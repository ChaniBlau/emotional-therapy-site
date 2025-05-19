using Dal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Api;

public interface IBL
{
    public IBLBusyAppointment BLBusyAppointments { get; }
    public IBLClient BLClients { get; }
    public IBLEmptyAppointment BLEmptyAppointments { get; }
    public IBLTherapist BLTherapists { get; }
    public IBLUser BLUsers { get; }
}
