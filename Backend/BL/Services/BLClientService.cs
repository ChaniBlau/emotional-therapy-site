using BL.Api;
using BL.Models;
using Dal.Api;
using Dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services;

public class BLClientService : IBLClient
{
    private readonly IClient _clients;
    private readonly IBusyAppointment _busyAppointment;
    private readonly ITherapist _therapist;
    private readonly IEmptyAppointment _emptyAppointment;

    public BLClientService(IClient clients, IBusyAppointment busyAppointment, ITherapist therapist, IEmptyAppointment emptyAppointment)
    {
        _clients = clients;
        _busyAppointment = busyAppointment;
        _therapist = therapist;
        _emptyAppointment = emptyAppointment;
    }

    public async Task<bool> CreateNewClient(Client client)
    {
        if (client == null)
            throw new ArgumentNullException(nameof(client), "Client cannot be null");

        if (string.IsNullOrWhiteSpace(client.FirstName) || string.IsNullOrWhiteSpace(client.LastName))
            throw new ArgumentException("Client name cannot be empty");

        if (string.IsNullOrWhiteSpace(client.PhoneNumber))
            throw new ArgumentException("Phone number cannot be empty");

        if (client.YearOfBirth < 1900 || client.YearOfBirth > DateTime.Now.Year)
            throw new ArgumentException("Year of birth is invalid");

        if (!string.IsNullOrWhiteSpace(client.Email))
        {
            try
            {
                var mailAddress = new System.Net.Mail.MailAddress(client.Email);
            }
            catch
            {
                throw new ArgumentException("Invalid email format");
            }
        }

        try
        {
            return await _clients.CreateAsync(client);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to create client", ex);
        }
    }

    public async Task<bool> ScheduleAppointment(string therapistId, DateOnly date, TimeOnly time, string clientId)
    {
        try
        {
            therapistId = therapistId?.Trim();
            clientId = clientId?.Trim();

            if (string.IsNullOrEmpty(therapistId))
                throw new ArgumentException("Therapist ID cannot be empty");

            if (string.IsNullOrEmpty(clientId))
                throw new ArgumentException("Client ID cannot be empty");

            var therapist = await _therapist.ReadByIdAsync(therapistId);
            if (therapist == null)
                throw new Exception($"Therapist with ID {therapistId} not found");

            var client = await _clients.ReadByIdAsync(clientId);
            if (client == null)
                throw new Exception($"Client with ID {clientId} not found");
            var emptyAppointments = await _emptyAppointment.ReadAllAsync();
            Console.WriteLine("==> בדיקת זמינות תורים:");
   
            Console.WriteLine($"==> TherapistId to match: '{therapist.Id}'");
            Console.WriteLine($"==> Date to match: {date}, Time to match: {time}");

            var appointmentToSchedule = emptyAppointments?.FirstOrDefault(a =>
                a.TherapistId.ToString().Trim().ToLower() == therapist.Id.ToString().Trim().ToLower() &&
                a.Date == date &&
                a.Time == time
            );

            if (appointmentToSchedule == null)
            {
                Console.WriteLine("❌ לא נמצא תור פנוי תואם.");
            }
            else
            {
                Console.WriteLine("✅ נמצא תור תואם:");
                Console.WriteLine($"TherapistId: '{appointmentToSchedule.TherapistId}', Date: {appointmentToSchedule.Date}, Time: {appointmentToSchedule.Time}");
            }

            if (appointmentToSchedule == null)
            {
                throw new Exception("The selected appointment slot is not available or the therapist is not free at this time");
            }

            var existingAppointments = await _busyAppointment.ReadAllAsync();
            var conflictingAppointment = existingAppointments?.FirstOrDefault(a =>
                a.ClientId.Trim().Equals(clientId, StringComparison.OrdinalIgnoreCase) &&
                a.Date == date &&
                a.Time == time);

            if (conflictingAppointment != null)
            {
                throw new Exception("You already have an appointment scheduled at this time");
            }

            var newBusyAppointment = new BusyAppointment
            {
                TherapistId = therapistId,
                ClientId = clientId,
                Date = date,
                Time = time
            };

            Console.WriteLine($"Creating busy appointment: TherapistId={therapistId}, ClientId={clientId}, Date={date}, Time={time}");

            bool appointmentCreated = await _busyAppointment.CreateAsync(newBusyAppointment);
            if (!appointmentCreated)
            {
                throw new Exception("Failed to create the appointment in busy appointments table");
            }

            Console.WriteLine("Busy appointment created successfully");

            bool appointmentRemoved = await _emptyAppointment.DeleteAsync(appointmentToSchedule.Code.ToString());
            if (!appointmentRemoved)
            {
                Console.WriteLine("Failed to remove from empty appointments, attempting rollback");

                var createdAppointments = await _busyAppointment.ReadAllAsync();
                var createdAppointment = createdAppointments?.FirstOrDefault(a =>
                    a.TherapistId.Trim().Equals(therapistId, StringComparison.OrdinalIgnoreCase) &&
                    a.ClientId.Trim().Equals(clientId, StringComparison.OrdinalIgnoreCase) &&
                    a.Date == date &&
                    a.Time == time);

                if (createdAppointment != null)
                {
                    await _emptyAppointment.DeleteAsync(createdAppointment.Code.ToString().Trim());
                }

                throw new Exception("Failed to remove the appointment from available slots");
            }

            Console.WriteLine("Empty appointment removed successfully");

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in ScheduleAppointment: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
            throw; 
        }
    }

    public async Task<bool> CancelAppointment(int appointmentId, string clientId)
    {
        try
        {
            Console.WriteLine($"Canceling appointment: ID={appointmentId}, ClientId='{clientId}'");

            var busyAppointments = await _busyAppointment.ReadAllAsync();
            var normalizedClientId = clientId?.Trim().ToLower();

            var appointmentToRemove = busyAppointments.FirstOrDefault(a =>
                a.Code == appointmentId &&
                a.ClientId?.Trim().ToLower() == normalizedClientId);

            if (appointmentToRemove == null)
            {
                throw new Exception("The selected appointment does not exist or does not belong to you");
            }

            // צור תור ריק חדש
            var newEmptyAppointment = new EmptyAppointment
            {
                TherapistId = appointmentToRemove.TherapistId,
                Date = appointmentToRemove.Date,
                Time = appointmentToRemove.Time
            };

            // הוסף לטבלת התורים הריקים
            bool emptyAdded = await _emptyAppointment.CreateAsync(newEmptyAppointment);
            if (!emptyAdded)
            {
                throw new Exception("Failed to move the appointment to available slots");
            }

            // הסר מטבלת התורים התפוסים
            bool busyRemoved = await _busyAppointment.DeleteAsync(appointmentToRemove.Code.ToString());
            if (!busyRemoved)
            {
                throw new Exception("Failed to remove the appointment from busy appointments");
            }

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in CancelAppointment: {ex.Message}");
            throw;
        }
    }

    public async Task<Client?> AuthenticateClient(string id, string name)
    {
        var clients = await _clients.ReadAllAsync();
        return clients.FirstOrDefault(t => t.Id.Trim() == id.Trim());
    }
}