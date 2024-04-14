using EZAvailability.Model;
using EZAvailability.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace EZAvailability.Utilities
{
    public class CategoryPicker
    {

        private static int page = 1;
        private static int limitPerPage = 10;
        private static int totalItems = 0;
        private double totalPages;

        public async Task<Picker> GetCategoryPickerAsync(long categoryId)
        {
            var categoryPicker = new Picker();
            List<CategoryModel> categoryData = await GetCategoryData();

            int totalRows = categoryData[0].total_rows;
            totalItems = totalRows;
            totalPages = Math.Ceiling((double)totalItems / limitPerPage);

            categoryPicker.Title = "Please Select...";

            if (categoryData.Count > 0)
            {
                while (totalPages + 1 > page)
                {
                    foreach (var category in categoryData)
                    {
                        categoryPicker.Items.Add($"{category.category_id} | {category.category_name}");
                    }

                    for (int i = 0; i < categoryData.Count; i++)
                    {
                        if (categoryData[i].category_id == categoryId)
                        {
                            var selectedIndex = categoryData.IndexOf(categoryData[i]);
                            categoryPicker.SelectedIndex = selectedIndex;
                        }
                    }

                    page++;
                    categoryData.Clear();
                    categoryData = await GetCategoryData();
                }
            }
            page = 1;

            return categoryPicker;

        }

        public async Task<long> GetCategoryIdByIndex(int index)
        {
            List<CategoryModel> categoryData = await GetCategoryData();

            int totalRows = categoryData[0].total_rows;
            totalItems = totalRows;
            totalPages = Math.Ceiling((double)totalItems / limitPerPage);

            if (categoryData.Count > 0)
            {
                while (totalPages + 1 > page)
                {
                    for (int i = 0; i < categoryData.Count; i++)
                    {
                        if (i == index)
                        {
                            return categoryData[i].category_id;
                        }
                    }
                }
            }
            page = 1;

            return -1;
        }

        public static async Task<List<CategoryModel>> GetCategoryData()
        {
            ResponseModel response = await CategoryService.GetCategories(limitPerPage, page);

            string jsonResponse = response.JsonResponse;

            List<CategoryModel> categoryData = JsonConvert.DeserializeObject<List<CategoryModel>>(jsonResponse);

            return categoryData;
        }

    }
}
