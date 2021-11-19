using ApiGateWay.AuthLogic;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddAuthentication(AuthServiceScheme.DefaultName).AddScheme<AuthServiceSchemeOptions, AuthServiceHandler>(AuthServiceScheme.DefaultName,options => { });
;
// builder.Services.AddAuthorization(options =>
// {
//     options.AddPolicy("customPolicy", policy =>
//         policy.RequireAuthenticatedUser());
// });


var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();
app.MapGet("/", () => "Hello World!");
app.MapReverseProxy();
app.Run();