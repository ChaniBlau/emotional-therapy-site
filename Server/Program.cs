
using BL;
using BL.Api;
using BL.Services;
using Dal;
using Dal.Api;
using Dal.Models;
using Dal.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<IDal, DalManager>();
builder.Services.AddSingleton<IBL, BLManager>();
//builder.Services.AddSingleton<IBLUser, BLUserService>();


builder.Services.AddSingleton<DatabaseManager>();

builder.Services.AddControllers();

var app = builder.Build();


app.MapControllers();

app.Run();