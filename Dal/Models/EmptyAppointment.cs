using System;
using System.Collections.Generic;

namespace Dal.Models;

public partial class EmptyAppointment
{
    public int Code { get; set; }

    public DateOnly Date { get; set; }

    public TimeOnly Time { get; set; }

    public string TherapistId { get; set; } = null!;

    public virtual Therapist Therapist { get; set; } = null!;
}
