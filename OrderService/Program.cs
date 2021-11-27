using System.Reflection;
using AuthForServicesExtension.AuthLogic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OrderService.Models;
using OrderService.Repositories;
using OrderService.Services.ProductService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Database
builder.Services.AddDbContext<AppDbContext>(options =>
{
    // options.UseInMemoryDatabase("Test");
    options.UseSqlite("Filename=Order.db", options =>
    {
        options.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
    });
});

builder.Services.AddAuthService(builder.Configuration);

// Internal Services
builder.Services.AddHttpClient();
// Product Service Configuration
builder.Services.Configure<ProductServiceSettings>(builder.Configuration.GetSection("ProductServiceSettings"));
builder.Services.AddScoped<IProductService, ProductService>();

// Repository
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

// AutoMapping
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

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