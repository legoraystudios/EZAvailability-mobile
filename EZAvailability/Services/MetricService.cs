using EZAvailability.Model;
using EZAvailability.Services.Base;
using EZAvailability.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EZAvailability.Services
{
    public class MetricService
    {
        public MetricService() { }

        public async static Task<ResponseModel> GetMetrics()
        {

            // Set the API Endpoint to make the request
            string endpoint = ConnectionViewModel.ConnectionUrl + "/stats/";
            // Set the HTTPClient Handler
            var handler = BaseService.DefaultHttpClientHandler();
            // Set the Cookie Container in order to set the required
            // session cookies to access the protected content
            var cookieContainer = handler.CookieContainer;

            // Check if the URL Exist in ConnnectionUrl
            if (ConnectionViewModel.ConnectionUrl == "" || ConnectionViewModel.ConnectionUrl == "Not Found")
            {
                return new ResponseModel { StatusCode = -1, JsonResponse = null };
            }

            using (HttpClient client = new HttpClient(handler))
            {
                // Setting the session cookies (refresh and access
                // token) to be able to access to this protected
                // endpoint
                foreach (var cookie in SessionManager.Cookies)
                {
                    cookieContainer.SetCookies(new Uri(endpoint), cookie);
                }

                // Process the API request
                HttpResponseMessage apiResponse = await client.GetAsync(endpoint);
                // Obtain the JSON Response
                string jsonResponse = await apiResponse.Content.ReadAsStringAsync();

                if (apiResponse.IsSuccessStatusCode)
                {
                    // Return OK Status and User data if the request was success
                    return new ResponseModel { StatusCode = 200, JsonResponse = jsonResponse }; ;
                }
                else
                {
                    // Request was not successful
                    return new ResponseModel { StatusCode = (int)apiResponse.StatusCode, JsonResponse = jsonResponse };
                }
            }
        }
    }
}
