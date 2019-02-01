using Sales.Common.Models;
using System.Web;

namespace Sales.BackEnd.Models
{
    public class ProductView : Product
    {

        public HttpPostedFileBase ImageFile { get; set; }
    }
}