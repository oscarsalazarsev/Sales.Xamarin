using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Sales.Helpers;
using Xamarin.Forms;

namespace Sales.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        #region Attributes

        private bool isRunning;

        private bool isEnabled;

        #endregion

        #region Properties

        public string Email { get; set; }

        public string Password { get; set; }

        public bool IsRememberme { get; set; }

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
            this.IsEnabled = true;
            this.IsRememberme = true;
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
        }
        #endregion
    }
}
