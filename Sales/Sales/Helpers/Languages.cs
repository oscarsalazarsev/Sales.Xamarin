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

    }

}
