using System;
using System.Collections.Generic;

namespace Dal.Models;

public partial class Client
{
    public string Id { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public int YearOfBirth { get; set; }

    public string? Email { get; set; }

    public string? City { get; set; }

    public string? PhoneNumber { get; set; }

    public string TherapistId { get; set; } = null!;

    public virtual ICollection<BusyAppointment> BusyAppointments { get; set; } = new List<BusyAppointment>();
}
