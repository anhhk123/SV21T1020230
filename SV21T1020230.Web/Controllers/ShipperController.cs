using Microsoft.AspNetCore.Mvc;
using SV21T1020230.BusinessLayers;
using SV21T1020230.DomainModels;
using System.Buffers;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SV21T1020230.Web.Controllers
{
    public class ShipperController : Controller
    {
        const int PAGE_SIZE = 10;
        public IActionResult Index(int page = 1, string searchValue = "")
        {
            var rowCount = 0;
            var data = CommonDataService.ListOfShippers(out rowCount, page, PAGE_SIZE, searchValue);
            int pageCount = (rowCount + PAGE_SIZE - 1) / PAGE_SIZE;

            ViewBag.Page = page;
            ViewBag.PageCount = pageCount;
            ViewBag.RowCount = rowCount;
            ViewBag.SearchValue = searchValue;

            return View(data);
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
