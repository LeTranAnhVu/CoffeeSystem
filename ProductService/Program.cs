using Microsoft.EntityFrameworkCore;
using ProductService.Models;
using ProductService.Repositories;
using ProductService.SeedData;

var builder = WebApplication.CreateBuilder(args);

// DB
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseInMemoryDatabase("Test");
});

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
// REPOSITORY
builder.Services.AddScoped<IProductRepository, ProductRepository>();

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