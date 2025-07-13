using Dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Api;

public interface IBLEmptyAppointment
{
    Task<List<EmptyAppointment>> GetAllEmptyAppointments();
    Task<List<TimeOnly>> GetAvailableHours(string therapistId, DateOnly date);


}
