using customer_info.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace customer_info.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ILogger<CustomerController> _logger;

        private readonly AppDbContext _context;

        public CustomerController(AppDbContext context, ILogger<CustomerController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public List<Customers_temp> GetCustomers()
        {
            return _context.Customers_temp
                           .FromSqlRaw("EXEC GetCustomers")
                           .ToList();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
