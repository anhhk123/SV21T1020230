using Microsoft.AspNetCore.Mvc;
using SV21T1020230.BusinessLayers;
using SV21T1020230.DomainModels;
using static System.Runtime.InteropServices.JavaScript.JSType;
using SV21T1020230.Web.Models;
using System.Buffers;
using Microsoft.CodeAnalysis;
using Microsoft.AspNetCore.Authorization;

namespace SV21T1020230.Web.Controllers
{
    [Authorize(Roles = $"{WebUserRoles.Administrator},{WebUserRoles.Employee}")]
    public class ProductController : Controller
    {
        const int PAGE_SIZE = 20;
        public IActionResult Index(int page = 1, int categoryID = 0, int supplierID = 0, int minPrice = 0, int maxPrice = 0, string searchValue = "")
        {
            
            int rowCount = 0, tempRowCount = 0;
            List<Product> products = ProductDataService.ListProducts(out rowCount, page, PAGE_SIZE, searchValue, categoryID, supplierID, minPrice, maxPrice);
            var model = new ProductSearchResult
            {
                data = products,
                Page = page,
                CategoryID = categoryID,
                SupplierID = supplierID,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                PageSize = PAGE_SIZE,
                RowCount = rowCount,
                SearchValue = searchValue,
                Categories = CommonDataService.ListOfCategories(out tempRowCount, 1, 0, ""),
                Suppliers = CommonDataService.ListOfSuppliers(out tempRowCount, 1, 0, "")
            };
            return View(model);
        }

        public IActionResult Create()
        {
            ViewBag.Title = "Thêm sản phẩm";
            int temp = 0;
            var lstCategory = CommonDataService.ListOfCategories(out temp, 1, 0, "");
            var lstSupplier = CommonDataService.ListOfSuppliers(out temp, 1, 0, "");

            var productDetail = new ProductDetail()
            {
                Categories = lstCategory ?? new List<Category>(),
                Suppliers = lstSupplier ?? new List<Supplier>(),
                Product = new Product()
            };
            return View("Edit", productDetail);
        }

        public IActionResult Edit(int id = 0)
        {
            ViewBag.Title = "Chỉnh sửa sản phẩm";

            var product = ProductDataService.GetProductById(id);
            if (product == null)
            {
                return View("Index");
            }

            int temp = 0;
            var lstPhoto = ProductDataService.ListOfProductPhotos(id);
            var lstAttribute = ProductDataService.ListOfProductAttributes(id);
            var lstCategory = CommonDataService.ListOfCategories(out temp, 1, 0, "");
            var lstSupplier = CommonDataService.ListOfSuppliers(out temp, 1, 0, "");

            var productDetail = new ProductDetail()
            {
                Photos = lstPhoto ?? new List<ProductPhoto>(),
                Attributes = lstAttribute ?? new List<ProductAttribute>(),
                Categories = lstCategory ?? new List<Category>(),
                Suppliers = lstSupplier ?? new List<Supplier>(),
                Product = product
            };

            return View(productDetail);
        }

        [HttpPost]
        public async Task<IActionResult> Save(Product product, IFormFile uploadPhoto)
        {
            Console.WriteLine(product.Price);
            Console.WriteLine(product.SupplierID);
            Console.WriteLine(product.CategoryID);

            ViewBag.Title = (product.ProductId == 0) ? "Thêm sản phẩm" : "Chỉnh sửa sản phẩm";

            if (string.IsNullOrEmpty(product.ProductName))
                ModelState.AddModelError(nameof(product.ProductName), "Tên sản phẩm không được để trống");
            if (product.CategoryID == 0)
                ModelState.AddModelError(nameof(product.CategoryID), "Vui lòng chọn loại sản phẩm");
            if (product.SupplierID == 0)
                ModelState.AddModelError(nameof(product.SupplierID), "Vui lòng chọn nhà cung cấp");

            product.Photo = product.Photo ?? "";
            product.Unit = product.Unit ?? "";

            int temp = 0;
            var lstPhoto = ProductDataService.ListOfProductPhotos(product.ProductId);
            var lstAttribute = ProductDataService.ListOfProductAttributes(product.ProductId);
            var lstCategory = CommonDataService.ListOfCategories(out temp, 1, 0, "");
            var lstSupplier = CommonDataService.ListOfSuppliers(out temp, 1, 0, "");
            var productDetail = new ProductDetail()
            {
                Photos = lstPhoto ?? new List<ProductPhoto>(),
                Attributes = lstAttribute ?? new List<ProductAttribute>(),
                Categories = lstCategory ?? new List<Category>(),
                Suppliers = lstSupplier ?? new List<Supplier>(),
                Product = product
            };

            if (!ModelState.IsValid)
            {
                return View("Edit", productDetail);
            }

            if (product.ProductId == 0)
            {
                product.ProductId = ProductDataService.AddProduct(product);
            }


            if (uploadPhoto != null && uploadPhoto.ContentType.Contains("image"))
            {
                var fileName = Path.GetFileName(product.ProductId.ToString() + Path.GetExtension(uploadPhoto.FileName));
                var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/products");

                // Kiểm tra và tạo thư mục nếu chưa tồn tại
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                var filePath = Path.Combine(directoryPath, fileName);
                Console.WriteLine(fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await uploadPhoto.CopyToAsync(stream);
                }

                product.Photo = $"/images/products/{fileName}";
            }


            ProductDataService.UpdateProduct(product);

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id = 0)
        {
            if (Request.Method == "POST")
            {
                ProductDataService.DeleteProduct(id);
                return RedirectToAction("Index");
            }

            Product? product = ProductDataService.GetProductById(id);

            if (product == null)
            {
                return RedirectToAction("Index");
            }

            ViewBag.AllowDelete = true;
            if (ProductDataService.IsUsed(id))
            {
                ViewBag.AllowDelete = false;
            }
            return View(product);
        }

        public IActionResult Photo(int id = 0, string method = "", int photoId = 0)
        {
            switch (method)
            {
                case "add":
                    ViewBag.Title = "Bổ sung ảnh cho mặt hàng";
                    ProductPhoto photo = new ProductPhoto
                    {
                        ProductId = id,
                        PhotoId = 0,
                        Description = "",
                        DisplayOrder = 100,
                        IsHidden = false,
                        Photo = ""
                    };
                    return View(photo);
                case "edit":
                    ViewBag.Title = "Thay đổi ảnh của mặt hàng";
                    ProductPhoto? img = ProductDataService.GetProductPhoto(photoId);
                    if (img == null) return RedirectToAction("Edit", new { id = id });
                    return View(img);
                case "delete":
                    ProductDataService.DeleteProductPhoto(photoId);
                    return RedirectToAction("Edit", new { id = id });
                default:
                    return RedirectToAction("Index");
            }
        }

        public IActionResult Attribute(int id = 0, string method = "", int attributeId = 0)
        {
            switch (method)
            {
                case "add":
                    ViewBag.Title = "Bổ sung thuộc tính cho mặt hàng";
                    var modelAdd = new ProductAttribute
                    {
                        AttributeId = 0,
                        AttributeValue = "",
                        DisplayOrder = 0,
                        AttributeName = "",
                        ProductId = id
                    };
                    return View(modelAdd);
                case "edit":
                    ViewBag.Title = "Thay đổi thuộc tính của mặt hàng";
                    var modelEdit = ProductDataService.GetAttributeById(attributeId);
                    if (modelEdit == null) return RedirectToAction("Edit", new { id = id });
                    return View(modelEdit);
                case "delete":
                    ProductDataService.DeleteAttribute(attributeId);
                    return RedirectToAction("Edit", new { id = id });
                default:
                    return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> SavePhoto(ProductPhoto productPhoto, IFormFile uploadPhoto)
        {
            ViewBag.Title = (productPhoto.PhotoId == 0) ? "Thêm ảnh" : "Chỉnh sửa ảnh";

            if (productPhoto.Equals("") && (uploadPhoto == null || !uploadPhoto.ContentType.Contains("image")))
                ModelState.AddModelError(nameof(productPhoto.Photo), "Chưa có ảnh hoặc ảnh không hợp lệ");

            if (!ModelState.IsValid)
            {
                if (productPhoto.PhotoId == 0)
                {
                    return RedirectToAction("Photo", new { id = productPhoto.ProductId, method = "add", photoId = 0 });
                }
                else
                {
                    return RedirectToAction("Photo", new { id = productPhoto.ProductId, method = "edit", photoId = productPhoto.PhotoId });
                }
            }

            productPhoto.Photo = productPhoto.Photo ?? "";
            productPhoto.Description = productPhoto.Description ?? "";

            if (productPhoto.PhotoId == 0)
            {
                productPhoto.PhotoId = ProductDataService.AddProductPhoto(productPhoto);
            }

            if (uploadPhoto != null && uploadPhoto.ContentType.Contains("image"))
            {
                var fileName = Path.GetFileName(productPhoto.PhotoId.ToString() + Path.GetExtension(uploadPhoto.FileName));
                var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/products/img");

                // Kiểm tra và tạo thư mục nếu chưa tồn tại
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                var filePath = Path.Combine(directoryPath, fileName);
                Console.WriteLine(fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await uploadPhoto.CopyToAsync(stream);
                }

                productPhoto.Photo = $"/images/products/img/{fileName}";
            }


            ProductDataService.UpdateProductPhoto(productPhoto);

            return RedirectToAction("Edit", new { id = productPhoto.ProductId });
        }

        [HttpPost]
        public IActionResult SaveAttribute(ProductAttribute productAttribute)
        {
            ViewBag.Title = (productAttribute.AttributeId == 0)
                ? "Bổ sung thuộc tính cho mặt hàng " : "Chỉnh sửa thuộc tính cho mặt hàng";

            productAttribute.AttributeValue = productAttribute.AttributeValue ?? "";
            productAttribute.AttributeName = productAttribute.AttributeName ?? "";

            if (productAttribute.AttributeId == 0)
                ProductDataService.AddAttribute(productAttribute);
            else
                ProductDataService.UpdateAttribute(productAttribute);

            return RedirectToAction("Edit", new { id = productAttribute.ProductId });
        }
    }
}
