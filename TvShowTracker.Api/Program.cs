using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using TvShowTracker.Api;
using TvShowTracker.DataAccessLayer;
using TvShowTracker.Domain.Models;
using TvShowTracker.Domain.Services;
using TvShowTracker.Infrastructure.Extensions;
using TvShowTracker.Infrastructure.MappingProfile;
using TvShowTracker.Infrastructure.Services;
using TvShowTracker.Infrastructure.Validators;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                }).AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = false;
                    var jwtConfiguration = builder.Configuration.GetSection("JwtConfiguration").Get<JwtConfiguration>();
                    options.TokenValidationParameters = new()
                    {
                        ValidAudience = jwtConfiguration.Audience,
                        ValidIssuer = jwtConfiguration.Issuer,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfiguration.Secret))
                    };
                });


builder.Host.ConfigureSerilog();
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
builder.Services.AddHttpContextAccessor();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

builder.Services.AddSingleton(builder.Configuration.GetSection("JwtConfiguration").Get<JwtConfiguration>());
builder.Services.AddRouting(opts => { opts.LowercaseUrls = true; });
builder.Services.AddHashingService(builder.Configuration["SaltKey"]);
//TODO: create injector for IValidators
builder.Services.AddScoped<IValidator<UserModel>, UserModelValidator>();
builder.Services.AddScoped<IValidator<RegisterUserModel>, RegisterUserModelValidator>();
builder.Services.AddScoped<IUserService,UserService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();

app.Run();
