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

    }

}
