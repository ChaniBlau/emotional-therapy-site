
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
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost5173",
        policy => policy.WithOrigins("http://localhost:5173")
                        .AllowAnyHeader()
                        .AllowAnyMethod());
});

builder.Services.AddSingleton<DatabaseManager>();

builder.Services.AddControllers();

var app = builder.Build();
app.UseCors("AllowLocalhost5173");

app.UseAuthorization();

app.MapControllers();

app.Run();