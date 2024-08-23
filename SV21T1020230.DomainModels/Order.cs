using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV21T1020230.DomainModels
{
    public class Order
    {
        public int OrderID { get; set; }
        public Customer Customer { get; set; }
        public DateTime OrderTime { get; set; }
        public string DeliveryProvince { get; set; }
        public string DeliveryAddress { get; set; }
        public Employee Employee { get; set; }
        public DateTime AcceptTime { get; set; }
        public Shipper Shipper { get; set; }
        public DateTime ShippedTime { get; set; }
        public DateTime FinishedTime { get; set; }
        public StatusOrder Status { get; set; }

    }
}
