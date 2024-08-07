﻿using Microsoft.AspNetCore.Mvc;
using SV21T1020230.BusinessLayers;
using SV21T1020230.DomainModels;
using SV21T1020230.Web.Models;

namespace SV21T1020230.Web.Controllers
{
    public class CustomerController : Controller
    {
        const int PAGE_SZE = 20;
        public IActionResult Index(int page = 1 , string searchValue = "")
        {
            int rowCount = 0;
            var data = CommonDataService.ListOfCustomers(out rowCount, page, PAGE_SZE, searchValue);


            CustomerSearchResult customerSearchResult = new CustomerSearchResult
            {
                Page = page,
                RowCount = rowCount,
                SearchValue = searchValue,
                PageSize = PAGE_SZE,
                data = data
            };


            
            return View(customerSearchResult);
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
            ViewBag.Title = data.CustomerId == 0 ? "Bổ sung khách hàng" : "Cập nhật thông tin khách hàng";
            if (string.IsNullOrEmpty(data.CustomerName))
                ModelState.AddModelError(nameof(data.CustomerName), "Tên khách hàng không được để trống");
            if (string.IsNullOrEmpty(data.ContactName))
                ModelState.AddModelError(nameof(data.ContactName), "Tên giao dịch không được để trống");
            if (string.IsNullOrEmpty(data.Province))
                ModelState.AddModelError(nameof(data.Province), "Vui long chọn tỉnh thành");

            data.Phone = data.Phone ?? "";
            data.Email = data.Email ?? "";
            data.Address = data.Address ?? "";

            if(!ModelState.IsValid)
            {
                return View("Edit",data);
            }

            if (data.CustomerId == 0)
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
