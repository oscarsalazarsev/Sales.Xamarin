using Sales.BackEnd.Helpers;
using Sales.BackEnd.Models;
using Sales.Common.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Sales.BackEnd.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private LocalDataContext db = new LocalDataContext();

        // GET: Products
        public async Task<ActionResult> Index()
        {
            return View(await this.db.Products.OrderBy(p => p.Description).ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = await this.db.Products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ProductView productView)
        {
            if (ModelState.IsValid)
            {
                var pic = string.Empty;
                var folder = "~/Content/Images/Products";

                if (productView.ImageFile != null)
                {
                    pic = FilesHelper.UploadPhoto(productView.ImageFile, folder);
                    pic = $"{folder}/{pic}";
                }

                Product product = this.ToProduct(productView, pic);
                this.db.Products.Add(product);
                await this.db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(productView);
        }

        private Product ToProduct(ProductView productView, string pic)
        {
            return new Product
            {
                ImagePath       = pic,
                Description     = productView.Description,
                IsAvailable     = productView.IsAvailable,
                Price           = productView.Price,
                ProductId       = productView.ProductId,
                PublshOn        = productView.PublshOn,
                Remarks         = productView.Remarks,
            };
        }

        // GET: Products/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = await this.db.Products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }

            ProductView productView = this.ToView(product);
            return View(productView);
        }

        private ProductView ToView(Product product)
        {
            return new ProductView
            {
                ImagePath   = product.ImagePath,
                Description = product.Description,
                IsAvailable = product.IsAvailable,
                Price       = product.Price,
                ProductId   = product.ProductId,
                PublshOn    = product.PublshOn,
                Remarks     = product.Remarks,
            };
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ProductView productView)
        {
            if (ModelState.IsValid)
            {
                var pic = productView.ImagePath;
                var folder = "~/Content/Images/Products";

                if (productView.ImageFile != null)
                {
                    pic = FilesHelper.UploadPhoto(productView.ImageFile, folder);
                    pic = $"{folder}/{pic}";
                }

                Product product = this.ToProduct(productView, pic);
                this.db.Entry(product).State = EntityState.Modified;
                await this.db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(productView);
        }

        // GET: Products/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = await this.db.Products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Product product = await this.db.Products.FindAsync(id);
            this.db.Products.Remove(product);
            await this.db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
