using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json;
using System.Text.Json.Serialization;
using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Serilog;
using TvShowTracker.Api;
using TvShowTracker.DataAccessLayer;
using TvShowTracker.Domain.Models;
using TvShowTracker.Domain.Services;
using TvShowTracker.Infrastructure.MappingProfile;
using TvShowTracker.Infrastructure.Services;
using TvShowTracker.Infrastructure.Validators;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((_, loggerConfiguration) =>
{
    loggerConfiguration.WriteTo.Console()
                       .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day);
});
// Add services to the container.
builder.Services.AddAutoMapper(mapperConfig =>
{
    mapperConfig.AddProfile(new MappingProfile(builder.Services.BuildServiceProvider()
                                                      .GetRequiredService<IHashingService>()));
});
builder.Services.AddControllers()
                .AddJsonOptions(options => options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull);
builder.Services.AddDbContext<TvShowTrackerDbContext>(opts => opts.UseSqlServer(builder.Configuration.GetConnectionString("TvShowTrackerDb")));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddRouting(opts => { opts.LowercaseUrls = true; });
builder.Services.AddHashingService(builder.Configuration["SaltKey"]);
//TODO: create injector for IValidators
builder.Services.AddScoped<IValidator<UserDto>, UserDtoValidator>();
builder.Services.AddScoped<IValidator<RegisterUserDto>, RegisterUserDtoValidator>();
builder.Services.AddScoped<IUserService,UserService>();

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
