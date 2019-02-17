using System;
using System.IO;
using Sales.Interfaces;

[assembly: Xamarin.Forms.Dependency(typeof(Sales.Droid.Implementations.PathService))]

namespace Sales.Droid.Implementations
{
    public class PathService : IPathService
    {
        public string GetDatabasePath()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            return Path.Combine(path, "Sales.db3");
        }
    }
}