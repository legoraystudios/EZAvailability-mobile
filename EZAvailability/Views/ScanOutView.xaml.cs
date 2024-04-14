using EZAvailability.Model;
using EZAvailability.Services;
using EZAvailability.Utilities;
using Newtonsoft.Json;
using Plugin.Maui.Audio;
using ZXing.Net.Maui;
using ZXing.Net.Maui.Controls;

namespace EZAvailability.Views;

public partial class ScanOutView : ContentPage
{
    private SnackBars snackBar = new SnackBars();
    private int qty = 1;

    public ScanOutView()
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

                if (long.TryParse(barcodeResult, out long productUpc))
                {
                    ResponseModel response = await ScanService.ScanOut(productUpc, qty);
                    string jsonResponse = response.JsonResponse;

                    ErrorModel errorData = JsonConvert.DeserializeObject<ErrorModel>(jsonResponse);

                    if (response.StatusCode == 200)
                    {
                        snackBar.Snackbar_ScanOutSuccess(productUpc, qty);
                        var successSound = AudioManager.Current.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("success.mp3"));
                        successSound.Play();
                    }
                    else
                    {
                        // Check if the errors object is not null
                        if (errorData.errors != null)
                        {
                            if (errorData.errors.errCode == "Prod04")
                            {
                                snackBar.Snackbar_ScansErrorProd04(Navigation);
                            }
                            else if (errorData.errors.errCode == "Prod05")
                            {
                                snackBar.Snackbar_ScansErrorProd05();
                            }
                        }
                    }
                } else
                {
                    snackBar.Snackbar_InvalidBarcode();
                }

                await Task.Delay(3000);
                barcodeReader.IsDetecting = true;

            });
        }
        catch
        {
            snackBar.Snackbar_Error();
        }
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
        }
        else if (!string.IsNullOrEmpty(e.NewTextValue))
        {
            if (int.TryParse(e.NewTextValue, out int parsedQty))
            {
                qty = parsedQty;
            }
            else
            {
                ((Entry)sender).Text = e.OldTextValue;
            }
        }
    }
}