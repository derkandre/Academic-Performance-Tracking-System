using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Academic_Performance_Tracking_System.Utilities
{
    internal class DatabaseConnection
    {        
        public static string ConnectionString { get; set; } = $@"data source={Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\apts.dat")};Foreign Keys=True";
    }
}
