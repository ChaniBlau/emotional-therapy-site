using BL.Models;

namespace BL.Api
{
    public interface IBLUser
    {
        Task<List<BusyAppointmentForUser>> LogInSpecificUser(string id, string name);
        //    Task<List<BusyAppointmentForUser>> GetAllAppointmentsForClient(string clientId);
        //    Task<List<BusyAppointmentForUser>> GetAllAppointmentsForTherapist(string therapistId);
        //Task<List<BusyAppointmentForUser>> GetAllAppointmentsForTherapist(string therapistId);

    }
}
