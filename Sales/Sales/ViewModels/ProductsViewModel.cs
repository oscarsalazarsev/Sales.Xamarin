using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Sales.Common.Models;
using Sales.Helpers;
using Sales.Services;
using Xamarin.Forms;

namespace Sales.ViewModels
{
    public class ProductsViewModel : BaseViewModel
    {
        #region Attributes

        private ApiServices apiService;
        
        private bool isRefreshing;

        #endregion

        #region Properties
        private ObservableCollection<Product> products;

        public ObservableCollection<Product> Products
        {
            get { return this.products; }
            set { this.SetValue(ref this.products, value); }
        }

        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { this.SetValue(ref this.isRefreshing, value); }
        }
        #endregion

        #region Constructors
        public ProductsViewModel()
        {
            intance = this;
            this.apiService = new ApiServices();
            this.LoadProducts();
        }
        #endregion

        #region Singleton

        private static ProductsViewModel intance;

        public static ProductsViewModel GetIntance()
        {
            if (intance == null)
            {
                return new ProductsViewModel();
            }

            return intance;
        }
        #endregion

        #region Methods

        private async void LoadProducts()
        {
            this.IsRefreshing = true;
            var checkConnection = await this.apiService.CheckConnection();
            if (!checkConnection.IsSuccess)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert(Languages.Error, checkConnection.Message, Languages.Accept);
                return;
            }
            var url = Application.Current.Resources["UrlAPI"].ToString();
            var prefix = Application.Current.Resources["UrlPrefix"].ToString();
            var controller = Application.Current.Resources["UrlProductsController"].ToString();
            var response = await this.apiService.GetList<Product>(url, prefix, controller);
            if (!response.IsSuccess)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept);
                return;
            }

            var list = (List<Product>)response.Result;
            this.Products = new ObservableCollection<Product>(list);
            this.IsRefreshing = false;
        }
        #endregion

        #region Commands
        public ICommand RefreshCommand
        {
            get { return new RelayCommand(LoadProducts); }
        }
        #endregion






    }
}

