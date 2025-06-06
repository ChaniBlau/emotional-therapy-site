﻿using BL.Api;
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
    IEmptyAppointment _emptyAppointment;
    public BLClientService(IDal dal)
    {
        _clients = dal.Clients;
        _busyAppointment = dal.BusyAppointments;
        _therapist = dal.Therapists;
        _emptyAppointment = dal.EmptyAppointments;
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


    public async Task<List<BusyAppointmentForUser>> GetBusyAppointmentsForClient(string id, string name)
    {
        var busyAppointments = await _busyAppointment.ReadAllAsync();
        var therapists = await _therapist.ReadAllAsync();
        var clients = await _clients.ReadAllAsync(); 

        if (busyAppointments == null)
        {
            return new List<BusyAppointmentForUser>();
        }

        return busyAppointments
       .Where(app => app.ClientId.Trim().Equals(id.Trim(), StringComparison.OrdinalIgnoreCase))
       .Select(appointment =>
       {
           var therapistForDetails = therapists.FirstOrDefault(t => t.Id.Equals(appointment.TherapistId));
           var clientForDetails = clients.FirstOrDefault(c => c.Id.Trim().Equals(appointment.ClientId.Trim(), StringComparison.OrdinalIgnoreCase));
           DateTime appointmentDateTime = appointment.Date.ToDateTime(appointment.Time);

           return new BusyAppointmentForUser
           {
               Id = appointment.Code.ToString(),
               Role = "Client",
               Date = appointmentDateTime,
               Name = therapistForDetails.FirstName + " " + therapistForDetails.LastName,
               ClientName = clientForDetails!= null ? clientForDetails.FirstName + " " + clientForDetails.LastName : ""
           };
       })
       .ToList();
    }
    //קביעת תור
    public async Task<bool> ScheduleAppointment(string therapistId, DateOnly date, TimeOnly time, string clientId)
    {
        var emptyAppointments = await _emptyAppointment.ReadAllAsync();
        var appointmentToScedule = emptyAppointments.FirstOrDefault(a =>
            a.TherapistId.Trim().Equals(therapistId.Trim(), StringComparison.OrdinalIgnoreCase)
            &&
            a.Date == date &&
            a.Time == time);

        if (appointmentToScedule == null)
        {
            throw new Exception("The selected appointment is not available or the therapist is not free at this time.");
        }
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

        bool removed = await _emptyAppointment.DeleteAsync(appointmentToScedule.Code.ToString());
        if (!removed)
        {
            throw new Exception("Failed to remove the appointment from available slots.");
        }

        return true;
    }

    public async Task<bool> CancelAppointment(int appointmentId, string clientId)
    {
        var busyAppointments = await _busyAppointment.ReadAllAsync();
        var appointmentToRemove = busyAppointments.FirstOrDefault(a =>
            a.Code == appointmentId &&
            a.ClientId.Trim().Equals(clientId.Trim(), StringComparison.OrdinalIgnoreCase));

        if (appointmentToRemove == null)
        {
            throw new Exception("The selected appointment does not exist.");
        }

        var newAppointment = new EmptyAppointment
        {
            TherapistId = appointmentToRemove.TherapistId,
            Date = appointmentToRemove.Date,
            Time = appointmentToRemove.Time
        };

        bool added = await _emptyAppointment.CreateAsync(newAppointment);
        if (!added)
        {
            throw new Exception("Failed to move the appointment to the empty table.");
        }

        bool removed = await _busyAppointment.DeleteAsync(appointmentToRemove.Code.ToString());
        if (!removed)
        {
            throw new Exception("Failed to remove the appointment from the busy table.");
        }

        return true;
    }

}





