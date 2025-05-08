using System;
using System.Collections.Generic;

namespace Dal.Models;

public partial class Therapist
{
    public string Id { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Specialization { get; set; } = null!;

    public string? City { get; set; }

    public int StartingWorkYear { get; set; }

    public double SalaryPerMonth { get; set; }

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public virtual ICollection<BusyAppointment> BusyAppointments { get; set; } = new List<BusyAppointment>();

    public virtual ICollection<EmptyAppointment> EmptyAppointments { get; set; } = new List<EmptyAppointment>();

    public virtual ICollection<TherapistWorkingHour> TherapistWorkingHours { get; set; } = new List<TherapistWorkingHour>();
}
