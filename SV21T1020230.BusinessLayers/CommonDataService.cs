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

    public static class CommonDataService
    {
        static readonly ICommonDAL<Province> provinceDB;
        static readonly ICommonDAL<Customer> customerDB;
        static CommonDataService()
        {
            provinceDB = new DataLayers.SQLSever.ProvinceDAL(Configuration.ConnectionString);
            customerDB = new DataLayers.SQLSever.CustomerDAL(Configuration.ConnectionString);
        }
        public static List<Province> ListOfProvinces()
        {
            return provinceDB.List().ToList();
        }
        /// <summary>
        /// Danh sách khác hàng (tìm kiếm phân trang)
        /// </summary>
        /// <param name="rowCount"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchvalue"></param>
        /// <returns></returns>
        public static List<Customer> ListOfCustomers(out int rowCount, int page = 1, int pageSize = 0, string searchvalue = "")
        {
            rowCount = customerDB.Count(searchvalue);
            return customerDB.List(page, pageSize, searchvalue).ToList();
        }
        /// <summary>
        /// Danh sách khách hàng (tim kiếm không phân trang)
        /// </summary>
        /// <param name="searchvalue"></param>
        /// <returns></returns>
        public static List<Customer> ListOfCustomer( string searchvalue = "")
        {
            
            return customerDB.List(1,0, searchvalue).ToList();
        }
        /// <summary>
        /// Thêm mới một khách hàng
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int AddCustomer(Customer data)
        {
            return customerDB.Add(data);
        }
        /// <summary>
        /// Cập nhật thông tin khách hàng
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool UpdateCustomer(Customer data)
        {
            return customerDB.Update(data);
        }
        /// <summary>
        /// Xóa một khách hàng bằng id khách hàng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool DeleteCustomer(int id)
        {
            return customerDB.Delete(id);
        }
        /// <summary>
        ///  Kiểm tra khách hàng có đơn hàng không
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool InUsed(int id)
        {
            return customerDB.InUsed(id);
        }
        /// <summary>
        /// Lấy ra customer có id  bằng id đưa vào
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Customer GetCustomer(int id) 
        {
            return customerDB.Get(id);
        }

    }

}
