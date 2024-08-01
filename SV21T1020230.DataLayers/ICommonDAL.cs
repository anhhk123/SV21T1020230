using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV21T1020230.DataLayers
{
    public interface ICommonDAL<T> where T : class
    {
        /// <summary>
        /// Tìm kiếm và lấy danh sách dữ liệu dưới dạng phân trang
        /// </summary>
        /// <param name="page">Trang cần hiển thị</param>
        /// <param name="pageSize">Số dong hiển thị trên mỗi trang(bằng 0 nếu không phân trang)</param>
        /// <param name="searchValue">Giá trị tìm kiếm (Chuỗi rỗng nếu không tìm kiếm </param>
        /// <returns></returns>
        IList<T> List(int page = 1, int pageSize = 0, string searchValue= "");
        /// <summary>
        /// Đếm số dòng dữ liệu tìm kiếm được 
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        int Count(string searchValue = "");
        /// <summary>
        /// Lấy một bản ghi/ dòng dữ liệu dựa trên mã id
        /// </summary>
        /// <param name="id">Mã của dữ liệu cần lấy </param>
        /// <returns></returns>
        T? Get(int id);
        /// <summary>
        /// Bổ sung dữ liệu vào bảng . hàm trả về id của dữ liệu vừa bổ sung
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        int Add(T data);
        /// <summary>
        /// Cập nhật dữ liệu
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool Update(T data);
        /// <summary>
        /// Xóa một dòng dữ liệu dựa vào id 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool Delete(T data);
        /// <summary>
        /// Kiểm tra xem dữ liệu có có id này có liên quan đến các bảng khác không 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool InUsed(int id);
    }
}
