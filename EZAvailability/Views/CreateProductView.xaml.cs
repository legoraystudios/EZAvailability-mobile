using EZAvailability.Model;
using EZAvailability.Services;
using EZAvailability.Utilities;
using EZAvailability.ViewModel.Base;
using Newtonsoft.Json;

namespace EZAvailability.Views;

public partial class CreateProductView : ContentPage
{

    private BaseViewModel baseViewModel = new BaseViewModel();
    private CategoryPicker categoryPicker = new CategoryPicker();
    private SnackBars snackBar = new SnackBars();

    // Product Data
    private long productId;
    private string productName;
    private string productDesc;
    private int productQty;
    private long productUpc;
    private int lowStockAlert;
    private long categoryId = 0;

    public CreateProductView()
	{
		InitializeComponent();
        LoadingModule.BindingContext = baseViewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        baseViewModel.isLoading = true;
        await LoadUser();
        await LoadCategoryPickerAsync();
        baseViewModel.isLoading = false;
    }

    // Load logged user.
    private async Task LoadUser()
    {
        await CheckIfLogin();
    }

    // Check if the user is logged in
    private async Task<List<UserModel>> CheckIfLogin()
    {
        try
        {
            ResponseModel response = await AuthService.Token();
            string jsonResponse = response.JsonResponse;

            List<UserModel> userData = JsonConvert.DeserializeObject<List<UserModel>>(jsonResponse);

            // If the user is NOT logged in, send the user to the Login Page
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

    // Load all the available categories for the Picker Selector.
    private async Task LoadCategoryPickerAsync()
    {
        var picker = await categoryPicker.GetCategoryPickerAsync(categoryId);
        CategoryPickerBinding.Children.Add(picker);

        // What to do if the user select a category on the picker.
        picker.SelectedIndexChanged += async (sender, args) =>
        {
            int index = ((Picker)sender).SelectedIndex;
            categoryId = await categoryPicker.GetCategoryIdByIndex(index);
        };
    }

    private async Task CreateProduct()
    {
        try
        {

            ResponseModel response = await ProductService.CreateProduct(productName, productDesc, productQty,
                productUpc, lowStockAlert, categoryId);
            string jsonResponse = response.JsonResponse;

            ErrorModel errorData = JsonConvert.DeserializeObject<ErrorModel>(jsonResponse);

            if (response.StatusCode == 200)
            {
                await Navigation.PopAsync();
                snackBar.Snackbar_SuccessProductCreated();
            }
            else if (errorData.errors.errCode == "Prod01")
            {
                snackBar.Snackbar_ScansErrorProd01();
            }
            else if (errorData.errors.errCode == "Prod03")
            {
                snackBar.Snackbar_ScansErrorProd03();
            }
            else if (errorData.errors.errCode == "Prod02")
            {
                snackBar.Snackbar_ScansErrorProd02();
            }
            else
            {
                snackBar.Snackbar_Error();
            }

        }
        catch
        {
            await DisplayAlert("Error", "An error has occured while loading the product. Please, try again later.", "Okay");
        }
    }

    private async void BtnCreateProduct_Clicked(object sender, EventArgs e)
    {
        productName = lblProductName.Text;
        productDesc = lblProductDesc.Text;

        if (int.TryParse(lblProductQty.Text, out int parsedQty))
        {
            productQty = parsedQty;
        }
        if (int.TryParse(lblProductUpc.Text, out int parsedUpc))
        {
            productUpc = parsedUpc;
        }
        if (int.TryParse(lblLowStockAlert.Text, out int parsedLowStockAlert))
        {
            lowStockAlert = parsedLowStockAlert;
        }

        await CreateProduct();
    }

    private async void BtnCancel_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}