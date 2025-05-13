using System.Threading.Tasks;
using BL.Api;
using BL.Models;
using BL.Services;
using Dal.Api;
using Dal.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AppointmentsController : ControllerBase
{
    BusyAppointmentForUser _blBusyAppointments;
    private readonly IBLUser _blUser;
    private readonly IBLClient _blClient;
    IClient clients;
    ITherapist therapists;
    IBusyAppointment busyAppointments;
    //public AppointmentsController(IDal dal)
    //{
    ////    // _blUser = bLUser;
    // clients = dal.Clients;
    ////    //therapists = dal.Therapists;
    ////    busyAppointments = dal.BusyAppointments;
    //}
    //
    //
    //public AppointmentsController(IBL bL)
    //{
    //    //_blUser = blUser;
    //    _blClient = bL.BLClients;
    //    //therapists = dal.Therapists;
    //    _blUser = bL.BLUsers;
    //}
    public AppointmentsController(IDal dal, IBL bL)
    {
        clients = dal.Clients;
        _blClient = bL.BLClients;
        _blUser = bL.BLUsers;
    }

    [HttpGet]
    //Get all clients

    //public async Task<ActionResult<List<Client>>> GetAllClients()
    //{
    //    var result = await clients.ReadAllAsync();
    //    if (result == null)
    //    {
    //        return NotFound();
    //    }
    //    return Ok(result);
    //}

    //Get all therapists

    //public async Task<ActionResult<List<Therapist>>> GetAllTherapists()
    //{
    //    var result = await therapists.ReadAllAsync();
    //    if (result == null)
    //    {
    //        return NotFound();
    //    }
    //    return Ok(result);
    //}

    //Get all busy appointments for user

    //public async Task<ActionResult<List<BusyAppointmentForUser>>> GetAllBusyAppointmentsForUser([FromQuery] string id, [FromQuery] string name)
    //{
    //    var result =await _blUser.LogInSpecificUser(id, name);
    //    return Ok(result);
    //}

    //Get all busy appointments

    //public async Task<ActionResult<List<BusyAppointment>>> GetAllBusyAppointments()
    //{
    //    var result = await busyAppointments.ReadAllAsync();
    //    if (result == null)
    //    {
    //        return NotFound();
    //    }
    //    return Ok(result);
    //}

    //Scedule appointment

    //public async Task<ActionResult<bool>> SceduleAppointment([FromQuery] string therapistId, [FromQuery] DateOnly date, [FromQuery] TimeOnly time, [FromQuery] string clientId)
    //{
    //    var result = await _blClient.ScheduleAppointment(therapistId, date, time, clientId);
    //    return Ok(result);
    //}

    //Cancle appointment
    [HttpDelete]
   public async Task<ActionResult<bool>> CancleAppointment([FromQuery] int code, [FromQuery] string clientId)
   {
       var result = await _blClient.CancelAppointment(code, clientId);
       return Ok(result);
   }
}
