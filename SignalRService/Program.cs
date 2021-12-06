using RabbitMqServiceExtension;
using SignalRService.BackgroundServices;
using SignalRService.Hubs;

var builder = WebApplication.CreateBuilder(args);

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

// RabbitMq
builder.Services.AddRabbitMqService(settingOptions =>
{
    var config = builder.Configuration.GetSection("RabbitMqSettings");
    settingOptions.HostName = config["HostName"];
    settingOptions.Port = Convert.ToInt32(config["Port"]);
});


// Background services
builder.Services.AddHostedService<SignalRTestMessageBackgroundService>();
builder.Services.AddHostedService<TestMessageSubscriber>();
builder.Services.AddHostedService<OrderChangeSubscriber>();
// Add services to the container.
builder.Services.AddSignalR();
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

app.UseAuthorization();

app.UseCors(MyAllowSpecificOrigins);

app.MapControllers();
app.MapHub<CommonHub>("/api/signalr/commonHub");
app.Run();