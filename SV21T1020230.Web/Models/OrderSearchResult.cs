using SV21T1020230.DomainModels;

namespace SV21T1020230.Web.Models
{
    public class OrderSearchResult : PaginationSearchResult<Order>
    {
        public int Status { get; set; } = 0;
        public string TimeRange { get; set; } = "";
    }
}
