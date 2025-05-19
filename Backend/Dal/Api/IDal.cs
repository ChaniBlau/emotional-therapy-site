using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal.Api;

public interface IDal
{
    public IBusyAppointment? BusyAppointments { get; }
    public IClient? Clients { get; }
    public IEmptyAppointment? EmptyAppointments { get; }
    public ITherapist? Therapists { get; }
}
