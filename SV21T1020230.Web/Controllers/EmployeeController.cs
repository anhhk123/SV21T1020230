using Microsoft.AspNetCore.Mvc;

namespace SV21T1020230.Web.Controllers
{
    public class EmployeeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Create()
        {
            ViewBag.Title = "Thêm nhân viên";
            return View("Edit");
        }
        public IActionResult Edit()
        {
            ViewBag.Title = "Chỉnh sửa thông tin nhân viên";

            return View();
        }
        public IActionResult Delete()
        {
            
            return View();
        }
    }
}
