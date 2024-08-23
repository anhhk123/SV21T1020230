using System.Globalization;

namespace SV21T1020230.Web.Models
{
    public class OrderSearchInput : PaginationSearchInput
    {
        public int Status { get; set; }

        public string orderTime { get; set; }

        public string GetDateFrom()
        {
            DateTime? dateFrom = null;
            if (!string.IsNullOrEmpty(orderTime))
            {
                var dates = orderTime.Split(" - ");
                if (dates.Length == 2)
                {
                    if (DateTime.TryParseExact(dates[0], "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime fromDate))
                    {
                        dateFrom = fromDate;
                    }
                }
            }
            return dateFrom.HasValue ? dateFrom.Value.ToString("yyyy-MM-dd") : "";
        }

        public string GetDateTo()
        {
            DateTime? dateTo = null;
            if (!string.IsNullOrEmpty(orderTime))
            {
                var dates = orderTime.Split(" - ");
                if (dates.Length == 2)
                {
                    if (DateTime.TryParseExact(dates[1], "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime toDate))
                    {
                        dateTo = toDate;
                    }
                }
            }
            return dateTo.HasValue ? dateTo.Value.ToString("yyyy-MM-dd") : "";
        }
    }
}
