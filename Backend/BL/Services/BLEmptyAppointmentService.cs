using BL.Api;
using Dal.Models;
using Dal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services;

public class BLEmptyAppointmentService : IBLEmptyAppointment
{
    private readonly IEmptyAppointment _emptyAppointment;
    private readonly IBusyAppointment _busyAppointment;
    public BLEmptyAppointmentService(IEmptyAppointment emptyAppointment, IBusyAppointment busyAppointment)
    {
        _emptyAppointment = emptyAppointment;
        _busyAppointment = busyAppointment;
    }


    public async Task<List<EmptyAppointment>> GetAllEmptyAppointments()
    {
        var emptyAppointments = await _emptyAppointment.ReadAllAsync();
        return emptyAppointments.ToList();
    }
    public async Task<List<TimeOnly>> GetAvailableHours(string therapistId, DateOnly date)
    {
        var allEmpty = await _emptyAppointment.ReadAllAsync();
        var allBusy = await _busyAppointment.ReadAllAsync();

        var available = allEmpty
            .Where(e => e.TherapistId == therapistId && e.Date == date)
            .Select(e => e.Time)
            .ToList();

        var taken = allBusy
            .Where(b => b.TherapistId == therapistId && b.Date == date)
            .Select(b => b.Time)
            .ToList();

        return available
            .Where(t => !taken.Contains(t))
            .OrderBy(t => t)
            .ToList();
    }
}

