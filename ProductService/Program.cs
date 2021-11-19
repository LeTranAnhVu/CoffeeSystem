using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProductService.AuthLogic;
using ProductService.Models;
using ProductService.Repositories;

var builder = WebApplication.CreateBuilder(args);

// AUTHENTICATION
builder.Services.AddAuthentication(AuthServiceScheme.DefaultName).AddScheme<AuthServiceSchemeOptions, AuthServiceHandler>(AuthServiceScheme.DefaultName,options => { });

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

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();