﻿using EZAvailability.Model;
using EZAvailability.Services.Base;
using EZAvailability.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZAvailability.Services
{
    public class CategoryService
    {

        public async static Task<ResponseModel> GetCategories(int limitPerPage, int page)
        {

            // Set the API Endpoint to make the request
            string endpoint = ConnectionViewModel.ConnectionUrl + "/category?limitPerPage=" + limitPerPage + "&page=" + page;

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

        public async static Task<ResponseModel> GetCategoryById(long category_id)
        {

            // Set the API Endpoint to make the request
            string endpoint = ConnectionViewModel.ConnectionUrl + "/category?categoryId=" + category_id;

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

    }
}
