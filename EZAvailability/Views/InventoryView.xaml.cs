using EZAvailability.Data;
using EZAvailability.Services;
using EZAvailability.ViewModel.Base;
using Newtonsoft.Json;
using System.Security.Cryptography.X509Certificates;
namespace EZAvailability.Views;

public partial class InventoryView : ContentPage
{

    private BaseViewModel baseViewModel = new BaseViewModel();
    public List<ProductData> Products { get; set; }

    public InventoryView()
	{
		InitializeComponent();
        this.BindingContext = baseViewModel;
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
        List<ProductData> productInfo = await GetProducts();
        Products = productInfo;
        BindingContext = this;
    }

    private async Task<List<ProductData>> GetProducts()
    {
        try
        {
            ResponseData response = await ProductService.GetProducts();
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