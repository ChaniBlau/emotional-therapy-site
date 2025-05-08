using BL.Api;
using BL.Services;
using Dal;
using Dal.Api;
using Dal.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BL
{
    public class BLManager : IBL
    {
        public IBLBusyAppointment? BLBusyAppointments { get; }

        public IBLClient? BLClients { get; }

        public IBLEmptyAppointment? BLEmptyAppointments { get; }

        public IBLTherapist? BLTherapists { get; }
        public IBLUser? BLUsers { get; }

        public BLManager()
        {
            ServiceCollection services = new ServiceCollection();
            services.AddSingleton<IDal,DalManager>();

            services.AddSingleton<IBLBusyAppointment, BLBusyAppointmentService>();
            services.AddSingleton<IBLClient, BLClientService>();
            services.AddSingleton<IBLEmptyAppointment, BLEmptyAppointmentService>();
            services.AddSingleton<IBLTherapist, BLTherapistService>();
            services.AddSingleton<IBLUser, BLUserService>();

            ServiceProvider serviceProvider = services.BuildServiceProvider();
            BLBusyAppointments = serviceProvider.GetService<IBLBusyAppointment>();
            BLClients = serviceProvider.GetService<IBLClient>();
            BLEmptyAppointments = serviceProvider.GetService<IBLEmptyAppointment>();
            BLTherapists = serviceProvider.GetService<IBLTherapist>();
            BLUsers = serviceProvider.GetService<IBLUser>();
        }
    }
}
