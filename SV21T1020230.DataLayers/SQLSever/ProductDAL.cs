using Dapper;
using SV21T1020230.DomainModels;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV21T1020230.DataLayers.SQLSever
{
    public class ProductDAL : _BaseDAL, IProductDAL
    {
        public ProductDAL(string connectionString) : base(connectionString)
        {
        }

        /// <summary>
        /// Thêm mặt hàng
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public int Add(Product data)
        {
            int id = 0;
            using (var connection = OpenConnection())
            {
                var sql = @"INSERT INTO Products
                            (
                                ProductName,
                                ProductDescription, 
                                SupplierID, 
                                CategoryID, 
                                Unit,
                                Price,
                                Photo,
                                IsSelling
                                )
                        VALUES(
                                @ProductName,
                                @ProductDescription,
                                @SupplierID,
                                @CategoryID,
                                @Unit,
                                @Price,
                                @Photo,
                                @IsSelling
                                );
                            SELECT @@IDENTITY";
                var parameters = new
                {
                    ProductName = data.ProductName,
                    ProductDescription = data.ProductDescription,
                    SupplierID = data.SupplierID,
                    CategoryID = data.CategoryID,
                    Unit = data.Unit,
                    Price = data.Price,
                    Photo = data.Photo,
                    IsSelling = data.IsSelling,


                };
                id = connection.ExecuteScalar<int>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();

            }
            return id;
        }
        /// <summary>
        /// Thêm thuộc tính sản phẩm
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public long AddAttribute(ProductAttribute data)
        {
            long id = 0;
            using (var connection = OpenConnection())
            {
                var sql = @"INSERT INTO ProductAttributes
                            (
                                ProductID,
                                AttributeName, 
                                AttributeValue,
                                DisplayOrder
                            )
                            VALUES
                            (
                                @ProductID,
                                @AttributeName, 
                                @AttributeValue,
                                @DisplayOrder
                            );
                            SELECT CAST(SCOPE_IDENTITY() as bigint)";
                var parameters = new
                {
                    ProductID = data.Productid,
                    AttributeName = data.AttributeName,
                    AttributeValue = data.AttributeValue,
                    DisplayOrder = data.DisplayOrder
                };
                int tempId = connection.ExecuteScalar<int>(sql: sql, param: parameters, commandType: CommandType.Text);

                
                id = (long)tempId;
                connection.Close();
            }
            return id;
        }

        /// <summary>
        /// Thêm ảnh sản phẩm
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public long AddPhoto(ProductPhoto data)
        {
            long id = 0;

            using (var connection = OpenConnection())
            {
                var sql = @"INSERT INTO ProductPhotos(ProductID, Photo)
                            VALUES(@ProductID, @Photo);
                           SELECT CAST(SCOPE_IDENTITY() AS bigint)";
                var parameters = new
                {
                    ProductID = data.ProductId,
                    Photo = data.Photo
                };
                int tempId = connection.ExecuteScalar<int>(sql: sql, param: parameters, commandType: CommandType.Text);

                // Chuyển đổi kiểu int sang long
                id = (long)tempId;
                connection.Close();
            }
            return id;
        }
        /// <summary>
        /// Tổng số sản phẩm tìm ra theo từ khóa phân loại
        /// </summary>
        /// <param name="searchValue"></param>
        /// <param name="categoryID"></param>
        /// <param name="supplierID"></param>
        /// <param name="minPrice"></param>
        /// <param name="maxPrice"></param>
        /// <returns></returns>
        public int Count(string searchValue = "", int categoryID = 0, int supplierID = 0, decimal minPrice = 0, decimal maxPrice = 0)
        {
            int count = 0;
            using(var connection = OpenConnection())
            {
                string sql = @"SELECT COUNT(*)
                                FROM Products
                                WHERE (@searchValue = '' OR ProductName LIKE '%' + @searchValue + '%')
                                  AND (@categoryId = 0 OR CategoryId = @categoryId)
                                  AND (@supplierId = 0 OR SupplierId = @supplierId)
                                  AND (@minPrice = 0 OR Price >= @minPrice)
                                  AND (@maxPrice = 0 OR Price <= @maxPrice);";
                var parameter = new
                {
                    searchValue = searchValue,
                    categoryId = categoryID,
                    supplierId = supplierID,
                    minPrice = minPrice,
                    maxPrice = maxPrice
                };
                var result = connection.ExecuteScalar(sql : sql, param : parameter, commandType : System.Data.CommandType.Text);
                if(result != null )
                {
                    count = Convert.ToInt32(result);
                }
            }
            return count;
        }
        /// <summary>
        /// Xóa mặt hàng bằng  id
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public bool Delete(int productId)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"DELETE FROM Products 
                            WHERE ProductID = @ProductId";
                var parameters = new
                {
                    ProductId = productId
                };
                result = connection.Execute(sql: sql, param: parameters, commandType: CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }
        /// <summary>
        /// Xóa thuộc tích mặt hàng bằng id thuộc tính
        /// </summary>
        /// <param name="attributeId"></param>
        /// <returns></returns>
        public bool DeleteAttribute(long attributeId)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"DELETE FROM ProductAttributes 
                             WHERE AttributeID = @AttributeId";
                var parameters = new
                {
                    AttributeId = attributeId
                };
                result = connection.Execute(sql: sql, param: parameters, commandType: CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }
        /// <summary>
        /// Xóa ảnh mặt hàng bằng id ảnh
        /// </summary>
        /// <param name="photoId"></param>
        /// <returns></returns>
        public bool DeletePhoto(long photoId)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"DELETE FROM ProductPhotos 
                                WHERE PhotoID = @PhotoID";
                var parameters = new 
                {
                    PhotoID = photoId
                };
                result = connection.Execute(sql: sql, param: parameters, commandType: CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }

        /// <summary>
        /// Lấy thông tin product có id được truyền vào 
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public Product? Get(int productId)
        {
            Product? result = null;
            using(var connection = OpenConnection())
            {
                string sql = @"select * FROM Products
                                where ProductID = @productID";
                var parameter = new
                {
                    productID = productId
                };
                result = connection.Query<Product?>(sql : sql,param : parameter, commandType : System.Data.CommandType.Text).FirstOrDefault();
            }
            return result;
        }
        /// <summary>
        /// Lấy ra thuộc tính bằng attributeID
        /// </summary>
        /// <param name="attributeID"></param>
        /// <returns></returns>
        public ProductAttribute? GetAttribute(long attributeID)
        {
            ProductAttribute? productAttribute = null;
            using (var connection = OpenConnection())
            {
                string sqlcmd = @"select * from ProductAttributes
                                    where ProductAttributes.AttributeID = @AttributeID";
                var parameter = new
                {
                    AttributeID = attributeID
                };
                productAttribute = connection.Query<ProductAttribute>(sql: sqlcmd, param: parameter, commandType: CommandType.Text).FirstOrDefault();
            }
            return productAttribute;
        }
        /// <summary>
        /// Lấy ra thông tin ảnh bằng photoId
        /// </summary>
        /// <param name="photoId"></param>
        /// <returns></returns>
        public ProductPhoto? GetPhoto(long photoId)
        {
            ProductPhoto? productPhoto = null;
            using(var connection = OpenConnection())
            {
                string sqlcmd = @"select * from ProductPhotos
                                    where PhotoID = @PhotoId";
                var parameter = new
                {
                    PhotoId = photoId
                };
                productPhoto = connection.Query<ProductPhoto>(sql : sqlcmd, param : parameter, commandType : CommandType.Text).FirstOrDefault();
            }
            return productPhoto;
        }

        public bool InUsed(int productId)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {

                var sql = @"IF EXISTS (SELECT * 
                                        FROM OrderDetails 
                                        WHERE ProductId = @ProductId)
                                 SELECT 1
                             ELSE 
                                  SELECT 0 ";
                var parameters = new
                {
                    ProductId = productId,
                };
                result = connection.ExecuteScalar<int>(sql: sql, param: parameters, commandType: CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }
        /// <summary>
        /// Lấy ra danh sách sản phẩm có phân trang
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <param name="categoryId"></param>
        /// <param name="supplierId"></param>
        /// <param name="minPrice"></param>
        /// <param name="maxPrice"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public IList<Product> List(int page = 1, int pageSize = 10, string searchValue = "", int categoryId = 0, int supplierId = 0, decimal minPrice = 0, decimal maxPrice = 0)
        {
            List<Product> products = new List<Product>();
            using (var connection = OpenConnection())
            {
                string sql = @"SELECT *
                       FROM (
                           SELECT *,
                                  ROW_NUMBER() OVER(ORDER BY ProductName) AS RowNumber
                           FROM Products
                           WHERE (@SearchValue = '' OR ProductName LIKE '%' + @SearchValue + '%')
                             AND (@CategoryID = 0 OR CategoryID = @CategoryID)
                             AND (@SupplierID = 0 OR SupplierId = @SupplierID)
                             AND (Price >= @MinPrice)
                             AND (@MaxPrice <= 0 OR Price <= @MaxPrice)
                       ) AS t
                       WHERE (@PageSize = 0)
                         OR (RowNumber BETWEEN (@Page - 1) * @PageSize + 1 AND @Page * @PageSize)";
                var parameter = new
                {
                    SearchValue = searchValue,
                    CategoryID = categoryId,
                    SupplierID = supplierId,
                    MinPrice = minPrice,
                    MaxPrice = maxPrice,
                    Page = page,
                    PageSize = pageSize,
                };

                try
                {
                    products = connection.Query<Product>(sql, parameter).ToList();
                }
                catch (Exception ex)
                {
                    // Log the exception or handle it accordingly
                    throw new Exception("Error retrieving products", ex);
                }
            }
            return products;
        }

        /// <summary>
        /// Lấy ra danh sách các thuộc tính của mặt hàng
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public IList<ProductAttribute> ListAttributes(int productId)
        {
            List<ProductAttribute> data = new List<ProductAttribute>();
            using (var connection = OpenConnection())
            {
                var sql = @"SELECT * FROM 
                            ProductAttributes 
                            WHERE ProductID = @ProductID";
                var parameters = new 
                { 
                    ProductID = productId
                };
                data = connection.Query<ProductAttribute>(sql: sql, param: parameters, commandType: CommandType.Text).ToList();
                connection.Close();
            }
            return data;
        }
        /// <summary>
        /// Lấy ra danh sách ảnh bằng producId 
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public IList<ProductPhoto> ListPhotos(int productId)
        {
            IList<ProductPhoto> data = new List<ProductPhoto>();
            using (var connection = OpenConnection())
            {
                var sql = "SELECT * FROM ProductPhotos WHERE ProductID = @ProductID";
                var parameters = new
                {
                    ProductID = productId
                };
                data = connection.Query<ProductPhoto>(sql: sql, param: parameters, commandType: CommandType.Text).ToList();
                connection.Close();

            }
            return data;
        }

        /// <summary>
        /// Cập nhật thông tin sản phẩm
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool Update(Product data)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"UPDATE Products SET 
                            ProductName = @ProductName, 
                            ProductDescription = @ProductDescription, 
                            Unit = @Unit, 
                            Price = @Price, 
                            Photo = @Photo, 
                            IsSelling = @IsSelling 
                            WHERE ProductID = @ProductID";

                var parameters = new
                {
                    ProductName = data.ProductName,
                    ProductDescription = data.ProductDescription,
                    Unit = data.Unit,
                    Price = data.Price,
                    Photo = data.Photo,
                    IsSelling = data.IsSelling,
                    ProductID = data.ProductId
                };
                result = connection.Execute(sql: sql, param: parameters, commandType: CommandType.Text) > 0;
            }
            return result;
        }

        /// <summary>
        /// Cập nhật thuộc tính sản phẩm
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool UpdateAttribute(ProductAttribute data)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"UPDATE ProductAttributes
                            SET AttributeName = @AttributeName,
                                AttributeValue = @AttributeValue
                            WHERE AttributeID = @AttributeID";
                var parameters = new
                {
                    AttributeName = data.AttributeName,
                    AttributeValue = data.AttributeValue,
                    AttributeID = data.AttributeId
                };
                result = connection.Execute(sql: sql, param: parameters, commandType: CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }

        /// <summary>
        /// Cập nhật ảnh sản phẩm 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool UpdatePhoto(ProductPhoto data)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"UPDATE ProductPhotos
                            SET Photo = @Photo
                            WHERE PhotoID = @PhotoID";
                var parameters = new
                {
                    Photo = data.Photo,
                    PhotoID = data.PhotoId
                };
                result = connection.Execute(sql: sql, param: parameters, commandType: CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }
    }
}
