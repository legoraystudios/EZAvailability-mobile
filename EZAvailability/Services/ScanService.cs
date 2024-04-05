using EZAvailability.Data;
using EZAvailability.Services.Base;
using EZAvailability.ViewModel;
using Microsoft.Maui.ApplicationModel.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EZAvailability.Services
{
    public class ScanService
    {
        public async static Task<ResponseData> ScanIn(long productUpc, int qty)
        {

            // Set the API Endpoint to make the request
            string endpoint = ConnectionViewModel.ConnectionUrl + "/scans/in";

            // Check if the URL Exist in ConnnectionUrl
            if (ConnectionViewModel.ConnectionUrl == "" || ConnectionViewModel.ConnectionUrl == "Not Found")
            {
                return new ResponseData { StatusCode = -1, JsonResponse = null };
            }

            // Set the HTTP Handler
            var handler = BaseService.DefaultHttpClientHandler();

            var requestData = new
            {
                productUpc = productUpc,
                productQty = qty
            };

            using StringContent jsonContent = new(JsonSerializer.Serialize(requestData), Encoding.UTF8, "application/json");

            // Set the Cookie Container in order to set the required
            // session cookies to access the protected content
            var cookieContainer = handler.CookieContainer;

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
                HttpResponseMessage apiResponse = await client.PostAsync(endpoint, jsonContent);
                // Obtain the JSON Response
                string jsonResponse = await apiResponse.Content.ReadAsStringAsync();


                if (apiResponse.IsSuccessStatusCode)
                {
                    // Return OK Status and products data if the request was success
                    return new ResponseData { StatusCode = 200, JsonResponse = jsonResponse };
                }
                else
                {
                    // Request was not successful
                    return new ResponseData { StatusCode = (int)apiResponse.StatusCode, JsonResponse = jsonResponse };
                }
            }
        }

        public async static Task<ResponseData> ScanOut(long productUpc, int qty)
        {

            // Set the API Endpoint to make the request
            string endpoint = ConnectionViewModel.ConnectionUrl + "/scans/out";

            // Check if the URL Exist in ConnnectionUrl
            if (ConnectionViewModel.ConnectionUrl == "" || ConnectionViewModel.ConnectionUrl == "Not Found")
            {
                return new ResponseData { StatusCode = -1, JsonResponse = null };
            }

            // Set the HTTP Handler
            var handler = BaseService.DefaultHttpClientHandler();

            var requestData = new
            {
                productUpc = productUpc,
                productQty = qty
            };

            using StringContent jsonContent = new(JsonSerializer.Serialize(requestData), Encoding.UTF8, "application/json");

            // Set the Cookie Container in order to set the required
            // session cookies to access the protected content
            var cookieContainer = handler.CookieContainer;

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
                HttpResponseMessage apiResponse = await client.PostAsync(endpoint, jsonContent);
                // Obtain the JSON Response
                string jsonResponse = await apiResponse.Content.ReadAsStringAsync();


                if (apiResponse.IsSuccessStatusCode)
                {
                    // Return OK Status and products data if the request was success
                    return new ResponseData { StatusCode = 200, JsonResponse = jsonResponse };
                }
                else
                {
                    // Request was not successful
                    return new ResponseData { StatusCode = (int)apiResponse.StatusCode, JsonResponse = jsonResponse };
                }
            }
        }
    }
}
