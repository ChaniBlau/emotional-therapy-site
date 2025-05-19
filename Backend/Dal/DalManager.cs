using Dal.Api;
using Dal.Models;
using Dal.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal;

public class DalManager:IDal
{
    public IBusyAppointment? BusyAppointments { get;}
    public IClient? Clients {  get;}
    public IEmptyAppointment EmptyAppointments  { get;}
    public ITherapist Therapists { get;}
    public DalManager()
    {
        ServiceCollection services = new ServiceCollection();
        services.AddSingleton<DatabaseManager>();
        services.AddSingleton<IBusyAppointment,BusyAppoitmentService>();
        services.AddSingleton<IClient, ClientService>();
        services.AddSingleton<IEmptyAppointment,EmptyAppointmentService>();
        services.AddSingleton<ITherapist,TherapistService>();

        ServiceProvider servicesProvider = services.BuildServiceProvider();

        BusyAppointments = servicesProvider.GetService<IBusyAppointment>();
        Clients = servicesProvider.GetService<IClient>();
        EmptyAppointments = servicesProvider.GetService<IEmptyAppointment>();
        Therapists = servicesProvider.GetService<ITherapist>();
    }

}
