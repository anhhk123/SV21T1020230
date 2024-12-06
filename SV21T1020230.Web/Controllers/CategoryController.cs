using Microsoft.AspNetCore.Mvc;
using SV21T1020230.BusinessLayers;
using SV21T1020230.DomainModels;

namespace SV21T1020230.Web.Controllers
{
    public class CategoryController : Controller
    {
        const int PAGE_SIZE = 10;
        public IActionResult Index(int page = 1, string searchValue = "")
        {
            int rowCount = 0;
            var data = CommonDataService.ListOfCategories(out rowCount, page, PAGE_SIZE, searchValue);
            int pageCount = 1;

            pageCount = (rowCount + PAGE_SIZE - 1) / PAGE_SIZE;

            ViewBag.Page = page;
            ViewBag.PageCount = pageCount;
            ViewBag.RowCount = rowCount;
            ViewBag.SearchValue = searchValue;

            return View(data);
        }


        public IActionResult Create()
        {
            ViewBag.Title = "Tạo loại hàng";
            Category category = new Category()
            {
                CategoryId = 0
            };
            return View("Edit", category);
        }

        public IActionResult Edit(int id = 0)
        {
            ViewBag.Title = "Chỉnh sửa loại hàng";
            Category? category = CommonDataService.GetCategory(id);
            if (category == null)
            {
                return RedirectToAction("Index");
            }
            return View(category);
        }

        public IActionResult Delete(int id = 0)
        {

            Category? category = CommonDataService.GetCategory(id);
            if (category == null)
            {
                return RedirectToAction("Index");
            }

            if (Request.Method == "POST")
            {
                CommonDataService.DeleteCategory(id);
                return RedirectToAction("Index");
            }

            ViewBag.AllowDelete = true;
            if (CommonDataService.IsUsedCategory(id))
            {
                ViewBag.AllowDelete = false;
            }
            return View(category);
        }

        [HttpPost]
        public IActionResult Save(Category category)
        {
            if (category.CategoryId == 0)
            {
                CommonDataService.AddCategory(category);
            }
            else
            {
                CommonDataService.UpdateCategory(category);
            }
            return RedirectToAction("Index");
        }
    }
}
