using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EZAvailability.Services.Base
{
    public class BaseService
    {
        public static HttpClientHandler DefaultHttpClientHandler()
        {
            var handler = new HttpClientHandler
            {
                UseCookies = true,
                CookieContainer = new CookieContainer()
            };
            return handler;
        }
    }
}
