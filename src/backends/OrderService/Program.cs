using System.Reflection;
using AuthForServicesExtension.AuthLogic;
using Microsoft.EntityFrameworkCore;
using OrderService.BackgroundServices;
using OrderService.Models;
using OrderService.Repositories;
using OrderService.SeedData;
using OrderService.Services.OrderProductService;
using OrderService.Services.ProductService;
using RabbitMqServiceExtension;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Database
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection"));
});

builder.Services.AddAuthService(builder.Configuration);

// Internal Services
builder.Services.AddHttpClient();
// Product Service Configuration
builder.Services.Configure<ProductServiceSettings>(builder.Configuration.GetSection("ProductServiceSettings"));
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IOrderProductService, OrderProductService>();

// Repository
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

// AutoMapping
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// RabbitMq
builder.Services.AddRabbitMqService(settingOptions =>
{
    var config = builder.Configuration.GetSection("RabbitMqSettings");
    settingOptions.HostName = config["HostName"];
    settingOptions.Port = Convert.ToInt32(config["Port"]);
});

builder.Services.AddHostedService<PaymentSubscriber>();

builder.Services.AddControllers();
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

// app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
PrepDb.PrepPopulation(app, app.Environment.IsProduction());

app.Run();