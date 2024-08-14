using SV21T1020230.DomainModels;
using System.Security.Cryptography.Pkcs;

namespace SV21T1020230.Web.Models
{
    public class PaginationSearchResult
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string SearchValue { get; set; }
        public int RowCount { get; set; }

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

    public class CustomerSearchResult : PaginationSearchResult
    {
        public required List<Customer> data { get; set; }
    }
    public class ProductSearchResult : PaginationSearchResult
    {
        public required List<Product> data { get; set; }
    }
}
