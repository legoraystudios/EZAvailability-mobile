using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZAvailability.Model
{
    public class Connection
    {
        public int Id { get; set; }
        public string ApiUrl { get; set; }

        public Connection()
        {
            ApiUrl = String.Empty;
        }

        public Connection(string apiUrl)
        {
            ApiUrl = apiUrl;
        }
    }
}
