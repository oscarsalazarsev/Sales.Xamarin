using Sales.Interfaces;
using Sales.Resources;
using Xamarin.Forms;

namespace Sales.Helpers
{
    public static class Languages
    {
        static Languages()
        {
            var ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
            Resource.Culture = ci;
            DependencyService.Get<ILocalize>().SetLocale(ci);
        }

        public static string Accept
        {
            get { return Resource.Accept; }
        }

        public static string Error
        {
            get { return Resource.Error; }
        }

        public static string NoInternet
        {
            get { return Resource.NoInternet; }
        }

        public static string titleProducts
        {
            get { return Resource.titleProducts; }
        }

        public static string TurnOnInternet
        {
            get { return Resource.TurnOnInternet; }
        }

        public static string titleAddProduct
        {
            get { return Resource.titleAddProduct; }
        }

        public static string labelDescription
        {
            get { return Resource.labelDescription; }
        }

        public static string placeholderDescription
        {
            get { return Resource.placeholderDescription; }
        }

        public static string labelPrice
        {
            get { return Resource.labelPrice; }
        }

        public static string placeholderPrice
        {
            get { return Resource.placeholderPrice; }
        }

        public static string labelRemarks
        {
            get { return Resource.labelRemarks; }
        }

        public static string btnSave
        {
            get { return Resource.btnSave; }
        }
        public static string labelChangeImage
        {
            get { return Resource.labelChangeImage; }
        }

        public static string errorDescription
        {
            get { return Resource.errorDescription; }
        }

        public static string errorPrice
        {
            get { return Resource.errorPrice; }
        }

        public static string msgImageSource
        {
            get { return Resource.msgImageSource; }
        }

        public static string msgFromGallery
        {
            get { return Resource.msgFromGallery; }
        }

        public static string msgNewPicture
        {
            get { return Resource.msgNewPicture; }
        }

        public static string Cancel
        {
            get { return Resource.Cancel; }
        }

        public static string btnEdit
        {
            get { return Resource.btnEdit; }
        }

        public static string btnDelete
        {
            get { return Resource.btnDelete; }
        }

        public static string msgDeleteConfirmation
        {
            get { return Resource.msgDeleteConfirmation; }
        }

        public static string btnYes
        {
            get { return Resource.btnYes; }
        }

        public static string btnNo
        {
            get { return Resource.btnNo; }
        }

        public static string msgConfirm
        {
            get { return Resource.msgConfirm; }
        }
        public static string titleEditProduct
        {
            get { return Resource.titleEditProduct; }
        }

        public static string labelIsAvailable
        {
            get { return Resource.labelIsAvailable; }
        }

        public static string labelSearch
        {
            get { return Resource.labelSearch; }
        }

        public static string titleLogin
        {
            get { return Resource.titleLogin; }
        }

        public static string labelEMail
        {
            get { return Resource.labelEMail; }
        }

        public static string msgEmailValidation
        {
            get { return Resource.msgEmailValidation; }
        }

        public static string placeholderEmail
        {
            get { return Resource.placeholderEmail; }
        }

        public static string labelPassword
        {
            get { return Resource.labelPassword; }
        }

        public static string msgPasswordValidation
        {
            get { return Resource.msgPasswordValidation; }
        }

        public static string placeholderPassword
        {
            get { return Resource.placeholderPassword; }
        }

        public static string labelRememberme
        {
            get { return Resource.labelRememberme; }
        }

        public static string msgSomethingWrong
        {
            get { return Resource.msgSomethingWrong; }
        }

        public static string titleMenu
        {
            get { return Resource.titleMenu; }
        }

        public static string titleAbout
        {
            get { return Resource.titleAbout; }
        }

        public static string titleSetup
        {
            get { return Resource.titleSetup; }
        }

        public static string msgExit
        {
            get { return Resource.msgExit; }
        }

        public static string msgNoProductsMessage
        {
            get { return Resource.msgNoProductsMessage; }
        }

        public static string labelFirstName
        {
            get { return Resource.labelFirstName; }
        }

        public static string labelLastName
        {
            get { return Resource.labelLastName; }
        }

        public static string labelPhone
        {
            get { return Resource.labelPhone; }
        }

        public static string labelAddress
        {
            get { return Resource.labelAddress; }
        }

        public static string labelPasswordConfirm
        {
            get { return Resource.labelPasswordConfirm; }
        }

        public static string placeholderFirstName
        {
            get { return Resource.placeholderFirstName; }
        }

        public static string placeholderLastName
        {
            get { return Resource.placeholderLastName; }
        }

        public static string placeholderPhone
        {
            get { return Resource.placeholderPhone; }
        }

        public static string placeholderAddress
        {
            get { return Resource.placeholderAddress; }
        }

        public static string placeholderPasswordConfirm
        {
            get { return Resource.placeholderPasswordConfirm; }
        }

        public static string msgFirstNameValidation
        {
            get { return Resource.msgFirstNameValidation; }
        }

        public static string msgLastNameValidation
        {
            get { return Resource.msgLastNameValidation; }
        }

        public static string msgEMailValidationValid
        {
            get { return Resource.msgEMailValidationValid; }
        }

        public static string msgPhoneValidation
        {
            get { return Resource.msgPhoneValidation; }
        }

        public static string msgPasswordValidationLong
        {
            get { return Resource.msgPasswordValidationLong; }
        }

        public static string msgPasswordConfirmValidation
        {
            get { return Resource.msgPasswordConfirmValidation; }
        }

        public static string msgPasswordsNoMatch
        {
            get { return Resource.msgPasswordsNoMatch; }
        }

        public static string msgRegisterConfirmation
        {
            get { return Resource.msgRegisterConfirmation; }
        }

        public static string titleRegister
        {
            get { return Resource.titleRegister; }
        }

    }

}
