using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SignalRService.Hubs;

namespace SignalRService.Controllers;

[Route("/api/signalr/[controller]")]
[ApiController]
public class TestController : ControllerBase
{
    private readonly IHubContext<CommonHub, ICommonHub> _commonHubContext;

    public TestController(IHubContext<CommonHub, ICommonHub> hubContext)
    {
        _commonHubContext = hubContext;
    }

    // GET
    public async Task<IActionResult> Test()
    {
        var getRandom = new Random();
        await _commonHubContext.Clients.All.TestMessage($"Welcome to Brian's coffee system - websocket is connected. Current temperature is {getRandom.Next(-10, 20)}° C -  - reported by from Mr. Controller");
        return Ok("Test return ok");
    }
}