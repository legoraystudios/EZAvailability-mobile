using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZAvailability.Data
{
    public class ProductData
    {
        public long product_id {  get; set; }
        public string product_name { get; set; }
        public string product_desc { get; set; }
        public int product_qty { get; set;}
        public long product_upc { get; set; }
        public int low_stock_alert { get; set; }
        public long category_id { get; set; }
        public string category_name { get; set;}
        public int total_rows { get; set; }

    }
}
