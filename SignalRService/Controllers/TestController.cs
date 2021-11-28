using Microsoft.AspNetCore.Mvc;

namespace SignalRService.Controllers;

public class TestController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}