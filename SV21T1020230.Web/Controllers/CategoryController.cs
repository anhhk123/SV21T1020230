using Microsoft.AspNetCore.Mvc;
using SV21T1020230.DataLayers.FakeData;
using SV21T1020230.DomainModels;

namespace SV21T1020230.Web.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            List<Category> model = CategoryFakeData.GetListCategory();
            return View(model);
        }
        
        
        public IActionResult Edit(int id = 0)
        {
            ViewBag.Title = "Sửa danh mục";

            return View();
        }
        public IActionResult Delete(int id = 0)
        {
            return View();
        }
        public IActionResult Create()
        {
            ViewBag.Title = "Thêm danh mục";

            return View("Edit");
        }
    }
}
