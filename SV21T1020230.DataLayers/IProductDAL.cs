using SV21T1020230.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV21T1020230.DataLayers
{
    public interface IProductDAL
    {

        /// <summary>
        /// Tìm kiếm cà lấy danh sách mặt hàng dưới dạng phân trang
        /// </summary>
        /// <param name="page">Trang cần hiển thị</param>
        /// <param name="pageSize">Số dòng trên mỗi trang(0 nếu không phân tran)</param>
        /// <param name="searchValue">Tên mặt hang cần tìm (chuỗi rỗng nếu không tìm kiếm)</param>
        /// <param name="categoryId">Mã loại hàng cần tìm(0 nếu tìm theo nhà cung cấp)</param>
        /// <param name="supplierId">Mã nhà cung cấp cần tìm (0 nếu không tìm theo nhà cung cấp)</param>
        /// <param name="minPrice">Mức giá nhỏ nhất trong khoảng giá cần tìm</param>
        /// <param name="maxPrice">Mức giá lớn nhất trong khoảng giá cần tìm(0 nếu hạn chế mức giá lớn nhất)</param>
        /// <returns></returns>
        IList<Product> List(int page = 1, int pageSize = 0,
                                string searchValue = "", int categoryId = 0, int supplierId = 0,
                                decimal minPrice = 0, decimal maxPrice = 0);

        /// <summary>
        /// Đếm số lượng mặt hàng tìm kiếm được
        /// </summary>
        /// <param name="searchValue">Tên mặt hàng cần tìm(chuỗi rỗng nếu không tìm kiếm)</param>
        /// <param name="categoryId">Mã loại hàng cần tìm (0 nếu không tìm theo loại hàng)</param>
        /// <param name="supplierId">Mã nhà cung cấp cần tìm (0 nếu tìm theo nhà cung cấp)</param>
        /// <param name="minPrice">Mức giá nhỏ nhất trong khoảng giá cần tìm</param>
        /// <param name="maxPrice">Mức giá lơn nhất trong khoảng giá cần tìm(0 nếu không hạn chế mức giá lơn nhất)</param>
        /// <returns></returns>
        int Count(string searchValue = "", int categoryId = 0, int supplierId = 0, decimal minPrice = 0, decimal maxPrice = 0);
        /// <summary>
        /// Lấy thông tin mặt hàng theo mã hàng
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        Product? Get(int productId);
        /// <summary>
        /// Bổ sung thông tin mặt hang mới (hàm trả về mã của mặt hàng được bổ sung)
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        int Add(Product data);
        /// <summary>
        /// Xóa mặt hàng
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool Update(Product data);
        /// <summary>
        /// Xóa mặt hang theo id mã hàng
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        bool Delete(int productId);
        /// <summary>
        /// Kiếm tra xem mặt hàng hiện có đơn hàng liên quan hay không?
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        bool InUsed(int productId);
        /// <summary>
        /// Lấy ra danh sách ảnh của mặt hàng (Sắp xếp theo thứ tự của DisPlayOrder)
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        IList<ProductPhoto> ListPhotos(int productId);
        /// <summary>
        /// Lấy thông tin 1 ảnh dựa vào Id
        /// </summary>
        /// <param name="photoId"></param>
        /// <returns></returns>
        ProductPhoto? GetPhoto(long photoId);
        /// <summary>
        /// Bổ sung ảnh cho mặt hàng(hàm trả về mã của ảnh được bổ sung)
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        long AddPhoto(ProductPhoto data);
        /// <summary>
        /// Cập nhật ảnh của mặt hàng
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool UpdatePhoto(ProductPhoto data);
        /// <summary>
        /// Xóa ảnh mặt hàng
        /// </summary>
        /// <param name="photoId"></param>
        /// <returns></returns>
        bool DeletePhoto(long photoId);
        /// <summary>
        /// Lấy ra danh sách thuộc tính của mặt hàng, sắp xếp theo thứ tự của DisplayOrder
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        IList<ProductAttribute> ListAttributes(int productId);
        /// <summary>
        /// Bổ sung thuộc tính của mặt hàng
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        ProductAttribute? GetAttribute(long productId);
        /// <summary>
        /// Cập nhật thuộc tính cho mặt hàng
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        long AddAttribute(ProductAttribute data);
        /// <summary>
        /// Cập nhật thuộc tính cho mặt hàng
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool UpdateAttribute(ProductAttribute data);
        /// <summary>
        /// Xóa thuộc tính của mặt hàng
        /// </summary>
        /// <param name="attributeId"></param>
        /// <returns></returns>
        bool DeleteAttribute(long attributeId);



    }
}
