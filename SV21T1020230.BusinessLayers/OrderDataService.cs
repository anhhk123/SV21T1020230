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
    public class OrderDataService
    {
        private static IOrderDAL orderDb;
        static OrderDataService()
        {
            orderDb = new OrderDAL(Configuration.ConnectionString);
        }
        /// <summary>
        /// Lấy ra danh sách trạng thái đơn hàng
        /// </summary>
        /// <returns></returns>
        public static List<StatusOrder> ListStatusOrder()
        {
            return orderDb.ListOfStatusOrder().ToList();
        }
        /// <summary>
        /// Lấy ra danh sách đơn hàng
        /// </summary>
        /// <param name="rowCount"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <param name="status"></param>
        /// <param name="dateTo"></param>
        /// <param name="form"></param>
        /// <returns></returns>
        public static List<Order> ListOfOrders(out int rowCount ,int page = 1, int pageSize = 0, string searchValue = "", int status = 0, string dateTo = "", string form = "")
        {
            rowCount = orderDb.Count(searchValue, status, dateTo, form);
            return orderDb.ListOfOrder(page,pageSize,searchValue, status,dateTo,form).ToList();
        }
        /// <summary>
        /// Lấy ra thôn tin đơn hàng bằng id đơn hàng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static OrderDetail GetOrderById(int id)
        {
            return orderDb.GetOrder(id);
        }

        /// <summary>
        /// Xóa đơn hàng bằng id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool DeleteOrderById(int id)
        {
            return orderDb.DeleteOrder(id);
        }
        /// <summary>
        /// Danh sách sản phẩm trong order
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public static List<ProductInOrderDetail> ListProductInOrders(int orderId)
        {
            return orderDb.ListOfProductInOrderDetail(orderId).ToList();
        }
        /// <summary>
        /// Xóa sản phẩm khỏi hóa đơn bằng orderId và productId
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        public static bool DeleteProductInOrders(int orderId, int productId)
        {
            return orderDb.DeleteProductInDetail(orderId, productId);
        }
    }
}
