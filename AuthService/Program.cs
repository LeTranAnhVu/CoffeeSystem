using System;
using System.Text;
using AuthService.Helpers;
using AuthService.Models;
using AuthService.SeedData;
using AuthService.Services;
using AuthService.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var services = builder.Services;
// Add services to the container.
services.AddControllers();
// CORS
var  MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        configurePolicy =>
        {
            var origins = builder.Configuration.GetSection("Cors").Value.Split(";");
            configurePolicy.WithOrigins(origins)
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});

// DATABASE
services.AddDbContext<AppDbContext>(options =>
{
    options.UseInMemoryDatabase("Test");
});

// IDENTITY
services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.User.RequireUniqueEmail = true;
    // Development Only.
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 4;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
})
    .AddEntityFrameworkStores<AppDbContext>();

// JWT AUTHENTICATION
services.Configure<JwtSettings>(configuration.GetSection("Jwt"))
        .Configure<JwtSettings>(settings => settings.Algorithm = SecurityAlgorithms.HmacSha256Signature);

services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    {
        var issuer = configuration.GetSection("Jwt:Issuer").Value ?? throw new NullReferenceException();
        var audience = configuration.GetSection("Jwt:Audience").Value ?? throw new NullReferenceException();;
        var secretKey = configuration.GetSection("Jwt:SecretKey").Value ?? throw new NullReferenceException();;
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

        options.TokenValidationParameters = new TokenValidationParameters
        {
           ValidIssuer = issuer,
           ValidAudience = audience,
           IssuerSigningKey = securityKey,
           ValidateIssuer = true,
           ValidateAudience = true,
        };
    });

// SERVICES
services.AddScoped<JwtFacade>();
services.AddScoped<IIdentityService, IdentityService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
var app = builder.Build();
app.UseCors(MyAllowSpecificOrigins);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

PrepDb.PrepPopulation(app, app.Environment.IsProduction());
app.Run();
