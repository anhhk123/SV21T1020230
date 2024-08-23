﻿
using SV21T1020230.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV21T1020230.DataLayers
{
    public interface IOrderDAL
    {
        /// <summary>
        /// Lấy ra danh sách đơn hàng
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <param name="status"></param>
        /// <param name="dateTo"></param>
        /// <param name="form"></param>
        /// <returns></returns>
        IList<Order> ListOfOrder(int page = 1, int pageSize = 0, string searchValue = "", int status = 0, string dateTo = "", string form = "");

        /// <summary>
        /// Đếm số lượng mặt hang tìm kiếm được
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <param name="status"></param>
        /// <param name="dateTo"></param>
        /// <param name="form"></param>
        /// <returns></returns>
        int Count(string searchValue = "", int status = 0, string dateTo = "", string form = "");

        bool UpdateOrder(Order order);

        int CreateOrder(Order order);
        /// <summary>
        /// Xóa Order bằng Id
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        bool DeleteOrder(int orderId);
        /// <summary>
        /// Get order by id
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        OrderDetail GetOrder(int orderId);
        /// <summary>
        /// Danh sách trạng thái đơn hàng
        /// </summary>
        /// <returns></returns>
        IList<StatusOrder> ListOfStatusOrder();

        /// <summary>
        /// Chỉnh sửa thông tin chi tiết sản phẩn trong đơn hàng 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        bool EditDetail(int id, int productId);
        /// <summary>
        /// Xóa sản phẩm trong hóa đơn
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool DeleteProductInDetail(int OrderId, int ProductId);
        /// <summary>
        /// Lấy ra danh sách sản phẩm   trong hóa đơn
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        IList<ProductInOrderDetail> ListOfProductInOrderDetail(int orderId);



    }
}
