using Microsoft.AspNetCore.Mvc;

namespace SV21T1020230.Web.Controllers
{
    public class ShipperController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Create()
        {
            ViewBag.Title = "Thêm người giao hàng";

            return View("Edit");
        }
        public IActionResult Delete(int id = 0)
        {
            return View();
        }
        public IActionResult Edit(int id = 0)
        {
            ViewBag.Title = "Sửa thông tin người giao hàng";

            return View();
        }
        

    }
}
