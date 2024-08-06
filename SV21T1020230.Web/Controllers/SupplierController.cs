using Microsoft.AspNetCore.Mvc;
using SV21T1020230.BusinessLayers;
using SV21T1020230.DomainModels;

namespace SV21T1020230.Web.Controllers
{
    public class SupplierController : Controller
    {
        const int PAGE_SZE = 20;
        public IActionResult Index(int page = 1, string searchValue = "")
        {
            int rowCount = 0;
            var data = CommonDataService.ListOfSuppliers(out rowCount, page, PAGE_SZE, searchValue ?? "");
            int pageCount = 1;
            pageCount = rowCount / PAGE_SZE;
            if (rowCount % PAGE_SZE > 0)
                pageCount += 1;


            ViewBag.RowCount = rowCount;
            ViewBag.PageCount = pageCount;
            ViewBag.SearchValue = searchValue;
            return View(data);
        }
        public IActionResult Create()
        {
            ViewBag.Title = "Thêm nhà cung cấp";
            Supplier supplier = new Supplier()
            {
                SupplierID = 0
            };
            return View("Edit", supplier);
        }
        public IActionResult Delete(int id = 0)
        {
            Supplier? supplier = CommonDataService.GetSupplier(id);
            if (supplier == null)
                return RedirectToAction("Index");

            if (Request.Method == "POST")
            {
                CommonDataService.DeleteSupplier(id);
                return RedirectToAction("Index");
            }

            ViewBag.AllowDelete = !CommonDataService.IsUsedSupplier(id);

            return View(supplier);
        }
        public IActionResult Edit(int id = 0)
        {
            ViewBag.Title = "Chỉnh sửa nhà cung cấp";
            Supplier? supplier = CommonDataService.GetSupplier(id);
            if (supplier == null)
            {
                return RedirectToAction("Index");
            }
            return View(supplier);
        }
        [HttpPost]
        public IActionResult Save(Supplier supplier)
        {
            if (supplier.SupplierID == 0)
            {
                CommonDataService.AddSupplier(supplier);
            }
            else
            {
                CommonDataService.UpdateSupplier(supplier);
            }
            return RedirectToAction("Index");
        }
    }
}
