using customer_info.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Drawing;


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

        [HttpGet]
        public IActionResult GetUnit(int personID)
        {
            var units = _context.Customers_Unit_temp
                                .FromSqlRaw(
                                    "EXEC GetUnit @PersonID",
                                    new SqlParameter("@PersonID", personID)
                                )
                                .ToList();

            return Json(units);
        }

        SqlParameter personIdParam = new SqlParameter
        {
            ParameterName = "@PersonID",
            SqlDbType = System.Data.SqlDbType.Int,
            Direction = System.Data.ParameterDirection.Output
        };


        [HttpPost]
        public IActionResult PostCustomer(Customers_temp customer, int size, List<string> unitNo, List<int> area, List<int> unitCost)
        {
            _context.Database.ExecuteSqlRaw(
                "EXEC PostCustomer @DateCol, @NameCol, @AddressCol, @Mobile, @Email, @Gender, @Occupation, @Cost, @ImagePath, @PersonID OUTPUT",
                new SqlParameter("@DateCol", customer.DateCol),
                new SqlParameter("@NameCol", customer.NameCol),
                new SqlParameter("@AddressCol", customer.AddressCol ?? (object)DBNull.Value),
                new SqlParameter("@Mobile", customer.Mobile),
                new SqlParameter("@Email", customer.Email ?? (object)DBNull.Value),
                new SqlParameter("@Gender", customer.Gender ?? (object)DBNull.Value),
                new SqlParameter("@Occupation", customer.Occupation ?? (object)DBNull.Value),
                new SqlParameter("@Cost", customer.Cost ?? (object)DBNull.Value),
                new SqlParameter("@ImagePath", customer.ImagePath ?? (object)DBNull.Value),

                personIdParam
            );

            int personId = (int)personIdParam.Value;

            for (int i = 0; i < size; i++)
            {
                _context.Database.ExecuteSqlRaw(
                    "EXEC PostUnit @UnitNo, @Area, @UnitCost, @PersonID",

                    new SqlParameter("@UnitNo", unitNo[i] ?? (object)DBNull.Value),
                    new SqlParameter("@Area", area[i]),
                    new SqlParameter("@UnitCost", unitCost[i]),
                    new SqlParameter("@PersonID", personId)
                );
            }

            return Json(new { success = true });

        }

        [HttpPost]
        public IActionResult UpdateCustomer(Customers_temp customer, int size, List<string> unitNo, List<int> area, List<int> unitCost)
        {
            _context.Database.ExecuteSqlRaw(
                "EXEC UpdateCustomer @PersonID, @DateCol, @NameCol, @AddressCol, @Mobile, @Email, @Gender, @Occupation, @Cost, @ImagePath",
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

            for (int i = 0; i < size; i++)
            {
                _context.Database.ExecuteSqlRaw(
                    "EXEC PostUnit @UnitNo, @Area, @UnitCost, @PersonID",

                    new SqlParameter("@UnitNo", unitNo[i] ?? (object)DBNull.Value),
                    new SqlParameter("@Area", area[i]),
                    new SqlParameter("@UnitCost", unitCost[i]),
                    new SqlParameter("@PersonID", customer.PersonID)
                );
            }

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
