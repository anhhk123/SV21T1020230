using Dapper;
using SV21T1020230.DomainModels;
using System;
using System.Collections.Generic;
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
            throw new NotImplementedException();
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

        public bool Delete(Customer data)
        {
            throw new NotImplementedException();
        }

        public Customer? Get(int id)
        {
            throw new NotImplementedException();
        }

        public bool InUsed(int id)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }
    }
}
