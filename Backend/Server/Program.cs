using BL.Api;
using BL.Services;
using Dal.Api;
using Dal.Models;
using Dal.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

var builder = WebApplication.CreateBuilder(args);

// Register DB Context
builder.Services.AddDbContext<DatabaseManager>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register DAL services
builder.Services.AddScoped<IClient, ClientService>();
builder.Services.AddScoped<IBusyAppointment, BusyAppoitmentService>();
builder.Services.AddScoped<IEmptyAppointment, EmptyAppointmentService>();
builder.Services.AddScoped<ITherapist, TherapistService>();


// Register BL services
builder.Services.AddScoped<IBLClient, BLClientService>();
builder.Services.AddScoped<IBLUser, BLUserService>();
builder.Services.AddScoped<IBLBusyAppointment, BLBusyAppointmentService>();
builder.Services.AddScoped<IBLEmptyAppointment, BLEmptyAppointmentService>();
builder.Services.AddScoped<IBLTherapist, BLTherapistService>();
builder.Services.AddScoped<ITherapistWorkingHours,TherapistWorkingHoursService>();


// Register Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy => policy
            .WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod());
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<DatabaseManager>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

app.UseHttpsRedirection();
app.UseCors("AllowFrontend"); // ? כאן חשוב!
app.UseAuthorization();
app.MapControllers();

app.Run();

