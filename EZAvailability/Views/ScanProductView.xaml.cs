using EZAvailability.Model;
using EZAvailability.Services;
using EZAvailability.Utilities;
using Newtonsoft.Json;
using Plugin.Maui.Audio;
using ZXing.Net.Maui;
using ZXing.Net.Maui.Controls;

namespace EZAvailability.Views;

public partial class ScanProductView : ContentPage
{
    private SnackBars snackBar = new SnackBars();

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

            if (long.TryParse(barcodeResult, out long productUpc))
            {
                await Navigation.PushAsync(new ProductView(productUpc));
            } else
            {
                snackBar.Snackbar_InvalidBarcode();
            }

        });
    }

    private async Task LoadUser()
    {
        List<UserModel> userInfo = await CheckIfLogin();
        BindingContext = userInfo;
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

    private async void AdvancedSearchBtn_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new InventoryView());
    }
}