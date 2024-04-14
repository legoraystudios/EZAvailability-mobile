using EZAvailability.Model;
using EZAvailability.Services;
using EZAvailability.Utilities;
using EZAvailability.ViewModel.Base;
using Newtonsoft.Json;
using System.Buffers;
using System.Reflection;

namespace EZAvailability.Views;

public partial class ProductView : ContentPage
{
    private BaseViewModel baseViewModel = new BaseViewModel();
    private SnackBars snackBar = new SnackBars();
    private long productUpc;

    // Product Lists
    List<ProductModel> productInfo;
    public List<ProductModel> Product { get; set; }

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
        await CheckIfLogin();
    }

    private async Task LoadProduct()
    {
        baseViewModel.isLoading = true;
        productInfo = await GetProduct();
        Product = productInfo;
        this.BindingContext = this;
        baseViewModel.isLoading = false;
    }

    private async Task<List<ProductModel>> GetProduct()
    {
        try
        {

            ResponseModel response = await ProductService.GetProductsByUpc(productUpc);
            string jsonResponse = response.JsonResponse;

            List<ProductModel> productData = JsonConvert.DeserializeObject<List<ProductModel>>(jsonResponse);

            if (response.StatusCode != 200)
            {
                await Navigation.PushAsync(new MainPage());
            }

            if (productData.Count == 0)
            {
                await Shell.Current.GoToAsync("//DashboardView");
                snackBar.Snackbar_ScansErrorProd04(Navigation);
            }

            return productData;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "An error has occured while loading the product. Please, try again later." + ex, "Okay");
            return null;
        }

    }

    private async Task<List<UserModel>> CheckIfLogin()
    {
        try
        {
            ResponseModel response = await AuthService.Token();
            string jsonResponse = response.JsonResponse;

            List<UserModel> userData = JsonConvert.DeserializeObject<List<UserModel>>(jsonResponse);

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

    private async void EditProductBtn_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new EditProductView(productUpc));
    }
}