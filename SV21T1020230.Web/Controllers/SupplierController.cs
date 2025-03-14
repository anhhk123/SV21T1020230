﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV21T1020230.BusinessLayers;
using SV21T1020230.DomainModels;
using SV21T1020230.Web.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SV21T1020230.Web.Controllers
{
    [Authorize(Roles = $"{WebUserRoles.Administrator},{WebUserRoles.Employee}")]
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

            SupplierSearchResult result = new SupplierSearchResult()
            {
                data = data,
                RowCount = rowCount,
                Page = page,
                PageSize = PAGE_SZE,
                SearchValue = searchValue,
                
            };
            ViewBag.RowCount = rowCount;
            ViewBag.PageCount = pageCount;
            ViewBag.SearchValue = searchValue;
            return View(result);
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
        public IActionResult Save(Supplier data, IFormFile? uploadPhoto)
        {
            ViewBag.Title = data.SupplierID == 0 ? "Bổ sung nhà cung cấp" : "Cập nhật nhà cung cấp";
            if (string.IsNullOrEmpty(data.SupplierName))
                ModelState.AddModelError(nameof(data.SupplierName), "Tên nhà cung cấp không được để trống");
            if (string.IsNullOrEmpty(data.Email))
                ModelState.AddModelError(nameof(data.Email), "Email không được để trống");
            if (string.IsNullOrEmpty(data.Address))
                ModelState.AddModelError(nameof(data.Address), "Địa chỉ không được để trống");
            if (string.IsNullOrEmpty(data.Phone))
                ModelState.AddModelError(nameof(data.Phone), "Số điện thoại không được để trống");
            if (string.IsNullOrEmpty(data.Province))
                ModelState.AddModelError(nameof(data.Province), "Tỉnh thành không được để trống");
            
            
            if (uploadPhoto != null)
            {
                string fileName = $"{DateTime.Now.Ticks}_{uploadPhoto.FileName}"; //Tên file sẽ lưu
                string folder = Path.Combine(ApplicationContext.WebRootPath, @"Images\Supplier"); //đường dẫn đến thư mục lưu file
                                                                                                   // Kiểm tra và tạo thư mục nếu chưa tồn tại
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                string filePath = Path.Combine(folder, fileName); //Đường dẫn đến file cần lưu D:\images\supplier\photo.png

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    uploadPhoto.CopyTo(stream);
                }
                data.Photo = fileName;
            }
            if (!ModelState.IsValid)
            {
                return View("Edit", data);
            }
            if (data.SupplierID == 0)
            {
                CommonDataService.AddSupplier(data);
                
            }
            else
            {
                CommonDataService.UpdateSupplier(data);
            }
            return RedirectToAction("Index");
        }
    }
}
