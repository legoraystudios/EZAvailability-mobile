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
        barcodeReader.IsDetecting = true;
    }

    private async void CameraBarcodeReaderView_BarcodesDetected(object sender, ZXing.Net.Maui.BarcodeDetectionEventArgs e)
    {

        barcodeReader.IsDetecting = false;

        Dispatcher.Dispatch(async () =>
        {
            var barcodeResult = e.Results[0].Value;
            var productUpc = long.Parse(barcodeResult);

            await Navigation.PushAsync(new ProductView(productUpc));
        });
    }
}