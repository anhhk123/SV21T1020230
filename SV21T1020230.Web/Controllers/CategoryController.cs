using Microsoft.AspNetCore.Mvc;
using SV21T1020230.BusinessLayers;
using SV21T1020230.DomainModels;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        public IActionResult Save(Category data)
        {
            ViewBag.Title = data.CategoryId == 0 ? "Bổ sung loại hàng" : "Cập nhật thông tin loại hàng";
            if (string.IsNullOrEmpty(data.CategoryName))
                ModelState.AddModelError(nameof(data.CategoryName), "Tên loại hàng không được để trống");
            data.Description = data.Description ?? "";
            if (!ModelState.IsValid)
            {
                return View("Edit", data);
            }

            //TODO:Ktra dữ liệu đầu vào có hợp lệ hay không
            if (data.CategoryId == 0)
            {
                CommonDataService.AddCategory(data);
                return RedirectToAction("Index");
            }
            else
            {
                CommonDataService.UpdateCategory(data);

            }
            return RedirectToAction("Index");

            
        }
    }
}
