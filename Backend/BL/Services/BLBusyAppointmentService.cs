using BL.Api;
using BL.Models;
using Dal.Api;
using Dal.Models;

namespace BL.Services
{
    public class BLBusyAppointmentService : IBLBusyAppointment
    {

        private readonly IBusyAppointment _busyAppointment;
        private readonly ITherapist _therapist;
        private readonly IClient _client;
        private readonly IEmptyAppointment _emptyAppointment;


        public BLBusyAppointmentService(IClient client, ITherapist therapist, IBusyAppointment busyAppointment, IEmptyAppointment emptyAppointment)
        {
            _client = client;
            _therapist = therapist;
            _busyAppointment = busyAppointment;
            _emptyAppointment = emptyAppointment;
        }

        public async Task<List<BusyAppointmentForUser>> GetAllAppointmentsForClient(string clientId)
        {
            var client = await _client.ReadByIdAsync(clientId);
            if (client == null) return new List<BusyAppointmentForUser>();

            var allAppointments = await _busyAppointment.ReadAllAsync();

            var result = new List<BusyAppointmentForUser>();

            foreach (var a in allAppointments.Where(a => string.Equals(a.ClientId.Trim(), clientId.Trim(), StringComparison.OrdinalIgnoreCase)))
            {
                var therapist = await _therapist.ReadByIdAsync(a.TherapistId);

                result.Add(new BusyAppointmentForUser
                {
                    Id = client.Id,
                    Name = $"{therapist.FirstName} {therapist.LastName}",
                    Date = a.Date.ToDateTime(a.Time),
                    Role = "Client",
                    Age = DateTime.Now.Year - client.YearOfBirth,
                    Email = client.Email,
                    PhoneNumber = client.PhoneNumber,
                    ClientName = null
                });
            }

            return result;
        }


        public async Task<List<BusyAppointmentForUser>> GetAllAppointmentsForTherapist(string therapistId)
        {
            var therapist = await _therapist.ReadByIdAsync(therapistId);
            if (therapist == null)
                return new List<BusyAppointmentForUser>();

            var allAppointments = await _busyAppointment.ReadAllAsync();

            // סינון תורים של המטפל הספציפי
            var relevantAppointments = allAppointments.Where(a => a.TherapistId == therapistId);

            var result = new List<BusyAppointmentForUser>();

            foreach (var appointment in relevantAppointments)
            {
                var client = await _client.ReadByIdAsync(appointment.ClientId);
                if (client == null)
                    continue;

                result.Add(new BusyAppointmentForUser
                {
                    Id = client.Id,
                    Name = $"{client.FirstName} {client.LastName}",
                    Date = appointment.Date.ToDateTime(appointment.Time),
                    Role = "Client",
                    Age = DateTime.Now.Year - client.YearOfBirth,
                    Email = client.Email,
                    PhoneNumber = client.PhoneNumber,
                    ClientName = null // או להשאיר ריק, או להכניס שם מקוצר
                });
            }

            return result;
        }

    }
        
}