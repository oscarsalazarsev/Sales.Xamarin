using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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

        private ObservableCollection<ProductsItemViewModel> products;

        private string filter;
        #endregion

        #region Properties
        public ObservableCollection<ProductsItemViewModel> Products
        {
            get { return this.products; }
            set { this.SetValue(ref this.products, value); }
        }

        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { this.SetValue(ref this.isRefreshing, value); }
        }

        public List<Product> MyProducts { get; set; }

        public string Filter
        {
            get { return this.filter; }
            set
            {
                this.filter = value;
                RefreshList();
            }
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
            var response = await this.apiService.GetList<Product>(url, prefix, controller, Settings.TokenType, Settings.AccessToken);
            if (!response.IsSuccess)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept);
                return;
            }

            this.MyProducts = (List<Product>)response.Result;
            this.RefreshList();
            this.IsRefreshing = false;
        }

        public void RefreshList()
        {
            if (String.IsNullOrEmpty(this.Filter))
            {
                var listProductsItemsViewModel = this.MyProducts.Select(p => new ProductsItemViewModel
                {
                    Description = p.Description,
                    ImageArray = p.ImageArray,
                    ImagePath = p.ImagePath,
                    IsAvailable = p.IsAvailable,
                    Price = p.Price,
                    ProductId = p.ProductId,
                    PublshOn = p.PublshOn,
                    Remarks = p.Remarks,
                });

                this.Products = new ObservableCollection<ProductsItemViewModel>(listProductsItemsViewModel.OrderBy(p => p.Description));
            }
            else
            {
                var listProductsItemsViewModel = this.MyProducts.Select(p => new ProductsItemViewModel
                {
                    Description = p.Description,
                    ImageArray = p.ImageArray,
                    ImagePath = p.ImagePath,
                    IsAvailable = p.IsAvailable,
                    Price = p.Price,
                    ProductId = p.ProductId,
                    PublshOn = p.PublshOn,
                    Remarks = p.Remarks,
                }).Where(p => p.Description.ToLower().Contains(this.Filter.ToLower()));

                this.Products = new ObservableCollection<ProductsItemViewModel>(listProductsItemsViewModel.OrderBy(p => p.Description));               
            }

        }
        #endregion

        #region Commands
        public ICommand SearchCommand
        {
            get { return new RelayCommand(RefreshList); }
        }

        public ICommand RefreshCommand
        {
            get { return new RelayCommand(LoadProducts); }
        }
        #endregion
        
    }
}

