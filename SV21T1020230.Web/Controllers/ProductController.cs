using Microsoft.AspNetCore.Mvc;
using SV21T1020230.BusinessLayers;
using SV21T1020230.DomainModels;
using static System.Runtime.InteropServices.JavaScript.JSType;
using SV21T1020230.Web.Models;
using System.Buffers;
using Microsoft.CodeAnalysis;

namespace SV21T1020230.Web.Controllers
{
    public class ProductController : Controller
    {
        const int PAGE_SZE = 20;
        public IActionResult Index(int page = 1, string searchValue = "", int CategoryID = 0, int SupplierID = 0, decimal MinPrice = 0, decimal MaxPrice = 0)
        {
            int rowCount = 0;
            List<Product> products =   ProductDataService.ListProducts(out rowCount, page,PAGE_SZE,searchValue,CategoryID, supplierId : SupplierID, minPrice : MinPrice, maxPrice: MaxPrice);
            ProductSearchResult productSearchResult = new ProductSearchResult
            {
                Page = page,
                RowCount = rowCount,
                SearchValue = searchValue,
                PageSize = PAGE_SZE,
                data = products
            };
            return View(productSearchResult);
        }
        public IActionResult Create()
        {
            ViewBag.Title = "Thêm mới sản phẩm";
            Product product = new Product()
            {
                ProductId = 0
            };
            return View("Edit", product);
        }
        
        public IActionResult Delete(int id = 0)
        {
            if (Request.Method == "POST")
            {
                ProductDataService.DeleteProduct(id);
                return RedirectToAction("Index");
            }

            Product product = ProductDataService.GetProductById(id);
            if(product == null)
            {
                
                return RedirectToAction("Index");
            }
            ViewBag.AllowDelete = ProductDataService.InUsed(id);
            return View(product);



         }

        
        public IActionResult Edit(int id = 0)
        {

            ViewBag.Title = "Cập nhật mặt hàng";
            Product product = ProductDataService.GetProductById(id);
            if(product == null)
            {
                return RedirectToAction("Index");
            }
            return View(product);
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
                    ViewBag.Title = "Xóa ảnh mặt hàng";
                    return RedirectToAction("Edit", new {id = id});
                default:
                    return RedirectToAction("Index");
            }
            
            
        }
        public IActionResult Attribute(int id = 0, string method = "", int attributeId = 0, ProductAttribute productAttribute = null )
        {

            switch (method)
            {
                case "add":
                    ViewBag.Title = "Bổ sung thuộc tính cho mặt hàng";
                    ViewBag.Method = "add";
                    if (Request.Method == "POST" && productAttribute != null)
                    {
                        productAttribute.Productid = id;
                        ProductDataService.AddAttribute(productAttribute);
                        return RedirectToAction("Edit", new { id = id });
                    }
                    ProductAttribute pa = new ProductAttribute()
                    {
                        Productid = id,
                        AttributeId = 0
                    };
                    
                    return View(pa);
                case "edit":
                    ViewBag.Title = "Thay đổi thuộc tính của  mặt hàng";
                    ViewBag.Method = "edit";
                    if (Request.Method == "POST")
                    {
                        ProductDataService.UpdateAttribute(productAttribute);
                        return RedirectToAction("Edit", new {id=productAttribute.Productid});
                    }
                    ProductAttribute result = null;
                    if (attributeId != 0)
                    {
                      result  = ProductDataService.GetAttributeById(attributeId);
                        result.Productid = id;
                        return View(result);
                    }
                    return RedirectToAction("Edit", new {id = id});
                    
                case "delete":
                    if(attributeId != 0)
                    {
                        ProductDataService.DeleteAttribute(attributeId);
                    }
                    return RedirectToAction("Edit", new { id = id });
                default:
                    return RedirectToAction("Index");
            }


        }


        [HttpPost]
        public IActionResult Save(Product data, IFormFile uploadPhoto)
        {
            ViewBag.Title = data.ProductId == 0 ? "Bổ sung sản phẩm" : "Cập nhật thông tin sản phẩm";
            if (string.IsNullOrEmpty(data.ProductName))
                ModelState.AddModelError(nameof(data.ProductName), "Tên sản phẩm không được để trống");
            if (string.IsNullOrEmpty(data.Unit))
                ModelState.AddModelError(nameof(data.Unit), "Đơn vị tính không được để trống");
            if (data.Price <= 0)
            {
                ModelState.AddModelError(nameof(data.Price), "Giá tiền không được để trống hoặc không hợp lệ");
            }
            if (uploadPhoto == null || string.IsNullOrEmpty(data.Photo))
            {
                ModelState.AddModelError(nameof(data.Photo), "Hãy chọn ảnh cho mặt hàng");
            }
            data.IsSelling = data.IsSelling;
            data.ProductDescription = data.ProductDescription ?? "";
            data.SupplierID = data.SupplierID;
            data.CategoryID = data.CategoryID;


            if (!ModelState.IsValid)
            {
                return View("Edit", data);
            }
            if (uploadPhoto != null && uploadPhoto.Length > 0)
            {
                var fileName = Path.GetFileName(uploadPhoto.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Product", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    uploadPhoto.CopyTo(stream);
                }
                data.Photo = fileName;
            }

            //TODO:Ktra dữ liệu đầu vào có hợp lệ hay không
            if (data.ProductId == 0)
            {
                ProductDataService.AddProduct(data);
                return RedirectToAction("Index");
            }
            else
            {
                ProductDataService.UpdateProduct(data);

            }
            return RedirectToAction("Index");
        }
    }
}
