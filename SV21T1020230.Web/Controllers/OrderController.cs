using Microsoft.AspNetCore.Mvc;
using SV21T1020230.BusinessLayers;
using SV21T1020230.DomainModels;
using SV21T1020230.Web.Models;

namespace SV21T1020230.Web.Controllers
{
    public class OrderController : Controller
    {
        const int PAGE_SIZE = 20;
        public IActionResult Index(int? page, string orderTime, string searchValue, int? PageSize)
        {
            ViewBag.PageSize = PAGE_SIZE;
            ViewBag.SearchValue = searchValue;
            ViewBag.OrderTime = orderTime;
            ViewBag.PageNumber = page == null ? 1 : Convert.ToInt32(page);
            return View();
        }

        public async Task<ActionResult> ListOrderPartial(OrderSearchInput input)
        {
            int rowCount = 0;
            int totalrows = 0;
            int TotalPage = 1;
            if (string.IsNullOrEmpty(input.SearchValue)) { input.SearchValue = ""; }

            var data = OrderDataService.ListOfOrders(out rowCount, input.Page, PAGE_SIZE, input.SearchValue, input.Status, input.GetDateTo(), input.GetDateFrom());
            var result = new OrderSearchResult()
            {
                data = data,
                RowCount = rowCount,
                Page = input.Page,
                PageSize = PAGE_SIZE,
                SearchValue = input.SearchValue,
                orderTime = input.orderTime
            };
            totalrows = result.RowCount;
            if (totalrows > 0)
            {
                TotalPage = totalrows % PAGE_SIZE != 0 ? totalrows / PAGE_SIZE + 1 : totalrows / PAGE_SIZE;
            }

            ViewBag.PageSize = PAGE_SIZE;
            ViewBag.TotalRecord = totalrows;
            ViewBag.TotalPage = TotalPage;

            return PartialView(result);
        }
        public IActionResult Details(int id)
        {
            OrderDetail order = OrderDataService.GetOrderById(id);
            return View(order);
        }
        public IActionResult EditDetail(int id = 0, int idProductId = 0)


        {
            return View();
        }
        public IActionResult Shipping()
        {
            return View();
        }
        public IActionResult Create()
        {
            return View();
        }

    }
}
