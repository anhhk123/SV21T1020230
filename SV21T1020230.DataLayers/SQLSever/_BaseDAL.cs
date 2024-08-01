using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV21T1020230.DataLayers.SQLSever
{
    /// <summary>
    /// Lớp đống vai trò là lớp cha cho các lớp cài đặt phép xử lý dữ liệu 
    /// trên cơ sử dữ liệu SQL Sever
    /// </summary>
    public abstract class _BaseDAL
    {
        protected string _connectionString = "";
        protected _BaseDAL( string connectionString)
        {
            _connectionString = connectionString;
        }
        /// <summary>
        /// Tạo và mở kết nối đến SQLSever
        /// </summary>
        /// <returns></returns>
        protected SqlConnection OpenConnection()
        {
            SqlConnection connection =  new SqlConnection( _connectionString );
            connection.Open();
            return connection;
        }

    }
}
