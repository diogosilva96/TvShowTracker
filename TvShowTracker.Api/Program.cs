using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using Microsoft.EntityFrameworkCore;
using Serilog;
using TvShowTracker.Api.Services;
using TvShowTracker.DataAccessLayer;
using TvShowTracker.Infrastructure.MappingProfile;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((_, loggerConfiguration) =>
{
    loggerConfiguration.WriteTo.Console()
                       .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day);
});
// Add services to the container.
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddControllers();
builder.Services.AddDbContext<TvShowTrackerDbContext>(opts => opts.UseSqlServer(builder.Configuration.GetConnectionString("TvShowTrackerDb")));
builder.Services.AddSingleton<IHashingService>(_ => new HashingService(builder.Configuration["SaltKey"]));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
