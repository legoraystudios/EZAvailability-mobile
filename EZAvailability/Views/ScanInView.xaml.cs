using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using EZAvailability.Data;
using EZAvailability.Services;
using Newtonsoft.Json;
using Plugin.Maui.Audio;
using ZXing.Net.Maui;
using EZAvailability.Utilities;
using ZXing.Net.Maui.Controls;

namespace EZAvailability.Views;

public partial class ScanInView : ContentPage
{

    private SnackBars snackBar = new SnackBars();
    private int qty = 1;

	public ScanInView()
	{
		InitializeComponent();
        QtyEntry.Text = qty.ToString();
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
        try
        {
            var scannerSound = AudioManager.Current.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("scansound.mp3"));
            scannerSound.Play();

            barcodeReader.IsDetecting = false;

            Dispatcher.Dispatch(async () =>
            {
                var barcodeResult = e.Results[0].Value;
                var productUpc = long.Parse(barcodeResult);

                ResponseData response = await ScanService.ScanIn(productUpc, qty);
                string jsonResponse = response.JsonResponse;

                ScanData scanData = JsonConvert.DeserializeObject<ScanData>(jsonResponse);

                if (response.StatusCode == 200)
                {
                    snackBar.Snackbar_ScanInSuccess(productUpc, qty);
                    var successSound = AudioManager.Current.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("success.mp3"));
                    successSound.Play();
                }
                else
                {
                    // Check if the errors object is not null
                    if (scanData.errors != null)
                    {
                        if (scanData.errors.errCode == "Prod04")
                        {
                            snackBar.Snackbar_ScansErrorProd04(Navigation);
                        }
                        else if (scanData.errors.errCode == "Prod05")
                        {
                            snackBar.Snackbar_ScansErrorProd05();
                        }
                    }
                }

                await Task.Delay(3000);
                barcodeReader.IsDetecting = true;

            });
        } catch
        {
            snackBar.Snackbar_Error();
        }
        
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

    private void QtyDecreaseBtn_Clicked(object sender, EventArgs e)
    {
        if (qty > 1)
        {
            qty--;
        }
        QtyEntry.Text = qty.ToString();
    }

    private void QtyIncreaseBtn_Clicked(object sender, EventArgs e)
    {
        qty++;
        QtyEntry.Text = qty.ToString();
    }

    private void Entry_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (!string.IsNullOrEmpty(e.NewTextValue) && e.NewTextValue.StartsWith("-"))
        {
            ((Entry)sender).Text = e.OldTextValue;
        }
        else if (e.NewTextValue.Equals("0"))
        {
            ((Entry)sender).Text = e.OldTextValue;
        } else if (!string.IsNullOrEmpty(e.NewTextValue))
        {
            qty = int.Parse(e.NewTextValue);
        }
    }
}