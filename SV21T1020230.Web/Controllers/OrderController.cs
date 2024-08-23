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
            List<ProductInOrderDetail> listProduct = OrderDataService.ListProductInOrders(id);
            order.ListProductInOrder = listProduct;
            return View(order);
        }
        public IActionResult EditDetail(int id = 0, int idProductId = 0)
        {
            var model = null;
            return View(model);
        }
        public IActionResult Shipping()
        {
            return View();
        }
        public IActionResult Create()
        {
            return View();
        }
        public IActionResult Delete(int orderId = 0)
        {
            if(orderId != 0)
            {
                bool result = OrderDataService.DeleteOrderById(orderId);
                if (result)
                {
                    return RedirectToAction("Index");
                }
            }
            
            return View();
        }
        public IActionResult DeleteDetail(int id = 0, int productId = 0)
        {
            if(productId != 0 && id != 0)
            {
                OrderDataService.DeleteProductInOrders(id, productId);
                
            }
            return RedirectToAction("Details", new { id = id });
        }

    }
}
