using System;
using System.Collections.Generic;

namespace Dal.Models;

public partial class TherapistWorkingHour
{
    public int Id { get; set; }

    public string TherapistId { get; set; } = null!;

    public int DayOfWeek { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public virtual Therapist Therapist { get; set; } = null!;
}
