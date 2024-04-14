using EZAvailability.Model;
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

        public async static Task<ResponseModel> Token()
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

                    // Return OK Status and User data if the request was successful
                    return new ResponseModel { StatusCode = 200, JsonResponse = jsonResponse }; ;
                }
                else
                {
                    // Request was not successful
                    return new ResponseModel { StatusCode = (int)apiResponse.StatusCode, JsonResponse = jsonResponse };
                }
            }
        }

        public async static Task<int> Login(string email, string password)
        {

            // Set the API Endpoint to make the request
            string endpoint = ConnectionViewModel.ConnectionUrl + "/auth/login";
            
            // Check if the URL Exist in ConnnectionUrl
            if (ConnectionViewModel.ConnectionUrl == "" || ConnectionViewModel.ConnectionUrl == "Not Found")
            {
                return -1;
            }

            // Set the HTTP Client Handler
            var handler = BaseService.DefaultHttpClientHandler();

            using (HttpClient client = new HttpClient(handler))
            {
                // Set JSON Body data for the request
                var requestData = new
                {
                    email = email,
                    password = password
                };

                // Serialize all the request data into a JSON
                using StringContent jsonContent = new(JsonSerializer.Serialize(requestData), Encoding.UTF8, "application/json");

                // Process the API request
                HttpResponseMessage apiResponse = await client.PostAsync(endpoint, jsonContent);

                // Login and set the cookies if the request was successful.
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
                    // Request was not successful.
                    return (int)apiResponse.StatusCode;
                }
            }
        }

        public async static Task<int> Logout()
        {
            
            // Set the API Endpoint to make the request
            string endpoint = ConnectionViewModel.ConnectionUrl + "/auth/logout";
            // Set the HTTPClient Handler
            var handler = BaseService.DefaultHttpClientHandler();
            // Set the Cookie Container in order to set the required
            // session cookies to access the protected content
            var cookieContainer = handler.CookieContainer;

            if (ConnectionViewModel.ConnectionUrl == "" || ConnectionViewModel.ConnectionUrl == "Not Found")
            {
                return -1;
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

                HttpResponseMessage apiResponse = await client.PostAsync(endpoint, null);

                if (apiResponse.IsSuccessStatusCode)
                {
                    SessionManager.Cookies.Clear();
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
