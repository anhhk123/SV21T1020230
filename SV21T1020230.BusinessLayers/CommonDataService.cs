using System;
using SV21T1020230.DataLayers;
using SV21T1020230.DomainModels;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV21T1020230.BusinessLayers
{
    public static class CommonDataService
    {
        private static readonly CustomerDAL customerDB;
        static CommonDataService()
        {
            string connectionString = @"server=.;
                                    user id = sa;
                                    password = 123; 
                                    database =LiteCommerceDB;
                                    TrustServerCertificate=true";
            customerDB = new CustomerDAL(connectionString);
        }
        /// <summary>
        /// Viết chú thích ở đây
        /// </summary>
        /// <returns></returns>
        public static List<Customer> ListOfCustomers()
        {
            return customerDB.List();
        }
    }
}
