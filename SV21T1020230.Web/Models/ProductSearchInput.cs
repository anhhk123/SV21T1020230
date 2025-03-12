using SV21T1020230.DomainModels;

namespace SV21T1020230.Web.Models
{
    public class ProductSearchInput : PaginationSearchInput
    {
        public int? SupplierID { get; set; }
        public int? CategoryID { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }

        public Order? Order { get; set; }
    }
}
