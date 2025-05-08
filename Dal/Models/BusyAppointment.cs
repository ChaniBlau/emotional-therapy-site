using System;
using System.Collections.Generic;

namespace Dal.Models;

public partial class BusyAppointment
{
    public int Code { get; set; }

    public DateOnly Date { get; set; }

    public TimeOnly Time { get; set; }

    public string TherapistId { get; set; } = null!;

    public string ClientId { get; set; } = null!;

    public virtual Client Client { get; set; } = null!;

    public virtual Therapist Therapist { get; set; } = null!;
}
