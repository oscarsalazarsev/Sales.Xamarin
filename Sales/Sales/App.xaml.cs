using Sales.Views;
using System;
using Sales.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Sales.Helpers;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Sales
{
    public partial class App : Application
    {
        public static NavigationPage Navigator { get; internal set; }

        public App()
        {
            InitializeComponent();

            if (Settings.IsRemembered && 
                !String.IsNullOrEmpty(Settings.AccessToken) &&
                Settings.Expires != DateTime.MinValue && 
                Settings.Expires >= DateTime.UtcNow)
            {
                MainViewModel.GetIntance().Products = new ProductsViewModel();
                MainPage = new MasterPage();
                //MainPage = new NavigationPage(new ProductsPage());
            }
            else
            {
                MainViewModel.GetIntance().Login = new LoginViewModel();
                MainPage = new NavigationPage(new LoginPage());            
            }
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
