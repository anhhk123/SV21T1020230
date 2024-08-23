using SV21T1020230.DomainModels;
using System.Security.Cryptography.Pkcs;

namespace SV21T1020230.Web.Models
{
    public class PaginationSearchResult<T>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string SearchValue { get; set; }
        public int RowCount { get; set; }
        public required List<T> data { get; set; }


        public int PageCount
        {
            get
            {
                if (PageSize == 0)
                    return 1;
                int n = RowCount / PageSize;
                if (RowCount % PageSize > 0)
                {
                    n += 1;
                }
                return n;
            }
        }
    }
    /// <summary>
    /// Kết quả tìm kiếm khách hàng 
    /// </summary>

    public class CustomerSearchResult : PaginationSearchResult<Customer>
    {

    }
    
    public class EmployeeSearchResult : PaginationSearchResult<Employee>
    {
    }
    public class ProductSearchResult : PaginationSearchResult<Product>
    {
        public int CategoryID { get; set; }
        public int SupplierID { get; set; }
        public int MinPrice { get; set; }
        public int MaxPrice { get; set; }
        public List<Category> Categories { get; set; } = new List<Category>();
        public List<Supplier> Suppliers { get; set; } = new List<Supplier>();
    }
    public class OrderSearchResult : PaginationSearchResult<Order>
    {
        public string orderTime { get; set; }
    }

}
