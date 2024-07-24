using System;
using Dapper;
using SV21T1020230.DomainModels;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace SV21T1020230.DataLayers
{
    public class CustomerDAL
    {
        private string connectionString = "";
        /// <summary>
        /// Constructor: Hàm được gọi khi tạo ra một đối tượng (object)
        /// là thể hiện (instance) của một lớp (class)
        /// </summary>
        /// <param name="connectionString"></param>
        public CustomerDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }
        /// <summary>
        /// Lấy danh sách toàn bộ khách hàng:
        /// Truy vấn và lấy dữ liệu trả về cho hàm
        /// </summary>
        /// <returns></returns>
        public List<Customer> List()
        {
            List<Customer> list = new List<Customer>();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.ConnectionString = connectionString;
                connection.Open();
                var sql = @"SELECT * FROM Customers";
                list = connection.Query<Customer>(sql: sql, commandType: System.Data.CommandType.Text).ToList();

                connection.Close();

            }
            return list;
        }
    }
}
