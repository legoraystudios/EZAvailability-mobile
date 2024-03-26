using EZAvailability.Data;
using EZAvailability.Services;
using EZAvailability.ViewModel;
using EZAvailability.ViewModel.Base;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Security.Cryptography.X509Certificates;
namespace EZAvailability.Views;

public partial class InventoryView : ContentPage
{
    // Initializing ViewModel
    private BaseViewModel baseViewModel = new BaseViewModel();
    private ProductViewModel productViewModel = new ProductViewModel();

    // Product Lists
    List<ProductData> productInfo;
    public List<ProductData> Products { get; set; }

    // Pagination variables for product ListView
    private static int limitPerPage = 10;
    private int page = 1;

    // Variables for the Search Bar
    private string SearchValue {  get; set; }
    private int SelectedSearchType;

    public InventoryView()
	{
		InitializeComponent();
        LoadingModule.BindingContext = baseViewModel;

        ProductList.Scrolled += OnProductListViewScrolled;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        baseViewModel.isLoading = true;
        await LoadUser();
        await LoadProducts();
        baseViewModel.isLoading = false;
    }

    private async Task LoadUser()
    {
        List<UserData> userInfo = await CheckIfLogin();
        BindingContext = userInfo;
    }

    private async Task LoadProducts()
    {
        BindingContext = null;
        productInfo = await GetProducts();
        Products = productInfo;
        BindingContext = this;
    }



    private async Task<List<ProductData>> GetProducts()
    {
        try
        {
            if (SearchValue != null) // If the Search Bar is NOT NULL
            {
                if (SelectedSearchType == 0) // If Search Type is by Product Name
                {

                } else if (SelectedSearchType == 1) // If Search Type is by Product ID
                {

                } else if (SelectedSearchType == 2) // If Search Type is by Product Name
                {

                } else // Get Products
                {

                }
            } else // Get Products
            {

            }

            ResponseData response = await ProductService.GetProducts(limitPerPage, page);
            string jsonResponse = response.JsonResponse;

            List<ProductData> productData = JsonConvert.DeserializeObject<List<ProductData>>(jsonResponse);

            if (response.StatusCode != 200)
            {
                await Navigation.PushAsync(new MainPage());
            }

            return productData;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "An error has occured while loading the products. Please, try again later." + ex, "Okay");
            return null;
        }

    }

    private async void OnProductListViewScrolled(object sender, ScrolledEventArgs e)
    {
        var scrollView = sender as ScrollView;

        // Check if the sender object is null or if loading is already in progress
        if (scrollView == null || baseViewModel.isLoading)
            return;

        var scrollY = e.ScrollY;
        var contentHeight = scrollView.ContentSize.Height; // Use ContentSize to get the total height of the content
        var scrollViewHeight = scrollView.Height;

        // If the content height is not available or the scroll view height is zero, return
        if (contentHeight <= 0 || scrollViewHeight <= 0)
            return;

        // Calculate the ratio of how far the user has scrolled
        var scrollRatio = scrollY / (contentHeight - scrollViewHeight);

        // If the scroll ratio is greater than or equal to 80%, trigger loading more products
        if (scrollRatio >= 0.8)
        {
            baseViewModel.isLoading = true;
            page++; // Increment the page number for pagination
            await LoadNextPageProducts();
            baseViewModel.isLoading = false;
        }
    }

    private async Task LoadNextPageProducts()
    {
        // Call your method to fetch the next page of products
        List<ProductData> nextPageProducts = await GetProducts();

        if (nextPageProducts.Count > 0)
        {
            baseViewModel.isLoading = true;
            productInfo.AddRange(nextPageProducts);

            BindingContext = null;
            BindingContext = this;
            OnPropertyChanged(nameof(Products));
            baseViewModel.isLoading = false;
        }

    }

    private async Task<List<UserData>> CheckIfLogin()
    {
        try
        {
            ResponseData response = await AuthService.Token();
            string jsonResponse = response.JsonResponse;

            List<UserData> userData = JsonConvert.DeserializeObject<List<UserData>>(jsonResponse);

            if (response.StatusCode != 200)
            {
                await Navigation.PushAsync(new MainPage());
            }

            return userData;
        }
        catch (Exception)
        {
            await DisplayAlert("Error", "An error has occured while performing your request. Please, try again later.", "Okay");
            await Navigation.PushAsync(new MainPage());
            return null;
        }

    }

    private void SearchBtn_Clicked(object sender, EventArgs e)
    {
        SearchValue = SearchValue_Entry.Text;
    }

    private void OnSearchTypeSelected(object sender, EventArgs e)
    {
        SelectedSearchType = ((Picker)sender).SelectedIndex;
    }
}