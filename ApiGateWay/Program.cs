using AuthForServicesExtension.AuthLogic;

var builder = WebApplication.CreateBuilder(args);

// CORS
var  MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        configurePolicy =>
        {
            configurePolicy.WithOrigins(builder.Configuration.GetSection("Cors").Value)
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddAuthService(builder.Configuration);
// Internal Services
builder.Services.AddHttpClient();
// Product Service Configuration
// builder.Services.Configure<AuthServiceSettings>(builder.Configuration.GetSection("AuthServiceSettings"));
// builder.Services.AddScoped<IAuthService, AuthService>();

// builder.Services.AddAuthorization(options =>
// {
//     options.AddPolicy("customPolicy", policy =>
//         policy.RequireAuthenticatedUser());
// });


var app = builder.Build();
app.UseCors(MyAllowSpecificOrigins);
app.UseAuthentication();
app.UseAuthorization();
app.MapGet("/", () => "Api GateWay is running!");
app.MapReverseProxy();
app.Run();