using Microsoft.AspNetCore.Mvc;
using Pups.Frontend.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Identity;

namespace Pups.Frontend.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        var signed = HttpContext.User.Identity!.IsAuthenticated;
        // if (signed) return Redirect("/Messenger");
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
