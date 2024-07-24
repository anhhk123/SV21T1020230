using Microsoft.AspNetCore.Mvc;

namespace SV21T1020230.Web.Controllers
{
    public class EmployeeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
