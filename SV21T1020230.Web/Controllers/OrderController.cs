﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV21T1020230.BusinessLayers;
using SV21T1020230.DomainModels;
using SV21T1020230.Web.Models;

namespace SV21T1020230.Web.Controllers
{
    [Authorize(Roles = $"{WebUserRoles.Administrator},{WebUserRoles.Employee}")]
    public class OrderController : Controller
    {
        private const int ORDER_PAGE_SIZE = 20;
        private const string ORDER_SEARCH = "order_search"; //Tên biến dùng để lưu trong session

        private const int PRODUCT_PAGE_SIZE = 5;
        private const string PRODUCT_SEARCH = "product_search_for_sale";
        private const string SHOPPING_CART = "shopping_cart";
        public IActionResult Index()
        {
            OrderSearchInput? input = ApplicationContext.GetSessionData<OrderSearchInput>(ORDER_SEARCH);
            if (input == null)
            {
                input = new OrderSearchInput()
                {
                    Page = 1,
                    PageSize = ORDER_PAGE_SIZE,
                    SearchValue = "",
                    Status = 0,
                    DateRange = string.Format("{0:dd/MM/yyyy}-{1:dd/MM/yyyy}",
                                                                DateTime.Today.AddMonths(-1),
                                                                DateTime.Today),
                };
            }
            return View(input);
        }
        public IActionResult Search(OrderSearchInput input)
        {
            int rowCount = 0;

            var data = OrderDataService.ListOrders(out rowCount, input.Page, input.PageSize,
                input.Status, input.FromTime, input.ToTime, input.SearchValue ?? "");

            var model = new OrderSearchResult()
            {
                Page = input.Page,
                PageSize = input.PageSize,
                SearchValue = input.SearchValue ?? "",
                Status = input.Status,
                TimeRange = input.DateRange ?? "",
                RowCount = rowCount,
                data = data
            };

            ApplicationContext.SetSessionData(ORDER_SEARCH, input);

            return View(model);
        }
        public IActionResult Details(int id =0)
        {
            ViewBag.IsFinish = false;
            ViewBag.IsDelete = false;
            ViewBag.IsEmployee = false;
            ViewBag.IsEditDetails = false;

            var userData = User.GetUserData();

            var order = OrderDataService.GetOrder(id);
            if (order == null)
            {
                return RedirectToAction("Index");
            }

            if (userData.UserId.Equals(order.EmployeeID.ToString()))
            {
                ViewBag.IsEmployee = true;
            }

            switch (order.Status)
            {
                case Constants.ORDER_INIT:
                    ViewBag.IsDelete = true;
                    ViewBag.IsEditDetails = true;
                    break;
                case Constants.ORDER_ACCEPTED:
                    ViewBag.IsEditDetails = true;
                    break;
                case Constants.ORDER_FINISHED:
                    ViewBag.IsFinish = true;
                    break;
                case Constants.ORDER_CANCEL:
                case Constants.ORDER_REJECTED:
                    ViewBag.IsFinish = true;
                    ViewBag.IsDelete = true;
                    break;

            }

            var details = OrderDataService.ListOrderDetails(id);
            var model = new OrderDetailModel()
            {
                Order = order,
                Details = details
            };

            if (TempData["Message"] != null) ViewBag.Message = TempData["Message"];
            return View(model);
        }
        /// <summary>
        /// Chuyển đơn hàng sang trạng thái được duyệt
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Accept(int id = 0)
        {
            bool result = OrderDataService.AcceptOrder(id);
            if (!result)

                TempData["Message"] = "Không thể duyệt đơn hàng này ";
            return RedirectToAction("Details", new { id });

        }
        /// <summary>
        /// Chuyển đơn hàng sang trạng thái đã kết thúc
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Finish(int id = 0)
        {
            bool result = OrderDataService.FinishOrder(id);
            if (!result)

                TempData["Message"] = "Không thể ghi nhận trạng thái đơn hàng kết thúc cho đơn hàng này ";
            return RedirectToAction("Details", new { id });

        }
        /// <summary>
        /// Chuyển đơn hàng sang trạng thái đã bị huỷ
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Cancel(int id = 0)
        {
            bool result = OrderDataService.CancelOrder(id);
            if (!result)

                TempData["Message"] = "Không thể thực hiện thao thác huỷ đối với đơn hàng này";
            return RedirectToAction("Details", new { id });

        }
        /// <summary>
        /// Chuyển trang thái đơn hàng sang bị Từ chối 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Reject(int id = 0)
        {
            bool result = OrderDataService.RejectOrder(id);
            if (!result)

                TempData["Message"] = "Không thể thực hiện thao thác từ chối đối với đơn hàng này ";
            return RedirectToAction("Details", new { id });

        }
        /// <summary>
        /// Xoá đơn hàng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Delete(int id)
        {
            bool result = OrderDataService.DeleteOrder(id);
            if (!result)
            {
                TempData["Message"] = "Không thể xoá đơn hàng này ";
                return RedirectToAction("Details", new { id });

            }
            return RedirectToAction("Index");

        }
        
        /// <summary>
        /// Giao diện để chọn người giao hàng cho đơn hàng
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Shipping(int id = 0)
        {
            ViewBag.OrderID = id;
            return View();
        }
        /// <summary>
        /// Ghi nhận giao hàng cho đơn hàng và chuyển đơn giao hàng sang trạng thái đang giao hàng
        /// Hàm trả về chuỗi khác rỗng thông báo lỗi nếu đầu vào không hợp lệ hoặc lỗi ,
        /// Hàm trả về chuỗi rỗng nếu thành công
        /// </summary>
        /// <param name="id"></param>
        /// <param name="shipperId"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Shipping(int id = 0, int shipperId = 0)
        {
            if (shipperId <= 0)
                return Json(new { success = false, message = "Vui lòng chọn người giao hàng" });

            bool result = OrderDataService.ShipOrder(id, shipperId);

            if (!result)
                return Json(new { success = false, message = "Đơn hàng không cho phép chuyển cho người giao hàng" });

            return Json(new { success = true, redirectUrl = Url.Action("Details", new { id }) });
        }
        /// <summary>
        /// Giao diện trang lập đơn hàng mới 
        /// </summary>
        /// <returns></returns>
        public IActionResult Create()
        {
            var input = ApplicationContext.GetSessionData<ProductSearchInput>(PRODUCT_SEARCH);
            if (input == null)
            {
                input = new ProductSearchInput()
                {
                    Page = 1,
                    PageSize = PRODUCT_PAGE_SIZE,
                    SearchValue = ""
                };
            }

            return View(input);
        }
        /// <summary>
        /// TÌm kiếm mặt hang để đưa vào giỏ hàng
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public IActionResult SearchProduct(ProductSearchInput input)
        {
            int rowCount = 0;
            var data = ProductDataService.ListProducts(out rowCount, input.Page, input.PageSize, input.SearchValue ?? "");
            var model = new ProductSearchResult()
            {
                Page = input.Page,
                PageSize = input.PageSize,
                SearchValue = input.SearchValue ?? "",
                RowCount = rowCount,
                data = data
            };
            ApplicationContext.SetSessionData(PRODUCT_SEARCH, input);
            return View(model);
        }
        /// <summary>
        /// Lấy giỏi hàng hiện tại trong session 
        /// </summary>
        /// <returns></returns>
        private List<OrderDetail> GetShoppingCart()
        {
            var shoppingCart = ApplicationContext.GetSessionData<List<OrderDetail>>(SHOPPING_CART);
            if (shoppingCart == null)
            {
                shoppingCart = new List<OrderDetail>();
                ApplicationContext.SetSessionData(SHOPPING_CART, shoppingCart);
            }
            return shoppingCart;
        }
        /// <summary>
        /// Trang hiển thị danh sách các mặt hàng có trong giỏ hàng
        /// </summary>
        /// <returns></returns>
        public IActionResult ShowShoppingCart()
        {
            var model = GetShoppingCart();
            return View(model);
        }
        /// <summary>
        /// Bổ sung mặt hàng vào giỏ hàng
        /// Hàm trả về chuỗi rỗng thông báo nếu lỗi dữ liệu không hợp lệ ,
        /// hàm trả về chuỗi rỗng nếu thành công
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public IActionResult AddToCart(OrderDetail data)
        {
            if (data.SalePrice <= 0 || data.Quantity <= 0)
                return Json("Giá bán và số lượng không hợp lệ");
            var shoppingCart = GetShoppingCart();
            var existsProduct = shoppingCart.FirstOrDefault(m => m.ProductID == data.ProductID);
            if (existsProduct == null)
            {
                shoppingCart.Add(data);
            }
            else
            {
                existsProduct.Quantity += data.Quantity;
                existsProduct.SalePrice += data.SalePrice;

            }
            ApplicationContext.SetSessionData(SHOPPING_CART, shoppingCart);
            return Json("");
        }
        /// <summary>
        /// Xoá mặt hàng ra khỏi giỏ hàng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult RemoveFromCart(int id = 0)
        {
            var shoppingCart = GetShoppingCart();
            int index = shoppingCart.FindIndex(m => m.ProductID == id);
            if (index >= 0)
                shoppingCart.RemoveAt(index);
            ApplicationContext.SetSessionData(SHOPPING_CART, shoppingCart);
            return Json("");
        }
        /// <summary>
        /// Xoá tất cả mặt hàng trong giỏ hàng
        /// </summary>
        /// <returns></returns>
        public IActionResult ClearCart()
        {
            var shoppingCart = GetShoppingCart();
            shoppingCart.Clear();
            ApplicationContext.SetSessionData(SHOPPING_CART, shoppingCart);
            return Json("");
        }
        /// <summary>
        /// Giao diện để sửa đổi địa chỉ giao hàng
        /// </summary>
        /// <param name="id">Mã đơn hàng</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult EditAddress(int id = 0)
        {
            var model = OrderDataService.GetOrder(id);
            return View(model);
        }

        /// <summary>
        /// Cập nhật địa chỉ và tỉnh thành giao trong đơn hàng.
        /// Hàm trả về chuỗi khác rỗng thông báo lỗi nếu đầu vào không hợp lệ hoặc lỗi,
        /// hàm trả về chuỗi rỗng nếu thành công
        /// </summary>
        /// <param name="orderID"></param>  
        /// <param name="deliveryAddress"></param>
        /// <param name="deliveryProvince"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult UpdateAddress(int orderID, string deliveryAddress, string deliveryProvince)
        {
            if (string.IsNullOrWhiteSpace(deliveryAddress))
                return Json(new { success = false, message = "Vui lòng nhập địa chỉ" });

            if (string.IsNullOrEmpty(deliveryProvince))
                return Json(new { success = false, message = "Vui lòng chọn tỉnh thành" });

            bool result = OrderDataService.SaveOrderAddress(orderID, deliveryAddress, deliveryProvince);

            if (!result)
                return Json(new { success = false, message = "Không được phép thay đổi thông tin của đơn hàng này" });

            return Json(new { success = true, redirectUrl = Url.Action("Details") });
        }
        /// <summary>
        /// Xoá mặt hàng ra khỏi đơn hàng 
        /// </summary>
        /// <param name="id">Mã đơn hàng</param>
        /// <param name="productId">Mã mặt hàng cần tìm</param>
        /// <returns></returns>
        public IActionResult DeleteDetail(int id = 0, int productId = 0)
        {
            bool result = OrderDataService.DeleteOrderDetail(id, productId);
            if (!result)
                TempData["Message"] = "Không thể xoá mặt hàng ra khỏi đơn hàng";
            return RedirectToAction("Details", new { id });
        }
        /// <summary>
        /// Giao diện để sửa đổi thông tin mặt hàng được bán trong đơn hàng
        /// </summary>
        /// <param name="id">Mã đơn hàng</param>
        /// <param name="productId">Mã mặt hàng</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult EditDetail(int id = 0, int productId = 0)
        {
            var model = OrderDataService.GetOrderDetail(id, productId);
            return View(model);
        }
        /// <summary>
        /// Chỉnh sửa giá bán số lượng hàng trong hóa đơn 
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="productId"></param>
        /// <param name="quantity"></param>
        /// <param name="salePrice"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult UpdateDetail(int orderId, int productId, int quantity, decimal salePrice)
        {
            if (quantity <= 0)
            {
                return Json("Số lượng bán không hợp lệ");
            }
            if (salePrice < 0)
                return Json("Giá bán không hợp lệ");
            bool result = OrderDataService.SaveOrderDetail(orderId, productId, quantity, salePrice);
            if (!result)
                return Json("Không được phép thay đổi thông tin của đơn hàng này ");
            return Json("");
        }
        /// <summary>
         /// Khởi tạo đơn hàng (lập một đơn hàng mới )
         /// Hàm trả về chuỗi khác rỗng thông báo lỗi nếu đầu vào không hợp lệ 
         /// hoặc việc tạo đơn hàng không thành công
         /// Ngược lại hàm trả về mã của đơn hàng được tạo (là một giá trị số)
         /// </summary>
         /// <param name="customerID">Mã đơn hàng</param>
         /// <param name="deliveryProvince">Tỉnh/thành giao hàng</param>
         /// <param name="deliveryAddress">Địa chỉ giao hàng</param>
         /// <returns></returns>
        public IActionResult Init(int customerID = 0, string deliveryProvince = "", string deliveryAddress = "")
        {
            var shoppingCart = GetShoppingCart();
            if (shoppingCart.Count == 0)
                return Json("Giỏ hàng trống, không thể lập đơn hàng");

            var lockedCustomer = CommonDataService.GetCustomer(customerID)?.IsLocked;
            if (lockedCustomer == true)
                return Json("Khách hàng này đang bị khóa");

            if (customerID <= 0 || string.IsNullOrWhiteSpace(deliveryProvince)
                                || string.IsNullOrWhiteSpace(deliveryAddress))
                return Json("Vui lòng nhập đầy đủ thông tin");

            int employeeID = Convert.ToInt32(User.GetUserData()?.UserId);
            int orderID = OrderDataService.InitOrder(employeeID, customerID, deliveryProvince, deliveryAddress, shoppingCart);

            ClearCart();

            return Json(orderID);
        }

    }
}
