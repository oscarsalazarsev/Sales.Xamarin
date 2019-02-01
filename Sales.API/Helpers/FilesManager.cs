using System.IO;
using System.Web;

namespace Sales.API.Helpers
{
    public class FilesManager
    {
        public static bool UploadPhoto(MemoryStream stream, string folder, string name)
        {
            try
            {
                stream.Position = 0;
                var path = Path.Combine(HttpContext.Current.Server.MapPath(folder), name);
                File.WriteAllBytes(path, stream.ToArray());
            }
            catch
            {
                return false;
            }

            return true;
        }

    }
}