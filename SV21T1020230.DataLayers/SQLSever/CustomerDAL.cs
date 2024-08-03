using Dapper;
using SV21T1020230.DomainModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV21T1020230.DataLayers.SQLSever
{
    public class CustomerDAL : _BaseDAL, ICommonDAL<Customer>
    {
        public CustomerDAL(string connectionString) : base(connectionString)
        {
        }

        public int Add(Customer data)
        {
            int id = 0;
            using (var connection = OpenConnection())
            {
                var sql = @"INSERT INTO Customers(CustomerName,ContactName,Province,Address,Phone,Email,IsLocked)
                VALUES(@CustomerName,@ContactName,@Province,@Address,@Phone,@Email,@IsLocked);
                    SELECT @@IDENTITY";
                var parameters = new
                {
                    CustomerName = data.CustomerName ?? "",
                    ContactName = data.ContactName ?? "",
                    Province = data.Province ?? "",
                    Address = data.Address ?? "",
                    Phone = data.Phone ?? "",
                    Email = data.Email ?? "",
                    IsLocked = data.IsLocked,
                };
                id = connection.ExecuteScalar<int>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();

            }
            return id;
        }

        public int Count(string searchValue = "")
        {
            int count = 0;
            using (var connection = OpenConnection())
            {
                var sql = @"select COUNT(*)  
                    from Customers  
                    where (CustomerName like @searchValue) or (ContactName like @searchValue)";

                var parameter = new
                {
                    searchValue = $"%{searchValue}%"
                };

                // Thực hiện truy vấn và chuyển đổi kết quả  
                var result = connection.ExecuteScalar(sql, param: parameter, commandType: System.Data.CommandType.Text);

                // Kiểm tra nếu kết quả không phải là null  
                if (result != null)
                {
                    count = Convert.ToInt32(result);
                }

                return count;
            }
        }

        public bool Delete(int id)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"DELETE FROM Customers WHERE CustomerId = @CustomerId";
                var param = new
                {
                    CustomerId = id
                };
                result = connection.Execute(sql, param, commandType: System.Data.CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }

        public Customer? Get(int id)
        {
            Customer? data = null;
            using (var connection = OpenConnection())
            {
                var sql = @"SELECT * FROM Customers WHERE CustomerId = @CustomerId";
                var param = new
                {
                    CustomerId = id
                };
                data = connection.QueryFirstOrDefault<Customer>(sql, param, commandType: System.Data.CommandType.Text);
            }
            return data;
        }

        public bool InUsed(int id)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"IF EXISTS(SELECT * FROM Orders WHERE CustomerId = @CustomerId) 
                                SELECT 1
                            ELSE
                                SELECT 0";
                var param = new
                {
                    CustomerId = id
                };
                result = connection.ExecuteScalar<int>(sql, param, commandType: System.Data.CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }

        public IList<Customer> List(int page = 1, int pageSize = 0, string searchValue = "")
        {
            List<Customer> data = new List<Customer>(); 
            using( var connection = OpenConnection())
            {
                var sql = @"

                    select *
                    from ( select *, 
			        ROW_NUMBER() over (order by CustomerName) as RowNumber
			        from Customers
			        where (Customername like @searchValue) or (ContactName like @searchValue)
			        )as t
			        where (@pageSize = 0)
			        or RowNumBer between(@page -1) * @pageSize +1 and @page * @pageSize
			        order by RowNumBer";
                var parameter = new
                {
                    page = page,
                    pageSize = pageSize,
                    searchValue = $"%{searchValue}%"
                };
                data = connection.Query<Customer>(sql : sql, param: parameter, commandType: System.Data.CommandType.Text).ToList();
            }
            return data;
        }

        public bool Update(Customer data)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"UPDATE Customers
                     SET CustomerName = @CustomerName,
                             ContactName  = @ContactName, 
                             Province     = @Province, 
                             Address      = @Address, 
                             Phone        = @Phone, 
                             Email        = @Email, 
                             IsLocked      = @IsLocked
                     WHERE CustomerId = @CustomerId";

                var param = new
                {
                    CustomerId = data.CustomerId,
                    CustomerName = data.CustomerName ?? "",
                    ContactName = data.ContactName ?? "",
                    Province = data.Province ?? "",
                    Address = data.Address ?? "",
                    Phone = data.Phone ?? "",
                    Email = data.Email ?? "",
                    IsLocked = data.IsLocked
                };
                result = connection.ExecuteScalar<int>(sql: sql, param: param, commandType: System.Data.CommandType.Text) > 0;
            }
            return result;
        }
    }
}
