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
    }

}
