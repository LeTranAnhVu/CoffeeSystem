using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SignalRService.Hubs;

namespace SignalRService.Controllers;

[Route("/realtime/api/[controller]")]
[ApiController]
public class TestController : ControllerBase
{
    private readonly IHubContext<TestHub, ITestHub> _testHubContext;

    public TestController(IHubContext<TestHub, ITestHub> hubContext)
    {
        _testHubContext = hubContext;
    }

    // GET
    public async Task<IActionResult> Test()
    {
        var getRandom = new Random();
        await _testHubContext.Clients.Group("private-group").ReceiveMessage("Brian",
            $" The temperature will be {getRandom.Next(-10, 20)}");
        // await _testHubContext.Clients.All.ReceiveMessage("Brian",
        //     $" The temperature will be {getRandom.Next(-10, 20)}");

        return Ok("Test return ok");
    }
}