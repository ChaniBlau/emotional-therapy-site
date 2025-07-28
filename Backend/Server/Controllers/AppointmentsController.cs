using BL.Api;
using BL.Models;
using BL.Services;
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
    private readonly ILogger<AppointmentsController> _logger;

    public AppointmentsController(
        IBLClient blClient,
        IBLTherapist blTherapist,
        IBLBusyAppointment blBusyAppointment,
        IBLEmptyAppointment blEmptyAppointment,
        IBLUser blUser,
        ILogger<AppointmentsController> logger)
    {
        _blClient = blClient;
        _blTherapist = blTherapist;
        _blBusyAppointment = blBusyAppointment;
        _blEmptyAppointment = blEmptyAppointment;
        _blUser = blUser;
        _logger = logger;
    }

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
            _logger.LogError(ex, "Error creating new client");
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }

    [HttpPost("ScheduleAppointment")]
    public async Task<ActionResult<bool>> ScheduleAppointment(
        [FromQuery] string therapistId,
        [FromQuery] string date,
        [FromQuery] string time,
        [FromQuery] string clientId)
    {
        try
        {
            _logger.LogInformation("ScheduleAppointment called with: therapistId={TherapistId}, date={Date}, time={Time}, clientId={ClientId}",
                therapistId, date, time, clientId);

            if (string.IsNullOrEmpty(therapistId))
            {
                _logger.LogWarning("TherapistId is null or empty");
                return BadRequest(new { message = "נדרש מזהה מטפל." });
            }

            if (string.IsNullOrEmpty(clientId))
            {
                _logger.LogWarning("ClientId is null or empty");
                return BadRequest(new { message = "נדרש מזהה לקוח." });
            }

            if (string.IsNullOrEmpty(date))
            {
                _logger.LogWarning("Date is null or empty");
                return BadRequest(new { message = "נדרש תאריך." });
            }

            if (string.IsNullOrEmpty(time))
            {
                _logger.LogWarning("Time is null or empty");
                return BadRequest(new { message = "נדרשת שעה." });
            }

            DateOnly parsedDate;
            if (!DateOnly.TryParse(date, out parsedDate))
            {
                _logger.LogWarning("Failed to parse date: {Date}", date);
                return BadRequest(new { message = "פורמט תאריך לא תקין. נדרש: YYYY-MM-DD." });
            }

            TimeOnly parsedTime;
            if (!TimeOnly.TryParse(time, out parsedTime))
            {
                _logger.LogWarning("Failed to parse time: {Time}", time);
                return BadRequest(new { message = "פורמט שעה לא תקין. נדרש: HH:MM:SS." });
            }

            _logger.LogInformation("Parsed values: date={ParsedDate}, time={ParsedTime}", parsedDate, parsedTime);

            var therapists = await _blTherapist.GetAllTherapists();
            var therapist = therapists.FirstOrDefault(t => t.Id.ToString().Trim().Equals(therapistId.ToString().Trim()));
            if (therapist == null)
            {
                _logger.LogWarning("Therapist not found: {TherapistId}", therapistId);
                return BadRequest(new { message = "המטפל לא נמצא." });
            }

            _logger.LogInformation("Checking available hours for therapist {TherapistId} on {Date}", therapistId, parsedDate);
            var availableHours = await _blEmptyAppointment.GetAvailableHours(therapistId, parsedDate);

            _logger.LogInformation("Available hours: {AvailableHours}", string.Join(", ", availableHours));
            _logger.LogInformation("Requested time: {RequestedTime}", parsedTime);

            var parsedTimeString = parsedTime.ToString("HH:mm");
            var availableTimeStrings = availableHours.Select(t => t.ToString("HH:mm")).ToList();

            if (!availableTimeStrings.Contains(parsedTimeString))
            {
                return BadRequest(new { message = "התור הנבחר אינו זמין או שהמטפל אינו פנוי בשעה זו." });
            }

            _logger.LogInformation("Attempting to schedule appointment");
            var result = await _blClient.ScheduleAppointment(therapistId, parsedDate, parsedTime, clientId);

            if (result)
            {
                _logger.LogInformation("Appointment scheduled successfully");
                return Ok(true);
            }
            else
            {
                _logger.LogWarning("Failed to schedule appointment - business logic returned false");
                return BadRequest(new { message = "נכשל בקביעת התור. אנא נסה שוב." });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error scheduling appointment for therapist {TherapistId}, date {Date}, time {Time}, client {ClientId}",
                therapistId, date, time, clientId);
            return StatusCode(500, new { message = $"אירעה שגיאה: {ex.Message}" });
        }
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
            _logger.LogError(ex, "Error cancelling appointment {AppointmentId} for client {ClientId}", appointmentId, clientId);
            return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
        }
    }

    [HttpGet("Therapists")]
    public async Task<ActionResult<List<Therapist>>> GetAllTherapists()
    {
        try
        {
            var therapists = await _blTherapist.GetAllTherapists();
            return Ok(therapists);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all therapists");
            return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
        }
    }

    [HttpGet("Client/AvailableTherapistsByDate")]
    public async Task<ActionResult<List<Therapist>>> GetAvailableTherapistsByDate([FromQuery] string date)
    {
        try
        {
            if (string.IsNullOrEmpty(date))
            {
                return BadRequest(new { message = "Date is required." });
            }

            if (!DateOnly.TryParse(date, out DateOnly parsedDate))
            {
                return BadRequest(new { message = "Invalid date format. Expected YYYY-MM-DD." });
            }

            var availableTherapists = await _blTherapist.GetAvailableTherapistsByDateWithWorkingHours(parsedDate);
            return Ok(availableTherapists);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting available therapists for date {Date}", date);
            return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
        }
    }

    [HttpGet("Therapist")]
    public async Task<ActionResult<List<BusyAppointmentForUser>>> GetBusyAppointmentsForTherapist([FromQuery] string therapistId)
    {
        try
        {
            var result = await _blBusyAppointment.GetAllAppointmentsForTherapist(therapistId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting busy appointments for therapist {TherapistId}", therapistId);
            return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
        }
    }

    [HttpGet("AvailableHours")]
    public async Task<ActionResult<List<TimeOnly>>> GetAvailableHours([FromQuery] string therapistId, [FromQuery] string date)
    {
        try
        {
            _logger.LogInformation("GetAvailableHours called with: therapistId={TherapistId}, date={Date}", therapistId, date);

            if (string.IsNullOrEmpty(therapistId))
            {
                return BadRequest(new { message = "Therapist ID is required." });
            }

            if (string.IsNullOrEmpty(date))
            {
                return BadRequest(new { message = "Date is required." });
            }

            if (!DateOnly.TryParse(date, out DateOnly parsedDate))
            {
                return BadRequest(new { message = "Invalid date format. Expected YYYY-MM-DD." });
            }

            var hours = await _blEmptyAppointment.GetAvailableHours(therapistId, parsedDate);
            _logger.LogInformation("Found {Count} available hours for therapist {TherapistId} on {Date}",
                hours.Count, therapistId, parsedDate);

            return Ok(hours);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting available hours for therapist {TherapistId} on date {Date}", therapistId, date);
            return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
        }
    }

    [HttpGet("GetAvailableHours")]
    public async Task<ActionResult<List<TimeOnly>>> GetAvailableHoursAlias([FromQuery] string therapistId, [FromQuery] string date)
    {
        return await GetAvailableHours(therapistId, date);
    }

    [HttpGet("LoginTherapist")]
    public async Task<IActionResult> LoginTherapist([FromQuery] string id, [FromQuery] string name)
    {
        try
        {
            var therapist = await _blTherapist.AuthenticateTherapist(id, name);
            if (therapist == null)
                return NotFound();

            return Ok(new
            {
                id = therapist.Id,
                name = therapist.FirstName + " " + therapist.LastName,
                role = "therapist"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during therapist login for id {Id}", id);
            return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
        }
    }

    [HttpGet("LoginClient")]
    public async Task<IActionResult> LoginClient([FromQuery] string id, [FromQuery] string name)
    {
        try
        {
            var client = await _blClient.AuthenticateClient(id, name);
            if (client == null)
                return NotFound();

            return Ok(new
            {
                id = client.Id,
                name = client.FirstName + " " + client.LastName,
                role = "client"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during client login for id {Id}", id);
            return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
        }
    }

    [HttpGet("GetByClient/{clientId}")]
    public async Task<ActionResult<List<object>>> GetClientAppointments(string clientId)
    {
        try
        {
            if (string.IsNullOrEmpty(clientId))
            {
                return BadRequest(new { message = "Client ID is required." });
            }

            var appointments = await _blBusyAppointment.GetAllAppointmentsForClient(clientId);
            return Ok(appointments);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting appointments for client {ClientId}", clientId);
            return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
        }
    }
}