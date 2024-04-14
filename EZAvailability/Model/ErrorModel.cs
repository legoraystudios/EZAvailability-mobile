using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZAvailability.Model
{
    public class ErrorModel
    {
        public ErrorObject errors { get; set; }
    }

    public class ErrorObject
    {
        public string errCode { get; set; }
    }
}
