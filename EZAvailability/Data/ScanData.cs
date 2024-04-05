using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZAvailability.Data
{
    public class ScanData
    {
        public ErrorObject errors { get; set; }
    }

    public class ErrorObject
    {
        public string errCode { get; set; }
    }
}
