using Microsoft.AspNetCore.Mvc;

namespace ApartmentBook.MVC.Features.Apartments.Controllers
{
    public class ApartmentController : Controller
    {
        private readonly ILogger<ApartmentController> _logger;

        public ApartmentController(ILogger<ApartmentController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewData["Name"] = "Pale moje jointy";
            return View();
        }
    }
}
