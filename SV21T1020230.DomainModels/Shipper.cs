﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV21T1020230.DomainModels
{
    public class Shipper
    {
        public int ShipperID { get; set; }
        public string ShipperName { get; set; }
        public string Phone { get; set; } = string.Empty;
    }
}
