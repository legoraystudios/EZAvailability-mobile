﻿using EZAvailability.Model;
using EZAvailability.Services.Base;
using EZAvailability.ViewModel;
using Microsoft.Maui.ApplicationModel.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EZAvailability.Services
{
    public class ProductService
    {
        public ProductService() { }

        public async static Task<ResponseModel> GetProducts(int limitPerPage, int page)
        {

            // Set the API Endpoint to make the request
            string endpoint = ConnectionViewModel.ConnectionUrl + "/products?limitPerPage=" + limitPerPage + "&page=" + page;
            
            // Check if the URL Exist in ConnnectionUrl
            if (ConnectionViewModel.ConnectionUrl == "" || ConnectionViewModel.ConnectionUrl == "Not Found")
            {
                return new ResponseModel { StatusCode = -1, JsonResponse = null };
            }

            // Set the HTTP Handler
            var handler = BaseService.DefaultHttpClientHandler();

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
                HttpResponseMessage apiResponse = await client.GetAsync(endpoint);
                // Obtain the JSON Response
                string jsonResponse = await apiResponse.Content.ReadAsStringAsync();


                if (apiResponse.IsSuccessStatusCode)
                {
                    // Return OK Status and products data if the request was success
                    return new ResponseModel { StatusCode = 200, JsonResponse = jsonResponse };
                }
                else
                {
                    // Request was not successful
                    return new ResponseModel { StatusCode = (int)apiResponse.StatusCode, JsonResponse = jsonResponse };
                }
            }
        }

        public async static Task<ResponseModel> GetProductsByCategoryId(int categoryId, int limitPerPage, int page)
        {

            // Set the API Endpoint to make the request
            string endpoint = ConnectionViewModel.ConnectionUrl + "/products?limitPerPage=" + limitPerPage + "&page=" + page + "&categoryId=" + categoryId;

            // Check if the URL Exist in ConnnectionUrl
            if (ConnectionViewModel.ConnectionUrl == "" || ConnectionViewModel.ConnectionUrl == "Not Found")
            {
                return new ResponseModel { StatusCode = -1, JsonResponse = null };
            }

            // Set the HTTP Handler
            var handler = BaseService.DefaultHttpClientHandler();

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
                HttpResponseMessage apiResponse = await client.GetAsync(endpoint);
                // Obtain the JSON Response
                string jsonResponse = await apiResponse.Content.ReadAsStringAsync();


                if (apiResponse.IsSuccessStatusCode)
                {
                    // Return OK Status and products data if the request was success
                    return new ResponseModel { StatusCode = 200, JsonResponse = jsonResponse };
                }
                else
                {
                    // Request was not successful
                    return new ResponseModel { StatusCode = (int)apiResponse.StatusCode, JsonResponse = jsonResponse };
                }
            }
        }

        public async static Task<ResponseModel> GetProductsById(long id)
        {

            // Set the API Endpoint to make the request
            string endpoint = ConnectionViewModel.ConnectionUrl + "/products?productId=" + id;

            // Check if the URL Exist in ConnnectionUrl
            if (ConnectionViewModel.ConnectionUrl == "" || ConnectionViewModel.ConnectionUrl == "Not Found")
            {
                return new ResponseModel { StatusCode = -1, JsonResponse = null };
            }

            // Set the HTTP Handler
            var handler = BaseService.DefaultHttpClientHandler();

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
                HttpResponseMessage apiResponse = await client.GetAsync(endpoint);
                // Obtain the JSON Response
                string jsonResponse = await apiResponse.Content.ReadAsStringAsync();


                if (apiResponse.IsSuccessStatusCode)
                {
                    // Return OK Status and products data if the request was success
                    return new ResponseModel { StatusCode = 200, JsonResponse = jsonResponse };
                }
                else
                {
                    // Request was not successful
                    return new ResponseModel { StatusCode = (int)apiResponse.StatusCode, JsonResponse = jsonResponse };
                }
            }
        }

        public async static Task<ResponseModel> GetProductsByUpc(long upc)
        {

            // Set the API Endpoint to make the request
            string endpoint = ConnectionViewModel.ConnectionUrl + "/products?productUpc=" + upc;

            // Check if the URL Exist in ConnnectionUrl
            if (ConnectionViewModel.ConnectionUrl == "" || ConnectionViewModel.ConnectionUrl == "Not Found")
            {
                return new ResponseModel { StatusCode = -1, JsonResponse = null };
            }

            // Set the HTTP Handler
            var handler = BaseService.DefaultHttpClientHandler();

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
                HttpResponseMessage apiResponse = await client.GetAsync(endpoint);
                // Obtain the JSON Response
                string jsonResponse = await apiResponse.Content.ReadAsStringAsync();


                if (apiResponse.IsSuccessStatusCode)
                {
                    // Return OK Status and products data if the request was success
                    return new ResponseModel { StatusCode = 200, JsonResponse = jsonResponse };
                }
                else
                {
                    // Request was not successful
                    return new ResponseModel { StatusCode = (int)apiResponse.StatusCode, JsonResponse = jsonResponse };
                }
            }
        }

        public async static Task<ResponseModel> CreateProduct(string productName, string productDesc, int productQty, 
            long productUpc, int lowStockAlert, long categoryId)
        {
            // Set the API Endpoint to make the request
            string endpoint = ConnectionViewModel.ConnectionUrl + "/products/create";

            // Check if the URL Exist in ConnnectionUrl
            if (ConnectionViewModel.ConnectionUrl == "" || ConnectionViewModel.ConnectionUrl == "Not Found")
            {
                return new ResponseModel { StatusCode = -1, JsonResponse = null };
            }

            // Set the HTTP Handler
            var handler = BaseService.DefaultHttpClientHandler();

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

                var requestData = new
                {
                    productName = productName,
                    productDesc = productDesc,
                    productQty = productQty,
                    productUpc = productUpc,
                    lowStockAlert = lowStockAlert,
                    categoryId = categoryId
                };

                using StringContent jsonContent = new(JsonSerializer.Serialize(requestData), Encoding.UTF8, "application/json");

                // Process the API request
                HttpResponseMessage apiResponse = await client.PostAsync(endpoint, jsonContent);
                // Obtain the JSON Response
                string jsonResponse = await apiResponse.Content.ReadAsStringAsync();


                if (apiResponse.IsSuccessStatusCode)
                {
                    // Return OK Status and products data if the request was success
                    return new ResponseModel { StatusCode = 200, JsonResponse = jsonResponse };
                }
                else
                {
                    // Request was not successful
                    return new ResponseModel { StatusCode = (int)apiResponse.StatusCode, JsonResponse = jsonResponse };
                }
            }
        }

        public async static Task<ResponseModel> GetProductByName(string name, int limitPerPage, int page)
        {

            // Set the API Endpoint to make the request
            string endpoint = ConnectionViewModel.ConnectionUrl + "/products?limitPerPage=" + limitPerPage + "&page=" + page + "&productName=" + name;

            // Check if the URL Exist in ConnnectionUrl
            if (ConnectionViewModel.ConnectionUrl == "" || ConnectionViewModel.ConnectionUrl == "Not Found")
            {
                return new ResponseModel { StatusCode = -1, JsonResponse = null };
            }

            // Set the HTTP Handler
            var handler = BaseService.DefaultHttpClientHandler();

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
                HttpResponseMessage apiResponse = await client.GetAsync(endpoint);
                // Obtain the JSON Response
                string jsonResponse = await apiResponse.Content.ReadAsStringAsync();


                if (apiResponse.IsSuccessStatusCode)
                {
                    // Return OK Status and products data if the request was success
                    return new ResponseModel { StatusCode = 200, JsonResponse = jsonResponse };
                }
                else
                {
                    // Request was not successful
                    return new ResponseModel { StatusCode = (int)apiResponse.StatusCode, JsonResponse = jsonResponse };
                }
            }
        }

        public async static Task<ResponseModel> UpdateProduct(long productId, string productName, string productDesc,
            int productQty, long productUpc, int lowStockAlert, long categoryId)
        {

            // Set the API Endpoint to make the request
            string endpoint = ConnectionViewModel.ConnectionUrl + "/products/edit";

            // Check if the URL Exist in ConnnectionUrl
            if (ConnectionViewModel.ConnectionUrl == "" || ConnectionViewModel.ConnectionUrl == "Not Found")
            {
                return new ResponseModel { StatusCode = -1, JsonResponse = null };
            }

            // Set the HTTP Handler
            var handler = BaseService.DefaultHttpClientHandler();

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

                var requestData = new
                {
                    productId = productId,
                    productName = productName,
                    productDesc = productDesc,
                    productQty = productQty,
                    productUpc = productUpc,
                    lowStockAlert = lowStockAlert,
                    categoryId = categoryId
                };

                using StringContent jsonContent = new(JsonSerializer.Serialize(requestData), Encoding.UTF8, "application/json");

                // Process the API request
                HttpResponseMessage apiResponse = await client.PutAsync(endpoint, jsonContent);
                // Obtain the JSON Response
                string jsonResponse = await apiResponse.Content.ReadAsStringAsync();


                if (apiResponse.IsSuccessStatusCode)
                {
                    // Return OK Status and products data if the request was success
                    return new ResponseModel { StatusCode = 200, JsonResponse = jsonResponse };
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
