using CommunityToolkit.Maui.Alerts;
using EZAvailability.Model;
using EZAvailability.Services;
using EZAvailability.Utilities;
using EZAvailability.ViewModel.Base;
using Newtonsoft.Json;

namespace EZAvailability.Views;

public partial class EditProductView : ContentPage
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
    private long categoryId;


    // Product Lists
    List<ProductModel> productInfo;
    public List<ProductModel> Product { get; set; }


    public EditProductView(long productUpc)
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
        await LoadCategoryPickerAsync();
        baseViewModel.isLoading = false;
    }

    // Load logged user.
    private async Task LoadUser()
    {
        await CheckIfLogin();
    }

    // Load selected/scanned product.
    private async Task LoadProduct()
    {
        baseViewModel.isLoading = true;
        productInfo = await GetProduct();
        Product = productInfo;
        this.BindingContext = this;
        baseViewModel.isLoading = false;
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

    // Get selected/scanned product
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

            productId = productData[0].product_id;
            categoryId = productData[0].category_id;

            return productData;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "An error has occured while loading the product. Please, try again later.", "Okay");
            return null;
        }

    }

    // Update product (This method will be executed when the user
    // taps "Save Changes" on the UI Button.
    private async Task UpdateProduct()
    {
        try
        {

            ResponseModel response = await ProductService.UpdateProduct(productId, productName, productDesc, productQty,
                productUpc, lowStockAlert, categoryId);
            string jsonResponse = response.JsonResponse;

            ErrorModel errorData = JsonConvert.DeserializeObject<ErrorModel>(jsonResponse);


            if (response.StatusCode == 200) {
                await Navigation.PopAsync();
                snackBar.Snackbar_SuccessProductModified();
            } else if (errorData.errors.errCode == "Prod01")
            {
                snackBar.Snackbar_ScansErrorProd01();
            } else if (errorData.errors.errCode == "Prod03")
            {
                snackBar.Snackbar_ScansErrorProd03();
            } else if (errorData.errors.errCode == "Prod02")
            {
                snackBar.Snackbar_ScansErrorProd02();
            } else
            {
                snackBar.Snackbar_Error();
            }

        }
        catch
        {
            await DisplayAlert("Error", "An error has occured while loading the product. Please, try again later.", "Okay");
        }
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

    // Cancel Button Action
    private void BtnCancel_Clicked(object sender, EventArgs e)
    {
		Navigation.PopAsync();
    }

    // Save Changes Button Action
    private async void BtnSaveChanges_Clicked(object sender, EventArgs e)
    {
        productName = lblProductName.Text;
        productDesc = lblProductDesc.Text; 

        if (int.TryParse(lblProductQty.Text.ToString(), out int parsedQty))
        {
            productQty = parsedQty;
        }
        if (int.TryParse(lblProductUpc.Text.ToString(), out int parsedUpc))
        {
            productUpc = parsedUpc;
        }
        if (int.TryParse(lblLowStockAlert.Text.ToString(), out int parsedLowStockAlert))
        {
            lowStockAlert = parsedLowStockAlert;
        }

        await UpdateProduct();
    }
}