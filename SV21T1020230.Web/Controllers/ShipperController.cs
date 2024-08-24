using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV21T1020230.BusinessLayers;
using SV21T1020230.DomainModels;
using SV21T1020230.Web.Models;
using System.Buffers;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SV21T1020230.Web.Controllers
{
    [Authorize(Roles = $"{WebUserRoles.Administrator},{WebUserRoles.Employee}")]

    public class ShipperController : Controller
    {
        private const int PAGE_SIZE = 9;
        private const string SEARCH_CONDITION = "shippers_search"; //Tên biến dùng để lưu trong session


        public IActionResult Index()
        {
            PaginationSearchInput? input = ApplicationContext.GetSessionData<PaginationSearchInput>(SEARCH_CONDITION);
            if (input == null)
            {
                input = new PaginationSearchInput()
                {
                    Page = 1,
                    PageSize = PAGE_SIZE,
                    SearchValue = ""
                };
            }
            return View(input);
        }
        public IActionResult Search(PaginationSearchInput input)
        {
            int rowCount = 0;
            var data = CommonDataService.ListOfShippers(out rowCount, input.Page, input.PageSize, input.SearchValue ?? "");
            var model = new ShipperSearchResult()
            {
                Page = input.Page,
                PageSize = input.PageSize,
                SearchValue = input.SearchValue ?? "",
                RowCount = rowCount,
                data = data
            };
            ApplicationContext.SetSessionData(SEARCH_CONDITION, input);
            return View(model);
        }
        public IActionResult Create()
        {
            ViewBag.Title = "Tạo đơn vị giao hàng";
            Shipper shipper = new Shipper()
            {
                ShipperID = 0
            };
            return View("Edit", shipper);
        }
        public IActionResult Delete(int id = 0)
        {
            Shipper? shipper = CommonDataService.GetShipper(id);
            if (shipper == null)
            {
                return RedirectToAction("Index");
            }
            if (Request.Method == "POST")
            {
                CommonDataService.DeleteShipper(id);
                return RedirectToAction("Index");
            }
            ViewBag.AllowDelete = true;
            if (CommonDataService.IsUsedShipper(id))
            {
                ViewBag.AllowDelete = false;
            }
            return View(shipper);
        }
        public IActionResult Edit(int id = 0)
        {
            ViewBag.Title = "Chỉnh sửa đơn vị giao hàng";
            Shipper? shipper = CommonDataService.GetShipper(id);
            if (shipper == null)
            {
                return RedirectToAction("Index");
            }
            return View(shipper);
        }
        [HttpPost]
        public IActionResult Save(Shipper data)
        {
            ViewBag.Title = data.ShipperID == 0 ? "Bổ sung người giao hàng" : "Cập nhật thông tin giao hàng";
            if (string.IsNullOrEmpty(data.ShipperName))
                ModelState.AddModelError(nameof(data.ShipperName), "Tên giao hàng không được để trống");
            if (string.IsNullOrEmpty(data.Phone))
                ModelState.AddModelError(nameof(data.Phone), "Số điện thoại  không được để trống");

            if(!ModelState.IsValid)
            {
                return View("Edit",data);
            }
            if (data.ShipperID == 0)
            {
                CommonDataService.AddShipper(data);
            }
            else
            {
                CommonDataService.UpdateShipper(data);
            }
            return RedirectToAction("Index");
        }


    }
}
