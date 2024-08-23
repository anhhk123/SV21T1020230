using Azure;
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
    public class OrderDAL : _BaseDAL , IOrderDAL
    {
        public OrderDAL(string connectionString) : base(connectionString)
        {
        }

        public int Count(string searchValue = "", int status = 0, string dateTo = "", string form = "")
        {
            int count = 0;
            using (var connection = OpenConnection())
            {
                string sql = @"SELECT COUNT(*)
                                FROM Orders
                                INNER JOIN Customers ON Customers.CustomerID = Orders.CustomerID
                                INNER JOIN Employees ON Employees.EmployeeID = Orders.EmployeeID
                                INNER JOIN Shippers ON Shippers.ShipperID = Orders.ShipperID
                                WHERE (@SearchValue = '' 
                                       OR Customers.CustomerName LIKE '%' + @SearchValue + '%'
                                       OR Employees.FullName LIKE '%' + @SearchValue + '%'
                                       OR Shippers.ShipperName LIKE '%' + @SearchValue + '%')
                                  AND (@Status = 0 OR Orders.Status = @Status)
                                  AND ((@DateFrom = '') OR (@DateTo = '') 
                                       OR (Orders.OrderTime BETWEEN @DateFrom AND @DateTo));";
                var parameter = new
                {
                    searchValue = searchValue,
                    Status = status,
                    DateFrom = form,
                    DateTo = dateTo
                };
                var result = connection.ExecuteScalar(sql: sql, param: parameter, commandType: System.Data.CommandType.Text);
                if (result != null)
                {
                    count = Convert.ToInt32(result);
                }
            }
            return count;
        }

        public int CreateOrder(Order order)
        {
            throw new NotImplementedException();
        }

        public bool DeleteOrder(int orderId)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"delete from OrderDetails where OrderID = @OrderID;
                            delete from Orders where OrderID = @OrderID";
                var parameters = new
                {
                    orderID = orderId,
                };
                result = connection.Execute(sql: sql, param: parameters, commandType: CommandType.Text) > 0;
            }
            return result;
        }

        public bool DeleteProductInDetail(int OrderId, int ProductId)
        {
            bool result = false;
            using(var connection = OpenConnection())
            {
                string sql = @"DELETE FROM [dbo].[OrderDetails]
                                  WHERE OrderID = @orderId
	                              AND ProductID = @productId";
                var parameters = new
                {
                    orderId = OrderId,
                    productId = ProductId
                };
                result = connection.Execute(sql: sql, param: parameters, commandType: CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }

        public bool EditDetail(int id, int productId)
        {
            throw new NotImplementedException();
        }

        public OrderDetail GetOrder(int orderId)
        {
            OrderDetail? data = null;
            using (var connection = OpenConnection())
            {
                var sql = @"select o.*,
                            c.CustomerName,

                            c.ContactName as CustomerContactName,
                            c.Address as CustomerAddress,
                            c.Phone as CustomerPhone,
                            c.Email as CustomerEmail,
                            e.FullName as EmployeeName,
                            s.ShipperName,
                            s.Phone as ShipperPhone
                       from Orders as o
                            left join Customers as c on o.CustomerID = c.CustomerID
                            left join Employees as e on o.EmployeeID = e.EmployeeID
                            left join Shippers as s on o.ShipperID = s.ShipperID

                       where o.OrderID = @OrderID";
                var parameters = new
                {
                    OrderId = orderId,
                };
                data = connection.QueryFirstOrDefault<OrderDetail>(sql: sql, param: parameters, commandType: CommandType.Text);
            }
            return data;
        }






        public IList<Order> ListOfOrder(int page = 1, int pageSize = 0, string searchValue = "", int status = 0, string dateTo = "", string form = "")
        {
            List<Order> orders = new List<Order>();
            using (var connection = OpenConnection())
            {
                string sql = @"SELECT *
                                    FROM (
                                        SELECT 
                                            Orders.OrderID,
                                            Customers.CustomerID, 
                                            Customers.CustomerName,
                                            Orders.OrderTime,
                                            Employees.EmployeeID,
                                            Employees.FullName,
                                            Orders.AcceptTime,
                                            Shippers.ShipperID, 
                                            Shippers.ShipperName,
                                            Orders.ShippedTime,
                                            Orders.FinishedTime,
                                            Orders.Status,
                                            ROW_NUMBER() OVER(ORDER BY Orders.OrderTime DESC) AS RowNumber
                                        FROM Orders
                                        INNER JOIN Customers ON Customers.CustomerID = Orders.CustomerID
                                        INNER JOIN Employees ON Employees.EmployeeID = Orders.EmployeeID
                                        INNER JOIN Shippers ON Shippers.ShipperID = Orders.ShipperID
                                        WHERE (@SearchValue = '' 
                                               OR Customers.CustomerName LIKE '%' + @SearchValue + '%'
                                               OR Employees.FullName LIKE '%' + @SearchValue + '%'
                                               OR Shippers.ShipperName LIKE '%' + @SearchValue + '%')
                                          AND (@Status = 0 OR Orders.Status = @Status)
                                          AND ((@DateFrom = '') OR (@DateTo = '') 
                                       OR (Orders.OrderTime BETWEEN @DateFrom AND @DateTo)) 
                                    ) AS t
                                    WHERE (@PageSize = 0) 
                                       OR (RowNumber BETWEEN (@Page - 1) * @PageSize + 1 AND @Page * @PageSize);";

                var parameter = new
                {
                    SearchValue = searchValue,
                    PageSize = pageSize,
                    Page = page,
                    Status = status,
                    DateFrom = form,
                    DateTo = dateTo

                };
                try
                {
                    orders = connection.Query<Order, Customer, Employee, Shipper, Order>(sql, (order, customer, employee, shipper) =>
                    {
                        order.Customer = customer;
                        order.Employee = employee;
                        order.Shipper = shipper;
                        return order;
                    },
                     splitOn: "CustomerID,EmployeeID,ShipperID",
                     param: parameter
                    ).ToList();
                }
                catch (Exception ex)
                {
                    // Log the exception or handle it accordingly
                    throw new Exception("Loi lay danh sach order :OrderDAL", ex);
                }
            }
            return orders;
            
        }

        public IList<ProductInOrderDetail> ListOfProductInOrderDetail(int orderId)
        {
            List<ProductInOrderDetail> data = null;
            using(var connection = OpenConnection())
            {
                string sql = @"SELECT [OrderID]
                                  ,dbo.OrderDetails.ProductID
	  
                                  ,[Quantity]
                                  ,[SalePrice]
	                              , dbo.Products.ProductName
	                              , dbo.Products.Unit
	                              , Quantity * SalePrice AS ThanhTien
                              FROM [dbo].[OrderDetails], dbo.Products
                              where OrderDetails.ProductID = Products.ProductID
                              AND OrderID = @orderId";
                var Parameter = new
                {
                    orderId = orderId,
                };
                data = connection.Query<ProductInOrderDetail>(sql, param: Parameter, commandType: CommandType.Text).ToList();
            }
            return data;
        }

        public IList<StatusOrder> ListOfStatusOrder()
        {
            List<StatusOrder> statusOrders = new List<StatusOrder>();
            using (var connection = OpenConnection())
            {
                string sql = @"select * from OrderStatus";
                statusOrders = connection.Query<StatusOrder>(sql: sql, commandType: System.Data.CommandType.Text).ToList();
                connection.Close();
            }
            return statusOrders;
        }

        public bool UpdateOrder(Order order)
        {
            throw new NotImplementedException();
        }
    }
}
