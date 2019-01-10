using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Sales.Common.Models;
using Sales.Services;
using Xamarin.Forms;

namespace Sales.ViewModels
{
    public class ProductsViewModel : BaseViewModel
    {
        private ApiServices apiService;
        
        private ObservableCollection<Product> products;

        public ObservableCollection<Product> Products
        {
            get { return this.products;}
            set { this.SetValue(ref this.products, value); }
        }

        public ProductsViewModel()
        {
            this.apiService = new ApiServices();
            this.LoadProducts();
        }

        private async void LoadProducts()
        {
            var response = await this.apiService.GetList<Product>("https://salesapisevices.azurewebsites.net", "/api", "/Products");
            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Accept");
                return;
            }

            var list = (List<Product>) response.Result;
            this.Products = new ObservableCollection<Product>(list);
        }
    }
}
