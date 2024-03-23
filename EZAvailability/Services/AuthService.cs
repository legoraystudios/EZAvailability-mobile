using EZAvailability.Data;
using EZAvailability.Services.Base;
using EZAvailability.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EZAvailability.Services
{
    public class AuthService
    {
        public AuthService() { }

        public async static Task<ResponseData> Token()
        {

            // Set the API Endpoint to make the request
            string endpoint = ConnectionViewModel.ConnectionUrl + "/auth/token";
            // Set the HTTPClient Handler
            var handler = BaseService.DefaultHttpClientHandler();
            // Set the Cookie Container in order to set the required
            // session cookies to access the protected content
            var cookieContainer = handler.CookieContainer;

            // Check if the URL Exist in ConnnectionUrl
            if (ConnectionViewModel.ConnectionUrl == "" || ConnectionViewModel.ConnectionUrl == "Not Found")
            {
                return new ResponseData { StatusCode = -1, JsonResponse = null };
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
                HttpResponseMessage apiResponse = await client.PostAsync(endpoint, null);
                // Obtain the JSON Response
                string jsonResponse = await apiResponse.Content.ReadAsStringAsync();

                if (apiResponse.IsSuccessStatusCode)
                {
                    // Get the new access token
                    var cookies = handler.CookieContainer.GetCookies(new Uri(endpoint));

                    // This for-loop isfor add the cookie to the list
                    SessionManager.Cookies = new List<string>();
                    foreach (Cookie cookie in cookies)
                    {
                        SessionManager.Cookies.Add(cookie.ToString());
                    }

                    // Return OK Status and User data if the request was success
                    return new ResponseData { StatusCode = 200, JsonResponse = jsonResponse }; ;
                }
                else
                {
                    // Request was not successful
                    return new ResponseData { StatusCode = (int)apiResponse.StatusCode, JsonResponse = jsonResponse };
                }
            }
        }

        public async static Task<int> Login(string email, string password)
        {

            string endpoint = ConnectionViewModel.ConnectionUrl + "/auth/login";

            if (ConnectionViewModel.ConnectionUrl == "" || ConnectionViewModel.ConnectionUrl == "Not Found")
            {
                return -1;
            }

            var handler = BaseService.DefaultHttpClientHandler();

            using (HttpClient client = new HttpClient(handler))
            {

                var requestData = new
                {
                    email = email,
                    password = password
                };

                using StringContent jsonContent = new(JsonSerializer.Serialize(requestData), Encoding.UTF8, "application/json");

                HttpResponseMessage apiResponse = await client.PostAsync(endpoint, jsonContent);

                if (apiResponse.IsSuccessStatusCode)
                {

                    var cookies = handler.CookieContainer.GetCookies(new Uri(endpoint));

                    SessionManager.Cookies = new List<string>();
                    foreach (Cookie cookie in cookies)
                    {
                        SessionManager.Cookies.Add(cookie.ToString());
                    }

                    return 200;
                }
                else
                {
                    return (int)apiResponse.StatusCode;
                }
            }
        }

    }
}
