using Sales.Views;
using System;
using Sales.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Sales.Helpers;
using Newtonsoft.Json;
using Sales.Common.Models;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Sales
{
    public partial class App : Application
    {
        public static NavigationPage Navigator { get; internal set; }

        public App()
        {
            InitializeComponent();

            var mainViewModel = MainViewModel.GetInstance();

            if (Settings.IsRemembered && 
                !String.IsNullOrEmpty(Settings.AccessToken) &&
                Settings.Expires != DateTime.MinValue && 
                Settings.Expires >= DateTime.UtcNow)
            {
                if (!string.IsNullOrEmpty(Settings.UserASP))
                {
                    mainViewModel.UserASP = JsonConvert.DeserializeObject<MyUserASP>(Settings.UserASP);
                }

                mainViewModel.Products = new ProductsViewModel();
                this.MainPage = new MasterPage();
                //MainPage = new NavigationPage(new ProductsPage());
            }
            else
            {
                mainViewModel.Login = new LoginViewModel();
                this.MainPage = new NavigationPage(new LoginPage());            
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
