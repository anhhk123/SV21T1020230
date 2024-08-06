using Microsoft.AspNetCore.Mvc;
using SV21T1020230.BusinessLayers;
using SV21T1020230.DomainModels;

namespace SV21T1020230.Web.Controllers
{
    public class CustomerController : Controller
    {
        const int PAGE_SZE = 20;
        public IActionResult Index(int page = 1 , string searchValue = "")
        {
            int rowCount = 0;
            var data = CommonDataService.ListOfCustomers(out rowCount, page, PAGE_SZE, searchValue ?? "");
            int pageCount = 1;
            pageCount = rowCount/PAGE_SZE;
            if (rowCount % PAGE_SZE > 0)
                pageCount += 1;

            
            ViewBag.RowCount = rowCount;
            ViewBag.PageCount = pageCount;
            ViewBag.SearchValue = searchValue;
            return View(data);
        }
        public IActionResult Create()
        {
            
            ViewBag.Title = "Bổ sung khách hàng";
            Customer customer = new Customer()
            {
                CustomerId = 0
            };
            return View("Edit", customer);
        }
        public IActionResult Edit(int id =0)
        {
            
            ViewBag.Title = "Cập nhật thông tin khách hàng";
            Customer customer= CommonDataService.GetCustomer(id);
            if (customer == null)
            {
                return RedirectToAction("Index");
            }
            return View(customer);
        }
        public IActionResult Delete(int id = 0)
        {
            if (Request.Method == "POST")
            { 
                CommonDataService.DeleteCustomer(id);
                return RedirectToAction("Index");
            }
            var customer = CommonDataService.GetCustomer(id);
            if (customer == null)
            {
                RedirectToAction("Index");
            }
            ViewBag.Allow.Delete = !CommonDataService.InUsed(id);
            return View(customer);
        }
        [HttpPost]
        public IActionResult Save(Customer data)
        {
            if(data.CustomerId == 0)
            {
                CommonDataService.AddCustomer(data);
            }
            else
            {
                CommonDataService.UpdateCustomer(data);
            }
            return RedirectToAction("Index");
        }
    }
}
