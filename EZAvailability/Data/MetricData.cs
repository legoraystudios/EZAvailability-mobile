using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZAvailability.Data
{
    public class MetricData
    {
        public int total_products {  get; set; }
        public int total_stock { get; set; }
        public int total_low_stock_items { get; set; }
        public int total_out_of_stock { get; set; }

    }
}
