using Microsoft.AspNetCore.Mvc;
using SV21T1020230.BusinessLayers;
using SV21T1020230.DomainModels;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SV21T1020230.Web.Controllers
{
    public class EmployeeController : Controller
    {
        const int PAGE_SIZE = 20;
        public IActionResult Index(int page = 1, int pageSize = 10, string searchValue = "")
        {
            int rowCount = 0;
            var data = CommonDataService.ListofEmployees(out rowCount, page, PAGE_SIZE, searchValue ?? "");
            int pageCount = 1;
            pageCount = rowCount / PAGE_SIZE;
            if (rowCount % PAGE_SIZE > 0)
                pageCount += 1;
            ViewBag.Page = page;
            ViewBag.RowCount = rowCount;
            ViewBag.PageCount = pageCount;
            ViewBag.SearchValue = searchValue;

            return View(data);
        }
            public IActionResult Create()
        {
            ViewBag.Title = "Thêm nhân viên";
            Employee employee = new Employee()
            {
                EmployeeID = 0,
                IsWorking = false,
                BirthDate = DateTime.Now,
                
            };
            return View("Edit", employee);
        }
        public IActionResult Edit(int id)
        {
            ViewBag.Title = "Chỉnh sửa nhân viên";
            Employee? employee = CommonDataService.GetEmployee(id);
            if (employee == null)
            {
                return RedirectToAction("Index");
            }
            return View(employee);
        }
        public IActionResult Delete(int id)
        {
            Employee? employee = CommonDataService.GetEmployee(id);
            if (employee == null)
                return RedirectToAction("Index");

            if (Request.Method == "POST")
            {
                CommonDataService.DeleteEmployee(id);
                return RedirectToAction("Index");
            }

            ViewBag.AllowDelete = !CommonDataService.IsUsedEmployee(id);

            return View(employee);
        }
        public IActionResult Save(Employee data)
        {
            ViewBag.Title = data.EmployeeID == 0 ? "Bổ sung nhân viên" : "Cập nhật thông tin nhân viên";
            if (string.IsNullOrEmpty(data.FullName))
                ModelState.AddModelError(nameof(data.FullName), "Tên nhân viên không được để trống");
            
            if (string.IsNullOrEmpty(data.Address))
                ModelState.AddModelError(nameof(data.Address), "Địa chỉ  không được để trống");
            if (string.IsNullOrEmpty(data.Phone))
                ModelState.AddModelError(nameof(data.Phone), "Số điện thoại viên không được để trống");

            if (!ModelState.IsValid)
            {
                return View("edit", data);
            }
            // TODO: Validate information of employee
            if (data.EmployeeID == 0)
            {
                CommonDataService.AddEmployee(data);
            }
            else
            {
                
                CommonDataService.UpdateEmployee(data);
            }
            return RedirectToAction("Index");
        }
    }
}
