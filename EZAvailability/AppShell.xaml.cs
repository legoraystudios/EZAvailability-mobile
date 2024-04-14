using CommunityToolkit.Maui.Alerts;
using EZAvailability.Model;
using EZAvailability.Services;
using EZAvailability.Utilities;
using System.Windows.Input;

namespace EZAvailability
{
    public partial class AppShell : Shell
    {
        public ICommand SignOutCommand { get; }
        private SnackBars snackBar = new SnackBars();

        public AppShell()
        {
            InitializeComponent();
        }
        private async void OnScanItemTapped(object sender, EventArgs e)
        {
            FlyoutIsPresented = false;
            await Navigation.PushAsync(new Views.ScanProductView());
        }

        private void OnShoppingListTapped(object sender, EventArgs e)
        {

        }
      
        private async void OnInventoryTapped(object sender, EventArgs e)
        {
            FlyoutIsPresented = false;
            await Navigation.PushAsync(new Views.InventoryView());
        }

        private async void BtnSignOut_Clicked(object sender, EventArgs e)
        {
            try
            {
                int response = await AuthService.Logout();

                if (response == 200)
                {
                    FlyoutIsPresented = false;
                    snackBar.Snackbar_SuccessLogout();
                    await Navigation.PushAsync(new MainPage());
                }
            }
            catch (Exception)
            {
                await DisplayAlert("Error", "An error has occured while signing you out. Please, try again later.", "Okay");
            }
        }
    }
}
