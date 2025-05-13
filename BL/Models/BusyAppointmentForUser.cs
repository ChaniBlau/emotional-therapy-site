using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BL.Models;

public class BusyAppointmentForUser

{
    [JsonIgnore]
    public string? Id { get; set; }
    public DateTime Date { get; set; }
    public string Name { get; set; }
    [JsonIgnore]
    public string? Email { get; set; }
    [JsonIgnore]
    public string? PhoneNumber { get; set; }
    [JsonIgnore]
    public int? Age { get; set; }

   
}
