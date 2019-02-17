using GalaSoft.MvvmLight.Command;
using Sales.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Sales.Helpers;
using Xamarin.Forms;

namespace Sales.ViewModels
{
    public class MenuItemViewModel
    {
        #region Attributes

        #endregion

        #region Properties

        public string Icon { get; set; }

        public string Title { get; set; }

        public string PageName { get; set; }

        #endregion

        #region Constructors

        #endregion

        #region Methods

        #endregion

        #region Commands

        public ICommand GotoCommand
        {
            get
            {
                return new RelayCommand(Goto);
            }
        }

        private void Goto()
        {
            if (this.PageName == "LoginPage")
            {
                Settings.AccessToken = String.Empty;
                Settings.TokenType = String.Empty;
                Settings.IsRemembered = false;
                Settings.Issued = DateTime.MinValue;
                Settings.Expires = DateTime.MinValue;
                MainViewModel.GetIntance().Login = new LoginViewModel();
                Application.Current.MainPage = new NavigationPage(new LoginPage());
            }
        }

        #endregion


    }
}
