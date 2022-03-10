using AuthForServicesExtension.AuthLogic;
using PaymentService.Services.OrderService;
using PaymentService.Services.PaymentService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthService(builder.Configuration);

// Internal Services
builder.Services.AddHttpClient();
// Order services
builder.Services.Configure<OrderServiceSettings>(builder.Configuration.GetSection("OrderServiceSettings"));
builder.Services.AddScoped<IOrderService, OrderService>();

builder.Services.AddScoped<IPaymentService, StripePaymentService>();
builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("StripeSettings"));

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