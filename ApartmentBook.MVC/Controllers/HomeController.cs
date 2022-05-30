using ApartmentBook.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ApartmentBook.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public List<int> Chart()
        {
            return new List<int>()
            {
                20, 40, 50, 30, 60, 30, 45
            };
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
    public class LineChartData
    {
        public DateTime xValue;
        public double yValue;
        public double yValue1;
    }

    public class ChartModel
    {
        public List<ChartFields> ChartData { get; set; }
    }
    public class ChartFields
    {
        public string EmployeeName { get; set; }
        public int Salery { get; set; }
    }
}