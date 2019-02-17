using System;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Sales.Helpers;
using Sales.Services;
using Sales.Views;
using Xamarin.Forms;

namespace Sales.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        #region Attributes

        private ApiServices apiService;

        private bool isRunning;

        private bool isEnabled;

        #endregion

        #region Properties

        public string Email { get; set; }

        public string Password { get; set; }

        public bool IsRemembered { get; set; }

        public bool IsRunning
        {
            get { return this.isRunning; }
            set { this.SetValue(ref this.isRunning, value); }
        }

        public bool IsEnabled
        {
            get { return this.isEnabled; }
            set { this.SetValue(ref this.isEnabled, value); }
        }

        #endregion

        #region Constructors

        public LoginViewModel()
        {
            this.apiService = new ApiServices();
            this.IsEnabled = true;
            this.IsRemembered = true;
            this.IsRunning = false;
        }
        #endregion

        #region Methods

        #endregion

        #region Commands

        public ICommand LoginCommand
        {
            get
            {
                return new RelayCommand(Login);
            }
        }

        private async void Login()
        {
            if (String.IsNullOrEmpty(this.Email))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.msgEmailValidation,
                    Languages.Accept);

                return;
            }

            if (String.IsNullOrEmpty(this.Password))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.msgPasswordValidation,
                    Languages.Accept);

                return;
            }

            this.isRunning = true;
            this.IsEnabled = false;

            var checkConnection = await this.apiService.CheckConnection();
            if (!checkConnection.IsSuccess)
            {
                this.isRunning = false;
                this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(Languages.Error, checkConnection.Message, Languages.Accept);
                return;
            }
            var url = Application.Current.Resources["UrlAPI"].ToString();
            var token = await this.apiService.GetToken(url, this.Email, this.Password);

            if (token == null || String.IsNullOrEmpty(token.AccessToken))
            {
                this.isRunning = false;
                this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(Languages.Error, Languages.msgSomethingWrong, Languages.Accept);
                return;
            }

            Settings.TokenType = token.TokenType;
            Settings.AccessToken = token.AccessToken;
            Settings.Issued = token.Issued;
            Settings.Expires = token.Expires;
            Settings.IsRemembered = this.IsRemembered;

            await Application.Current.MainPage.DisplayAlert("Ok", "Fuck yeahh!!", Languages.Accept);

            MainViewModel.GetIntance().Products = new ProductsViewModel();
            Application.Current.MainPage = new MasterPage();
            //Application.Current.MainPage = new ProductsPage();
            this.isRunning = false;
            this.IsEnabled = true;

        }

        public ICommand RegisterCommand
        {
            get
            {
                return new RelayCommand(Register);
            }
        }

        private async void Register()
        {
            MainViewModel.GetIntance().Register = new RegisterViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new RegisterPage());
        }
        #endregion
    }
}
