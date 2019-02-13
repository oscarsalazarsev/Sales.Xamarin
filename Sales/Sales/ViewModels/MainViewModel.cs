using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
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
        #endregion

        #region Constructors
        public MainViewModel()
        {
            intance = this;
            this.Products = new ProductsViewModel();
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
            await Application.Current.MainPage.Navigation.PushAsync(new AddProductPage());
        }
        #endregion

        #region Singleton

        private static MainViewModel intance;

        public static MainViewModel GetIntance()
        {
            if (intance == null)
            {
                return new MainViewModel();
            }

            return intance;
        }
        #endregion

    }
}
