using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Sales.Common.Models;
using Sales.Helpers;
using Sales.Views;
using Xamarin.Forms;

namespace Sales.ViewModels
{
    public class MainViewModel
    {
        #region Properties

        public ProductsViewModel Products { get; set; }

        public AddProductViewModel AddProduct { get; set; }

        public EditProductViewModel EditProduct { get; set; }

        public LoginViewModel Login { get; set; }

        public ObservableCollection<MenuItemViewModel> Menu { get; set; }

        public RegisterViewModel Register { get; set; }

        public MyUserASP UserASP { get; set; }

        public string UserFullName
        {
            get
            {
                if (this.UserASP != null && this.UserASP.Claims != null && this.UserASP.Claims.Count > 1)
                {
                    return $"{this.UserASP.Claims[0].ClaimValue} {this.UserASP.Claims[1].ClaimValue}";
                }

                return null;
            }
        }

        public string UserImageFullPath
        {
            get
            {
                if (this.UserASP != null && this.UserASP.Claims != null && this.UserASP.Claims.Count > 2)
                {
                    return $"https://salesapisevices.azurewebsites.net{this.UserASP.Claims[3].ClaimValue.Substring(1)}";
                }

                return null;
            }
        }

        #endregion

        #region Constructors
        public MainViewModel()
        {
            instance = this;
            this.LoadMenu();
            //this.Products = new ProductsViewModel();
        }

        #endregion

        #region Commands
        public ICommand AddProductCommand
        {
            get { return new RelayCommand(GoToAddProduct); }
        }

        private async void GoToAddProduct()
        {
            this.AddProduct = new AddProductViewModel();
            await App.Navigator.PushAsync(new AddProductPage());
            //await Application.Current.MainPage.Navigation.PushAsync(new AddProductPage());
        }
        #endregion

        #region Singleton

        private static MainViewModel instance;

        public static MainViewModel GetInstance()
        {
            if (instance == null)
            {
                return new MainViewModel();
            }

            return instance;
        }

        #endregion

        #region Methods

        private void LoadMenu()
        {
            this.Menu = new ObservableCollection<MenuItemViewModel>();

            this.Menu.Add(new MenuItemViewModel
            {
                Icon = "ic_info",
                PageName = "AboutPage",
                Title = Languages.titleAbout,
            });

            this.Menu.Add(new MenuItemViewModel
            {
                Icon = "ic_phonelink_setup",
                PageName = "SetupPage",
                Title = Languages.titleSetup,
            });

            this.Menu.Add(new MenuItemViewModel
            {
                Icon = "ic_exit_to_app",
                PageName = "LoginPage",
                Title = Languages.msgExit,
            });

        }

        #endregion

    }
}
