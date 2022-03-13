using AuthForServicesExtension.AuthLogic;
using Microsoft.EntityFrameworkCore;
using PaymentService.Models;
using PaymentService.Repositories;
using PaymentService.Services.OrderService;
using PaymentService.Services.PaymentService;
using RabbitMqServiceExtension;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseInMemoryDatabase("Test");
});

// Add services to the container.
builder.Services.AddAuthService(builder.Configuration);

// Internal Services
builder.Services.AddHttpClient();
// Order services
builder.Services.Configure<OrderServiceSettings>(builder.Configuration.GetSection("OrderServiceSettings"));
builder.Services.AddScoped<IOrderService, OrderService>();

builder.Services.AddScoped<IPaymentService, StripePaymentService>();
builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("StripeSettings"));

builder.Services.AddTransient<IPaymentRepository, PaymentRepository>();

// RabbitMq
builder.Services.AddRabbitMqService(settingOptions =>
{
    var config = builder.Configuration.GetSection("RabbitMqSettings");
    settingOptions.HostName = config["HostName"];
    settingOptions.Port = Convert.ToInt32(config["Port"]);
});

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

app.Run();