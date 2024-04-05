using EZAvailability.Data;
using EZAvailability.Services;
using EZAvailability.ViewModel.Base;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.Json.Serialization;
using UserData = EZAvailability.Data.UserData;

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
        List<UserData> userInfo = await CheckIfLogin();
        BindingContext = userInfo;
    }

    private async Task LoadMetrics()
    {
        List<MetricData> metricInfo = await GetMetrics();
        DashboardMetrics.BindingContext = metricInfo;
    }

    private async Task<List<MetricData>> GetMetrics()
    {
        try
        {
            ResponseData response = await MetricService.GetMetrics();
            string jsonResponse = response.JsonResponse;

            List<MetricData> metricData = JsonConvert.DeserializeObject<List<MetricData>>(jsonResponse);

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
        } catch (Exception)
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