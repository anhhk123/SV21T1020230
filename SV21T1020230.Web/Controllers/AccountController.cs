using Microsoft.AspNetCore.Mvc;

namespace SV21T1020230.Web.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Logout()
        {
            return View();
        }
        public IActionResult ChangePassword()
        {
            return View();
        }
    }
}
