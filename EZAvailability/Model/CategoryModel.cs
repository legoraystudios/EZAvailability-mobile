using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZAvailability.Model
{
    public class CategoryModel
    {
        private int _totalQty;

        public long category_id { get; set; }
        public string category_name { get; set; }
        public string categoy_desc { get; set; }
        public int low_stock_alert { get; set; }
        public int total_rows { get; set; }
        public int total_products { get; set; }
    }
}
