using BL.Api;
using BL.Models;
using Dal.Api;
using Dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BL.Services;

public class BLClientService : IBLClient
{
    IClient _clients;
    IBusyAppointment _busyAppointment;
    ITherapist _therapist;
    public BLClientService(IDal dal)
    {
        _clients = dal.Clients;
        _busyAppointment = dal.BusyAppointments;
        _therapist = dal.Therapists;
    }

    public async Task<bool> CreateNewClient(Client client)
    {
        if (client == null)
            throw new ArgumentNullException(nameof(client), "Client cannot be null");

        if (string.IsNullOrWhiteSpace(client.FirstName) || string.IsNullOrWhiteSpace(client.LastName))
            throw new ArgumentException("Client name cannot be empty", nameof(client.FirstName));

        if (string.IsNullOrWhiteSpace(client.PhoneNumber))
            throw new ArgumentException("Phone number cannot be empty", nameof(client.PhoneNumber));

        if (client.YearOfBirth < 1900 || client.YearOfBirth > DateTime.Now.Year)
            throw new ArgumentException("Year of birth is invalid", nameof(client.YearOfBirth));


        try
        {
            var mailAddress = new System.Net.Mail.MailAddress(client.Email);
        }
        catch
        {
            throw new ArgumentException("Invalid email", nameof(client.Email));
        }
        return await _clients.CreateAsync(client);

    }

    public async Task<List<BusyAppointmentForUser>> GetBusyAppointmentsForClient(string id, string name)
    {
        var busyAppointments = await _busyAppointment.ReadAllAsync();
        var therapists = await _therapist.ReadAllAsync();
        if (busyAppointments == null)
        {
            return new List<BusyAppointmentForUser>();
        }

        return busyAppointments
       .Where(app => app.TherapistId.Equals(id))
       .Select(appointment =>
       {
           var therapist = therapists.FirstOrDefault(t => t.Id.Equals(appointment.TherapistId));
           DateTime appointmentDateTime = appointment.Date.ToDateTime(appointment.Time);

           return new BusyAppointmentForUser
           {
               Date = appointmentDateTime,
               Name = therapist != null
                   ? therapist.FirstName + " " + therapist.LastName
                   : "Unknown Therapist"
           };
       })
       .ToList();
    }
public async Task<bool> ScheduleAppointment(string therapistId, DateOnly date, TimeOnly time, string clientId)
    {
       
        var availableAppointments = await _busyAppointment.ReadAllAsync();
        var appointmentToRemove = availableAppointments.FirstOrDefault(a =>
            a.TherapistId.Equals(therapistId, StringComparison.OrdinalIgnoreCase) &&
            a.Date == date &&
            a.Time == time);

        if (appointmentToRemove == null)
        {
            throw new Exception("The selected appointment is not available or the therapist is not free at this time.");
        }

        // Create a new busy appointment
        var newAppointment = new BusyAppointment
        {
            TherapistId = therapistId,
            ClientId = clientId,
            Date = date,
            Time = time
        };

        
        bool added = await _busyAppointment.CreateAsync(newAppointment);
        if (!added)
        {
            throw new Exception("Failed to create the appointment.");
        }

       
        bool removed = await _busyAppointment.DeleteAsync(appointmentToRemove.Code.ToString()); // Pass the ID (string)
        if (!removed)
        {
            throw new Exception("Failed to remove the appointment from available slots.");
        }

        return true;
    }
}

