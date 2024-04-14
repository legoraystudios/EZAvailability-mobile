using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using EZAvailability.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZAvailability.Utilities
{
    public class SnackBars
    {

        public void Snackbar_ScanInSuccess(long productUpc, int qty)
        {
            var snackbarOptions = new SnackbarOptions
            {
                BackgroundColor = Color.FromHex("#198754"),
                Font = Microsoft.Maui.Font.SystemFontOfSize(15),
                TextColor = Color.FromHex("#FFFFFF")
            };

            var snackbar = Snackbar.Make("Added +" + qty + " product(s) to UPC " + productUpc, null, "", null, snackbarOptions);
            snackbar.Show();
        }
        public void Snackbar_ScanOutSuccess(long productUpc, int qty)
        {
            var snackbarOptions = new SnackbarOptions
            {
                BackgroundColor = Color.FromHex("#198754"),
                Font = Microsoft.Maui.Font.SystemFontOfSize(15),
                TextColor = Color.FromHex("#FFFFFF")
            };

            var snackbar = Snackbar.Make("Removed -" + qty + " product(s) to UPC " + productUpc, null, "", null, snackbarOptions);
            snackbar.Show();
        }

        public void Snackbar_ScansErrorProd01()
        {
            var snackbarOptions = new SnackbarOptions
            {
                BackgroundColor = Color.FromHex("#DC3545"),
                Font = Microsoft.Maui.Font.SystemFontOfSize(15),
                TextColor = Color.FromHex("#FFFFFF")
            };

            var snackbar = Snackbar.Make("Product Name already exist in our records.", null, "", null, snackbarOptions);
            snackbar.Show();
        }
        public void Snackbar_ScansErrorProd02()
        {
            var snackbarOptions = new SnackbarOptions
            {
                BackgroundColor = Color.FromHex("#DC3545"),
                Font = Microsoft.Maui.Font.SystemFontOfSize(15),
                TextColor = Color.FromHex("#FFFFFF")
            };

            var snackbar = Snackbar.Make("Category doesn't exist in our records.", null, "", null, snackbarOptions);
            snackbar.Show();
        }
        public void Snackbar_ScansErrorProd03()
        {
            var snackbarOptions = new SnackbarOptions
            {
                BackgroundColor = Color.FromHex("#DC3545"),
                Font = Microsoft.Maui.Font.SystemFontOfSize(15),
                TextColor = Color.FromHex("#FFFFFF")
            };

            var snackbar = Snackbar.Make("Product UPC already exist in our records.", null, "", null, snackbarOptions);
            snackbar.Show();
        }

        public void Snackbar_ScansErrorProd04(INavigation navigation)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            var snackbarOptions = new SnackbarOptions
            {
                BackgroundColor = Color.FromHex("#DC3545"),
                Font = Microsoft.Maui.Font.SystemFontOfSize(15),
                TextColor = Color.FromHex("#FFFFFF"),
                ActionButtonTextColor = Color.FromHex("#FFFFFF")
            };

            TimeSpan duration = TimeSpan.FromSeconds(5);
            Action action = async () => await navigation.PushAsync(new DashboardView());
            var snackbar = Snackbar.Make("Product not found in our records.", action, "CREATE ITEM", duration, snackbarOptions);
            snackbar.Show(cancellationTokenSource.Token);
        }

        public void Snackbar_ScansErrorProd05()
        {
            var snackbarOptions = new SnackbarOptions
            {
                BackgroundColor = Color.FromHex("#DC3545"),
                Font = Microsoft.Maui.Font.SystemFontOfSize(15),
                TextColor = Color.FromHex("#FFFFFF")
            };

            var snackbar = Snackbar.Make("Desired quantity exceeds the quantity on inventory.", null, "", null, snackbarOptions);
            snackbar.Show();
        }

        public void Snackbar_SuccessProductModified()
        {
            var snackbarOptions = new SnackbarOptions
            {
                BackgroundColor = Color.FromHex("#198754"),
                Font = Microsoft.Maui.Font.SystemFontOfSize(15),
                TextColor = Color.FromHex("#FFFFFF")
            };

            var snackbar = Snackbar.Make("Product modifies successfully.", null, "", null, snackbarOptions);
            snackbar.Show();
        }

        public void Snackbar_InvalidBarcode()
        {
            var snackbarOptions = new SnackbarOptions
            {
                BackgroundColor = Color.FromHex("#DC3545"),
                Font = Microsoft.Maui.Font.SystemFontOfSize(15),
                TextColor = Color.FromHex("#FFFFFF"),
                ActionButtonTextColor = Color.FromHex("#FFFFFF")
            };

            var snackbar = Snackbar.Make("You scanned an invalid barcode format.", null, "DISMISS", null, snackbarOptions);
            snackbar.Show();
        }

        public void Snackbar_Error()
        {
            var snackbarOptions = new SnackbarOptions
            {
                BackgroundColor = Color.FromHex("#DC3545"),
                Font = Microsoft.Maui.Font.SystemFontOfSize(15),
                TextColor = Color.FromHex("#FFFFFF"),
                ActionButtonTextColor = Color.FromHex("#FFFFFF")
            };

            var snackbar = Snackbar.Make("An error has ocurred while performing your request. Please try again later.", null, "DISMISS", null, snackbarOptions);
            snackbar.Show();
        }

        public void Snackbar_SuccessLogin()
        {
            var snackbarOptions = new SnackbarOptions
            {
                BackgroundColor = Color.FromHex("#198754"),
                Font = Microsoft.Maui.Font.SystemFontOfSize(15),
                TextColor = Color.FromHex("#FFFFFF")
            };

            var snackbar = Snackbar.Make("You're Logged in! Redirecting...", null, "", null, snackbarOptions);
            snackbar.Show();
        }

        public void Snackbar_SuccessLogout()
        {
            var snackbarOptions = new SnackbarOptions
            {
                BackgroundColor = Color.FromHex("#198754"),
                Font = Microsoft.Maui.Font.SystemFontOfSize(15),
                TextColor = Color.FromHex("#FFFFFF")
            };

            var snackbar = Snackbar.Make("Successfully logged out!", null, "", null, snackbarOptions);
            snackbar.Show();
        }

        public void Snackbar_ErrorLogin()
        {
            var snackbarOptions = new SnackbarOptions
            {
                BackgroundColor = Color.FromHex("#DC3545"),
                Font = Microsoft.Maui.Font.SystemFontOfSize(15),
                TextColor = Color.FromHex("#FFFFFF")
            };

            var snackbar = Snackbar.Make("ERROR | Email or password are incorrect.", null, "", null, snackbarOptions);
            snackbar.Show();
        }

    }
}
