//using System.Threading.Tasks;
//using BL.Api;
//using BL.Models;
//using BL.Services;
//using Dal.Api;
//using Dal.Models;
//using Dal.Services;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;

//namespace Server.Controllers;

//[Route("api/[controller]")]
//[ApiController]
//public class AppointmentsController : ControllerBase
//{
//    BusyAppointmentForUser _blBusyAppointments;
//    private readonly IBLUser _blUser;
//    private readonly IBLClient _blClient;
//    IClient clients;
//    ITherapist therapists;
//    IBusyAppointment busyAppointments;
//    //public AppointmentsController(IDal dal)
//    //{
//    ////    // _blUser = bLUser;
//    // clients = dal.Clients;
//    ////    //therapists = dal.Therapists;
//    ////    busyAppointments = dal.BusyAppointments;
//    //}
//    //
//    //
//    //public AppointmentsController(IBL bL)
//    //{
//    //    //_blUser = blUser;
//    //    _blClient = bL.BLClients;
//    //    //therapists = dal.Therapists;
//    //    _blUser = bL.BLUsers;
//    //}
//    public AppointmentsController(IDal dal, IBL bL)
//    {
//        clients = dal.Clients;
//        _blClient = bL.BLClients;
//        _blUser = bL.BLUsers;
//    }

//    [HttpGet]
//    //Get all clients

//    public async Task<ActionResult<List<Client>>> GetAllClients()
//    {
//        var result = await clients.ReadAllAsync();
//        if (result == null)
//        {
//            return NotFound();
//        }
//        return Ok(result);
//    }

//    //Get all therapists

//    //public async Task<ActionResult<List<Therapist>>> GetAllTherapists()
//    //{
//    //    var result = await therapists.ReadAllAsync();
//    //    if (result == null)
//    //    {
//    //        return NotFound();
//    //    }
//    //    return Ok(result);
//    //}

//    //Get all busy appointments for user
//    [HttpGet("GetAllBusyAppointmentsForUser")]
//    public async Task<ActionResult<List<BusyAppointmentForUser>>> GetAllBusyAppointmentsForUser([FromQuery] string id, [FromQuery] string name)
//    {
//        var result = await _blUser.LogInSpecificUser(id, name);
//        return Ok(result);
//    }


//    [HttpPost("CreateNewClient")]
//    public async Task<ActionResult<bool>> CreateNewClient([FromBody] Client client)
//    {
//        if (client == null)
//        {
//            return BadRequest("Client data is required.");
//        }

//        try
//        {
//            var result = await client.CreateAsync(client);
//            if (result)
//            {
//                return Ok(true);
//            }
//            else
//            {
//                return StatusCode(500, "Failed to create the client.");
//            }
//        }
//        catch (Exception ex)
//        {
//            // Log the exception (if logging is set up)
//            return StatusCode(500, $"An error occurred: {ex.Message}");
//        }
//    }
//    //Get all busy appointments

//    //public async Task<ActionResult<List<BusyAppointment>>> GetAllBusyAppointments()
//    //{
//    //    var result = await busyAppointments.ReadAllAsync();
//    //    if (result == null)
//    //    {
//    //        return NotFound();
//    //    }
//    //    return Ok(result);
//    //}

//    //Scedule appointment
//    [HttpPost]
//    public async Task<ActionResult<bool>> SceduleAppointment([FromQuery] string therapistId, [FromQuery] DateOnly date, [FromQuery] TimeOnly time, [FromQuery] string clientId)
//    {
//        var result = await _blClient.ScheduleAppointment(therapistId, date, time, clientId);
//        return Ok(result);
//    }



//    //Cancle appointment
//    //[HttpDelete]
//    //public async Task<ActionResult<bool>> DeleteAppointment([FromQuery] int code, [FromQuery] string clientId)
//    //{
//    //    var result = await _blClient.CancelAppointment(code, clientId);
//    //    return Ok(result);
//    //}
//}

using BL.Api;
using BL.Models;
using Dal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;

namespace Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AppointmentsController : ControllerBase
{
    private readonly IBLClient _blClient;
    private readonly IBLTherapist _blTherapist;
    private readonly IBLBusyAppointment _blBusyAppointment;
    private readonly IBLEmptyAppointment _blEmptyAppointment;
    private readonly IBLUser _blUser;

    public AppointmentsController(
        IBLClient blClient,
        IBLTherapist blTherapist,
        IBLBusyAppointment blBusyAppointment,
        IBLEmptyAppointment blEmptyAppointment,
        IBLUser blUser)
    {
        _blClient = blClient;
        _blTherapist = blTherapist;
        _blBusyAppointment = blBusyAppointment;
        _blEmptyAppointment = blEmptyAppointment;
        _blUser = blUser;
    }


    //[HttpGet("GetAllClients")]
    //public async Task<ActionResult<List<Client>>> GetAllClients()
    //{
    //    // Ensure the method exists in the IBLClient interface
    //    var result = await _blClient.GetAllClientsAsync();
    //    if (result == null)
    //    {
    //        return NotFound();
    //    }
    //    return Ok(result);
    //}

    [HttpGet("GetAllBusyAppointmentsForUser")]
    public async Task<ActionResult<List<BusyAppointmentForUser>>> GetAllBusyAppointmentsForUser([FromQuery] string id, [FromQuery] string name)
    {
        var result = await _blUser.LogInSpecificUser(id, name);
        return Ok(result);
    }

    [HttpPost("CreateNewClient")]
    public async Task<ActionResult<bool>> CreateNewClient([FromBody] Client client)
    {
        if (client == null)
        {
            return BadRequest("Client data is required.");
        }

        try
        {
            var result = await _blClient.CreateNewClient(client);
            if (result)
            {
                return Ok(true);
            }
            else
            {
                return StatusCode(500, "Failed to create the client.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex); // או לוג אחר
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }



    [HttpPost("ScheduleAppointment")]
    public async Task<ActionResult<bool>> ScheduleAppointment([FromQuery] string therapistId, [FromQuery] DateOnly date, [FromQuery] TimeOnly time, [FromQuery] string clientId)
    {
        var result = await _blClient.ScheduleAppointment(therapistId, date, time, clientId);
        return Ok(result);
    }

    [HttpDelete("CancelAppointment")]
    public async Task<ActionResult<bool>> CancelAppointment([FromQuery] int appointmentId, [FromQuery] string clientId)
    {
        try
        {
            var result = await _blClient.CancelAppointment(appointmentId, clientId);
            if (result)
            {
                return Ok(true);
            }
            else
            {
                return BadRequest(new { message = "Failed to cancel the appointment." });
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
        }
    }
    [HttpGet("Therapists")]
    public async Task<ActionResult<List<Therapist>>> GetAllTherapists()
    {
        var therapists = await _blTherapist.GetAllTherapists();
        return Ok(therapists);
    }
    [HttpGet("Client/AvailableTherapistsByDate")]
    public async Task<ActionResult<List<Therapist>>> GetAvailableTherapistsByDate([FromQuery] DateOnly date)
    {
        var therapists = await _blTherapist.GetAllTherapists(); // שכבר כתבת קודם
        var allEmptyAppointments = await _blEmptyAppointment.GetAllEmptyAppointments();

        var therapistIdsAvailable = allEmptyAppointments
            .Where(a => a.Date == date)
            .Select(a => a.TherapistId)
            .Distinct()
            .ToList();

        var availableTherapists = therapists
            .Where(t => therapistIdsAvailable.Contains(t.Id))
            .ToList();

        return Ok(availableTherapists);
    }
    [HttpGet("Therapist/BusyAppointments")]
    public async Task<ActionResult<List<BusyAppointmentForUser>>> GetBusyAppointmentsForTherapist([FromQuery] string therapistId)
    {
        var result = await _blBusyAppointment.GetAllAppointmentsForTherapist(therapistId);
        return Ok(result);
    }
    [HttpGet("AvailableHours")]
    public async Task<ActionResult<List<TimeOnly>>> GetAvailableHours([FromQuery] string therapistId, [FromQuery] DateOnly date)
    {
        var hours = await _blEmptyAppointment.GetAvailableHours(therapistId, date);
        return Ok(hours);
    }
    [HttpGet("Debug/EmptyAppointmentColumns")]
    public async Task<ActionResult<List<string>>> GetEmptyAppointmentColumnNames()
    {
        var columns = new List<string>();
        var conn = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename='C:\\Users\\User\\Desktop\\emotional-therapy-site\\Backend\\Dal\\dataBase\\dataBase.mdf';Integrated Security=True;Connect Timeout=30;Encrypt=True");

        try
        {
            await conn.OpenAsync();

            var cmd = new SqlCommand(
                "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'EmptyAppointments'",
                conn
            );

            var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                columns.Add(reader.GetString(0));
            }

            return Ok(columns);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
        finally
        {
            await conn.CloseAsync();
        }
    }



}
