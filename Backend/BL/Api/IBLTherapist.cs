using BL.Models;
using Dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BL.Api;

public interface IBLTherapist
{
    Task<List<Therapist>> GetAllTherapists();
    Task<Therapist?> AuthenticateTherapist(string id, string name);
    Task<List<Therapist>> GetAvailableTherapistsByDateWithWorkingHours(DateOnly date);


}
