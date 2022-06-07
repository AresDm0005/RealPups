using Microsoft.AspNetCore.Mvc;

namespace Pups.Frontend.Controllers;

public class MessengerController : Controller
{
    private readonly ILogger<MessengerController> _logger;

    public MessengerController(ILogger<MessengerController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }
}