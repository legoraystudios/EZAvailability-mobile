using EZAvailability.ViewModel;

namespace EZAvailability
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            ConnectionViewModel.GetConnection();
            MainPage = new AppShell();
        }

    }
}
