using SV21T1020230.DataLayers;
using SV21T1020230.DataLayers.SQLSever;
using SV21T1020230.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV21T1020230.BusinessLayers
{
    public static class ProductDataService
    {
        private static readonly IProductDAL productDB;
        static ProductDataService()
        {
            productDB = new ProductDAL(Configuration.ConnectionString);
        }
        /// <summary>
        /// Lấy ra danh sách mặt hàng cod phân trang
        /// </summary>
        /// <param name="rowCount"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <param name="categoryId"></param>
        /// <param name="supplierId"></param>
        /// <param name="minPrice"></param>
        /// <param name="maxPrice"></param>
        /// <returns></returns>
        public static List<Product> ListProducts(out int rowCount,int page = 1, int pageSize = 0, string searchValue = "", int categoryId = 0, int supplierId = 0, decimal minPrice = 0, decimal maxPrice = 0)
        {
            rowCount = productDB.Count(searchValue, categoryId, supplierId, minPrice,maxPrice);
            return productDB.List(page, pageSize, searchValue, categoryId, supplierId, minPrice, maxPrice).ToList();
        }
        /// <summary>
        /// Lấy ra mặt hàng bằng id
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public static Product GetProductById(int productId)
        {
            return productDB.Get(productId);
        }
        /// <summary>
        /// Thêm mặt hàng
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public static int AddProduct(Product product)
        {
            return productDB.Add(product);
        }
        /// <summary>
        /// Cập nhật mặt hàng
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public static bool UpdateProduct(Product product)
        {
            return productDB.Update(product);
        }
        /// <summary>
        /// Lấy ra danh sách ảnh của product id
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public static List<ProductPhoto> ListOfProductPhotos(int productId = 0)
        {
            return productDB.ListPhotos(productId).ToList();
        }
        /// <summary>
        /// Lấy ra danh sách thuộc tính của product id
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public static List<ProductAttribute> ListOfProductAttributes(int productId = 0)
        {
            return productDB.ListAttributes(productId).ToList();
        }
        /// <summary>
        /// Bổ sung thuộc tính cho mặt hàng
        /// </summary>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public static long AddAttribute(ProductAttribute attribute)
        {
            return productDB.AddAttribute(attribute);
        }
        public static bool UpdateAttribute(ProductAttribute attribute)
        {
            return productDB.UpdateAttribute(attribute);
        }
        public static bool DeleteAttribute(int idAttribute)
        {
            return productDB.DeleteAttribute(idAttribute);
        }
        public static ProductAttribute GetAttributeById(int attributeId)
        {
            return productDB.GetAttribute(attributeId);
        }
        /// <summary>
        /// Thêm ảnh cho mặt hàng
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static long AddProductPhoto(ProductPhoto data)
        {

            return productDB.AddPhoto(data);
        }
        /// <summary>
        /// Cập nhật ảnh sản phẩm
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool UpdateProductPhoto(ProductPhoto data)
        {

            return productDB.UpdatePhoto(data);
        }
        /// <summary>
        /// Lấy ra thôn tin ảnh mặt hang bằng id
        /// </summary>
        /// <param name="photoId"></param>
        /// <returns></returns>
        public static ProductPhoto GetProductPhoto(int photoId)
        {
            return productDB.GetPhoto(photoId);
        }
        public static bool DeleteProductPhoto(int photoId)
        {
            return productDB.DeletePhoto(photoId);
        }
        /// <summary>
        /// Xóa mặt hàng
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public static bool DeleteProduct(int productId)
        {

            return productDB.Delete(productId);
        }
        /// <summary>
        /// Kiểm tra mặt hàng có liên quan tới các bảng khác ko
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public static bool InUsed(int productId)
        {
            return productDB.InUsed(productId);
        }
        



    }
}
