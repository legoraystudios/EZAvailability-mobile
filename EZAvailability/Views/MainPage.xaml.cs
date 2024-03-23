using EZAvailability.ViewModel;
using EZAvailability.ViewModel.Base;

using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Font = Microsoft.Maui.Font;
using EZAvailability.Data;
using EZAvailability.Services;

namespace EZAvailability
{
    public partial class MainPage : ContentPage
    {
        private BaseViewModel baseViewModel = new BaseViewModel();

        public MainPage()
        {
            ConnectionViewModel.GetConnection();
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);
            CheckIfLogin();
            this.BindingContext = baseViewModel;

        }

        [Obsolete]
        protected override void OnAppearing()
        {
            base.OnAppearing();
            Device.BeginInvokeOnMainThread(async () =>
            {
                string apiUrl = ConnectionViewModel.ConnectionUrl;
                if (apiUrl == null || apiUrl == "Not Found")
                {
                    await ConnectionShowPopUp();
                }
            });
        }

        private async Task ConnectionShowPopUp()
        {
            string userInput = await DisplayPromptAsync("Set-Up New Connection", "Enter your absolute EZAvailabilty " +
                "API Connection to access to your inventory. Example: (https://yourdomain.com/ezavailability).", "Add Connection");

            if (userInput != null)
            {
                int response = await ConnectionViewModel.AddConnection(userInput);
                bool alertResult = false;

                baseViewModel.isLoading = true;

                if (response != 0)
                {
                    switch (response)
                    {
                        case -1:
                            alertResult = await DisplayAlert("Unable to validate connection", "Error connecting to your EZAvailability API Connection.", "Retry", "Cancel");
                            baseViewModel.isLoading = false;
                            break;
                        case -2:
                            alertResult = await DisplayAlert("Error", "An error has occured while establishing connection to your EZAvailability API Connection.", "Retry", "Cancel");
                            baseViewModel.isLoading = false;
                            break;
                    }

                    if (alertResult)
                    {
                        await ConnectionShowPopUp();
                        baseViewModel.isLoading = false;
                    }
                }
            }
        }

        private async void CheckIfLogin()
        {
            try
            {
                ResponseData response = await AuthService.Token();

                if (response.StatusCode == 200)
                {
                    await Navigation.PushAsync(new Views.DashboardView());
                }
            } catch 
            {
               await DisplayAlert("Error", "An error has occured while performing your request. Please, check your network and API connection and try again.", "Okay");
            }
        }

        private void BtnEditConn_Clicked(object sender, EventArgs e)
        {
            ConnectionShowPopUp();
        }

        private void BtnForgotPasswd_Clicked(object sender, EventArgs e)
        {

        }

        private async void BtnSignIn_Clicked(object sender, EventArgs e)
        {
            try
            {
                baseViewModel.isLoading = true;

                int response = await AuthService.Login(EmailEntry.Text, PasswordEntry.Text);

                if (response == 200)
                {

                    var snackbarOptions = new SnackbarOptions
                    {
                        BackgroundColor = Color.FromHex("#198754"),
                        Font = Font.SystemFontOfSize(15),
                        TextColor = Color.FromHex("#FFFFFF")
                    };

                    var snackbar = Snackbar.Make("You're Logged in! Redirecting...", null, "", null, snackbarOptions);
                    snackbar.Show();
                    //Application.Current.MainPage = new NavigationPage(new Views.DashboardView());
                    await Shell.Current.GoToAsync("//DashboardView");

                }
                else
                {
                    var snackbarOptions = new SnackbarOptions
                    {
                        BackgroundColor = Color.FromHex("#DC3545"),
                        Font = Font.SystemFontOfSize(15),
                        TextColor = Color.FromHex("#FFFFFF")
                    };

                    var snackbar = Snackbar.Make("ERROR | Email or password are incorrect.", null, "", null, snackbarOptions);
                    snackbar.Show();
                }

                baseViewModel.isLoading = false;
            } catch (Exception)
            {
                await DisplayAlert("Error", "An error has occured while performing your request. Please, check your network and API connection and try again." , "Okay");
                baseViewModel.isLoading = false;
            }

        }
    }

}