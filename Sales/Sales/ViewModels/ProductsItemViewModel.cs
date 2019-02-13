using System;
using System.Linq;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Sales.Common.Models;
using Sales.Helpers;
using Sales.Services;
using Sales.Views;
using Xamarin.Forms;

namespace Sales.ViewModels
{
    public class ProductsItemViewModel : Product
    {
        #region Attributes

        private ApiServices apiService;

        #endregion

        #region Constructors

        public ProductsItemViewModel()
        {
            this.apiService = new ApiServices();
        }
        #endregion

        #region Commands
        //public ICommand RefreshCommand
        //{
        //    get { return new RelayCommand(LoadProducts); }
        //}

        public ICommand DeleteProductCommand
        {
            get { return new RelayCommand(DeleteProducts); }
        }

        public ICommand EditProductCommand
        {
            get { return new RelayCommand(EditProducts); }
        }

        private async void DeleteProducts()
        {
            var answer = await Application.Current.MainPage.DisplayAlert(
                Languages.msgConfirm,
                Languages.msgDeleteConfirmation, 
                Languages.btnYes, 
                Languages.btnNo);
            if (!answer)
            {
                return;
            }

            var checkConnection = await this.apiService.CheckConnection();
            if (!checkConnection.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(Languages.Error, checkConnection.Message, Languages.Accept);
                return;
            }
            var url = Application.Current.Resources["UrlAPI"].ToString();
            var prefix = Application.Current.Resources["UrlPrefix"].ToString();
            var controller = Application.Current.Resources["UrlProductsController"].ToString();
            var response = await this.apiService.Delete(url, prefix, controller, this.ProductId);
            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept);
                return;
            }

            var productsViewModel = ProductsViewModel.GetIntance();
            var deletedProduct = productsViewModel.MyProducts.Where(p => p.ProductId == this.ProductId).FirstOrDefault();
            if (deletedProduct != null)
            {
                productsViewModel.MyProducts.Remove(deletedProduct);
            }
            productsViewModel.RefreshList();
        }

        private async void EditProducts()
        {
            MainViewModel.GetIntance().EditProduct = new EditProductViewModel(this);
            await Application.Current.MainPage.Navigation.PushAsync(new EditProductPage());
        }


        #endregion
    }
}
