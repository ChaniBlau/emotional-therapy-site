﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BL.Models;

public class BusyAppointmentForUser

{
    public string? Id { get; set; }
    public DateTime Date { get; set; }
    public string Name { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public int? Age { get; set; }
    public string? Role { get; set; }
    public string? ClientName { get; set; } // <-- הוסף שדה זה


}
