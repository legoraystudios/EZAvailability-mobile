using EZAvailability.Services;
using EZAvailability.ViewModel.Base;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.Json.Serialization;
using EZAvailability.Model;

namespace EZAvailability.Views;

public partial class DashboardView : ContentPage
{

    private BaseViewModel baseViewModel = new BaseViewModel();
    public DashboardView()
	{
        InitializeComponent();

        this.BindingContext = baseViewModel;
        NavigationPage.SetHasNavigationBar(this, true);
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        baseViewModel.isLoading = true;
        await LoadUser();
        await LoadMetrics();
        baseViewModel.isLoading = false;
    }

    private async Task LoadUser()
    {
        List<UserModel> userInfo = await CheckIfLogin();
        BindingContext = userInfo;
    }

    private async Task LoadMetrics()
    {
        List<MetricModel> metricInfo = await GetMetrics();
        DashboardMetrics.BindingContext = metricInfo;
    }

    private async Task<List<MetricModel>> GetMetrics()
    {
        try
        {
            ResponseModel response = await MetricService.GetMetrics();
            string jsonResponse = response.JsonResponse;

            List<MetricModel> metricData = JsonConvert.DeserializeObject<List<MetricModel>>(jsonResponse);

            if (response.StatusCode != 200)
            {
                await Navigation.PushAsync(new MainPage());
            }

            return metricData;
        } catch (Exception)
        {
            await DisplayAlert("Error", "An error has occured while loading the metrics. Please, try again later.", "Okay");
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

    private async void ScanInBtn_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ScanInView());
    }

    private async void ScanOutBtn_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ScanOutView());
    }

    private async void BtnInventory_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new InventoryView());
    }

    private async void BtnProductInfo_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ScanProductView());
    }
}