﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV21T1020230.BusinessLayers
{
    /// <summary>
    /// Cấu hình cho tầng Business
    /// </summary>
    public static class Configuration
    {
        public static string ConnectionString { get; private set; } = "";
        public static void Initialize(string connectionString)
        {
            ConnectionString = connectionString;
        }
    }
}
