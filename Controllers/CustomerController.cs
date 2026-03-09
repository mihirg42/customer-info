using customer_info.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;


namespace customer_info.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ILogger<CustomerController> _logger;

        private readonly IConfiguration _configuration;

        private readonly AppDbContext _context;

        public CustomerController(AppDbContext context, ILogger<CustomerController> logger, IConfiguration configuration)
        {
            _context = context;
            _logger = logger;
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult GetCustomers()
        {
            var customers = _context.Customers_temp
                                    .FromSqlRaw("EXEC GetCustomers")
                                    .ToList();

            return Json(customers);
        }

        [HttpPost]
        public IActionResult PostCustomer(Customers_temp customer)
        {
            _context.Database.ExecuteSqlRaw(
                "EXEC PostCustomer @DateCol, @NameCol, @AddressCol, @Mobile, @Email, @Gender, @Occupation, @Cost",
                new SqlParameter("@DateCol", customer.DateCol),
                new SqlParameter("@NameCol", customer.NameCol),
                new SqlParameter("@AddressCol", customer.AddressCol ?? (object)DBNull.Value),
                new SqlParameter("@Mobile", customer.Mobile),
                new SqlParameter("@Email", customer.Email ?? (object)DBNull.Value),
                new SqlParameter("@Gender", customer.Gender ?? (object)DBNull.Value),
                new SqlParameter("@Occupation", customer.Occupation ?? (object)DBNull.Value),
                new SqlParameter("@Cost", customer.Cost ?? (object)DBNull.Value),
                new SqlParameter("@ImagePath", customer.ImagePath ?? (object)DBNull.Value)
            );

            return Json(new { success = true });

        }

        [HttpPost]
        public IActionResult UpdateCustomer(Customers_temp customer)
        {
            _context.Database.ExecuteSqlRaw(
                "EXEC UpdateCustomer @PersonID, @DateCol, @NameCol, @AddressCol, @Mobile, @Email, @Gender, @Occupation, @Cost",
                new SqlParameter("@PersonID", customer.PersonID),
                new SqlParameter("@DateCol", customer.DateCol),
                new SqlParameter("@NameCol", customer.NameCol),
                new SqlParameter("@AddressCol", customer.AddressCol ?? (object)DBNull.Value),
                new SqlParameter("@Mobile", customer.Mobile),
                new SqlParameter("@Email", customer.Email ?? (object)DBNull.Value),
                new SqlParameter("@Gender", customer.Gender ?? (object)DBNull.Value),
                new SqlParameter("@Occupation", customer.Occupation ?? (object)DBNull.Value),
                new SqlParameter("@Cost", customer.Cost ?? (object)DBNull.Value),
                new SqlParameter("@ImagePath", customer.ImagePath ?? (object)DBNull.Value)
            );

            return Json(new { success = true });
        }
        public IActionResult TestConnection()
        {
            using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                conn.Open();
            }

            return Content("Database Connected Successfully");
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
