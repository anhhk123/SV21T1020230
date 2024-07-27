﻿using Microsoft.AspNetCore.Mvc;

namespace SV21T1020230.Web.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Create()
        {
            ViewBag.Title = "Thêm mặt hàng";
            return View("Edit");
        }
        public IActionResult Delete(int id = 0)
        {
            return View();
        }
        public IActionResult Edit(int id = 0)
        {

            ViewBag.Title = "Cập nhật mặt hàng";
            return View();
        }
        public IActionResult Photo(int id =0 , string method ="", int idPhoto = 0)
        {

            switch (method)
            {
                case "add":
                    ViewBag.Title = "Bổ sung ảnh cho mặt hàng";
                    return View();
                case "edit":
                    ViewBag.Title = "Thay đổi ảnh cho mặt hàng";
                    return View();
                case "delete":
                    ViewBag.Title = "Bổ sung ảnh cho mặt hàng";
                    return RedirectToAction("Edit", new {id = id});
                default:
                    return RedirectToAction("Index");
            }
            
            
        }
        public IActionResult Attribute(int id = 0, string method = "", int idPhoto = 0)
        {

            switch (method)
            {
                case "add":
                    ViewBag.Title = "Bổ sung thuộc tính cho mặt hàng";
                    return View();
                case "edit":
                    ViewBag.Title = "Thay đổi thuộc tính của  mặt hàng";
                    return View();
                case "delete":
                    return RedirectToAction("Edit", new { id = id });
                default:
                    return RedirectToAction("Index");
            }


        }
    }
}
