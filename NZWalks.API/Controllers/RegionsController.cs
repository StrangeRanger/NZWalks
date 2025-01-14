using Microsoft.AspNetCore.Mvc;

namespace NZWalks.API.Controllers;

public class RegionsController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}