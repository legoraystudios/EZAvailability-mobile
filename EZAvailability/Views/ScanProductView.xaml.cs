using EZAvailability.Data;
using EZAvailability.Services;
using Newtonsoft.Json;
using Plugin.Maui.Audio;
using ZXing.Net.Maui;
using ZXing.Net.Maui.Controls;

namespace EZAvailability.Views;

public partial class ScanProductView : ContentPage
{
	public ScanProductView()
	{
		InitializeComponent();
        barcodeReader.IsDetecting = true;

        barcodeReader.Options = new BarcodeReaderOptions
        {
            Formats = BarcodeFormats.OneDimensional,
            AutoRotate = true,
            Multiple = false
        };
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadUser();
        barcodeReader.IsDetecting = true;
    }

    private async void CameraBarcodeReaderView_BarcodesDetected(object sender, ZXing.Net.Maui.BarcodeDetectionEventArgs e)
    {

        var scannerSound = AudioManager.Current.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("scansound.mp3"));
        scannerSound.Play();

        barcodeReader.IsDetecting = false;

        Dispatcher.Dispatch(async () =>
        {
            var barcodeResult = e.Results[0].Value;
            var productUpc = long.Parse(barcodeResult);

            await Navigation.PushAsync(new ProductView(productUpc));
        });
    }

    private async Task LoadUser()
    {
        List<UserData> userInfo = await CheckIfLogin();
        BindingContext = userInfo;
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

    private async void AdvancedSearchBtn_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new InventoryView());
    }
}