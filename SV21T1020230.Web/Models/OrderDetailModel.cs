﻿using SV21T1020230.DomainModels;

namespace SV21T1020230.Web.Models
{
    public class OrderDetailModel
    {
        public Order Order { get; set; }
        public List<OrderDetail> Details { get; set; }
    }
}
