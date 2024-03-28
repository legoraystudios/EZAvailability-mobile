using EZAvailability.Data;
using EZAvailability.Services;
using EZAvailability.ViewModel.Base;
using Newtonsoft.Json;
using System.Buffers;

namespace EZAvailability.Views;

public partial class ProductView : ContentPage
{
    private BaseViewModel baseViewModel = new BaseViewModel();
    private long productUpc;

    // Product Lists
    List<ProductData> productInfo;
    public List<ProductData> Product { get; set; }

    public ProductView(long productUpc)
	{
        this.productUpc = productUpc;

		InitializeComponent();
        LoadingModule.BindingContext = baseViewModel;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        baseViewModel.isLoading = true;
        await LoadUser();
        await LoadProduct();
        baseViewModel.isLoading = false;
    }

    private async Task LoadUser()
    {
        List<UserData> userInfo = await CheckIfLogin();
        BindingContext = userInfo;
    }

    private async Task LoadProduct()
    {
        baseViewModel.isLoading = true;
        productInfo = await GetProduct();
        Product = productInfo;
        BindingContext = this;
        baseViewModel.isLoading = false;
    }

    private async Task<List<ProductData>> GetProduct()
    {
        try
        {

            ResponseData response = await ProductService.GetProductsByUpc(productUpc);
            string jsonResponse = response.JsonResponse;

            List<ProductData> productData = JsonConvert.DeserializeObject<List<ProductData>>(jsonResponse);

            if (response.StatusCode != 200)
            {
                await Navigation.PushAsync(new MainPage());
            }

            if (productData.Count == 0)
            {
                await Navigation.PushAsync(new DashboardView());
                await DisplayAlert("Item not Found", "The item that you requested was not found in our records.", "Okay");
            }

            return productData;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "An error has occured while loading the product. Please, try again later." + ex, "Okay");
            return null;
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
}