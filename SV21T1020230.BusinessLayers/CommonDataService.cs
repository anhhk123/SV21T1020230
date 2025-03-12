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
        static readonly ICommonDAL<Supplier> supplierDB;
        static readonly ICommonDAL<Product> productDB;
        static readonly ICommonDAL<Category> categoryDB;
        static readonly ICommonDAL<Shipper> shipperDB;
        static readonly ICommonDAL<Employee> employeeDB;
        static CommonDataService()
        {
            provinceDB = new ProvinceDAL(Configuration.ConnectionString);
            customerDB = new CustomerDAL(Configuration.ConnectionString);
            supplierDB = new SupplierDAL(Configuration.ConnectionString);
            employeeDB = new EmployeeDAL(Configuration.ConnectionString);
            categoryDB = new CategoryDAL(Configuration.ConnectionString);
            shipperDB = new ShipperDAL(Configuration.ConnectionString);
            
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
        /// <summary>
        /// Lấy ra danh danh sách nhà cung cấp có phân trang
        /// </summary>
        /// <param name="rowCount"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchvalue"></param>
        /// <returns></returns>

        public static List<Supplier> ListOfSuppliers(out int rowCount, int page = 1, int pageSize = 0, string searchvalue = "")
        {
            rowCount = supplierDB.Count(searchvalue);
            return supplierDB.List(page, pageSize, searchvalue).ToList();

        }
        public static List<Supplier> ListOfSuppliers()
        {
            
            return supplierDB.List().ToList();

        }

        /// <summary>
        /// Lấy ra danh sách employee
        /// </summary>
        /// <param name="rowCount"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public static List<Employee> ListofEmployees(out int rowCount, int page = 1, int pageSize = 0, string searchValue = "")
        {
            rowCount = employeeDB.Count(searchValue);
            return employeeDB.List(page, pageSize, searchValue).ToList();
        }
        /// <summary>
        /// Lây ra nhân viên có mã nv = id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Employee GetEmployee(int id)
        {
            return employeeDB.Get(id);
        }
        /// <summary>
        /// Thêm mới nhân viên
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int AddEmployee(Employee data)
        {
            return employeeDB.Add(data);
        }
        /// <summary>
        /// Cập nhật thông tin nhân viên
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool UpdateEmployee(Employee data)
        {
            return employeeDB.Update(data);
        }
        /// <summary>
        /// Xóa nhân viên bằng id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool DeleteEmployee(int id)
        {
            return employeeDB.Delete(id);
        }
        /// <summary>
        /// Kiểm tra nhân viên có các thông tin liên quan ở các bảng khác không
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool IsUsedEmployee(int id)
        {
            return employeeDB.InUsed(id);
        }
        /// <summary>
        /// Lấy ra nhà cung cấp có id bằng id nhập vào
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Supplier GetSupplier(int id)
        {
            return supplierDB.Get(id);
        }
        /// <summary>
        /// Thêm mới nhà cung cấp
        /// </summary>
        /// <param name="supplier"></param>
        /// <returns>Trả về id nhà cung cấp</returns>
        public static int AddSupplier(Supplier supplier)
        {
            return supplierDB.Add(supplier);
        }
        /// <summary>
        /// Cập nhật nhà cung cấp
        /// </summary>
        /// <param name="supplier"></param>
        /// <returns></returns>
        public static bool UpdateSupplier(Supplier supplier)
        {
            return supplierDB.Update(supplier);
        }

        public static bool IsUsedSupplier(int id)
        {
            return supplierDB.InUsed(id);
        }
        /// <summary>
        /// Xóa nhà cung cấp bằng id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool DeleteSupplier(int id)
        {
            return supplierDB.Delete(id);
        }
        /// <summary>
        /// Lấy ra danh sách shipper
        /// </summary>
        /// <param name="rowCount"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public static List<Shipper> ListOfShippers(out int rowCount, int page = 1, int pageSize = 0, string searchValue = "")
        {
            rowCount = shipperDB.Count(searchValue);
            return shipperDB.List(page, pageSize, searchValue).ToList();
        }
        /// <summary>
        /// Lấy thông tin shipper bằng id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Shipper? GetShipper(int id)
        {
            return shipperDB.Get(id);
        }
        /// <summary>
        /// Add Shipper
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int AddShipper(Shipper data)
        {
            return shipperDB.Add(data);
        }
        /// <summary>
        /// Cập nhật thông tin shipper
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool UpdateShipper(Shipper data)
        {
            return shipperDB.Update(data);
        }
        /// <summary>
        /// Xóa thông tin shipper
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool DeleteShipper(int id)
        {
            return shipperDB.Delete(id);
        }
        /// <summary>
        /// Kiểm tra xem shipper có bị sử dụng ở một bảng khác không
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool IsUsedShipper(int id)
        {
            return shipperDB.InUsed(id);
        }
        /// <summary>
        /// Lấy ra danh sách loại hàng
        /// </summary>
        /// <param name="rowCount"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public static List<Category> ListOfCategories(out int rowCount, int page = 1, int pageSize = 1, string searchValue = "")
        {
            rowCount = categoryDB.Count(searchValue);
            return categoryDB.List(page, pageSize, searchValue).ToList();
        }
        public static List<Category> ListAllCategories()
        {
            
            return categoryDB.List().ToList();
        }
        /// <summary>
        /// Lấy ra một loại hàng bằng id loại hàng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Category? GetCategory(int id)
        {
            return categoryDB.Get(id);
        }
        public static int AddCategory(Category data)
        {
            return categoryDB.Add(data);
        }
        public static bool UpdateCategory(Category data)
        {
            return categoryDB.Update(data);
        }
        public static bool DeleteCategory(int id)
        {
            return categoryDB.Delete(id);
        }
        public static bool IsUsedCategory(int id)
        {
            return categoryDB.InUsed(id);
        }
    }

}
