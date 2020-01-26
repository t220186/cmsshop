using Cms.Models.Data;
using Cms.Models.ViewModels.Shop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace Cms.Controllers
{
    public class ShopController : Controller
    {
        // GET: Shop
        public ActionResult Index()
        {

            return RedirectToAction("Index", "Pages");
        }

        public ActionResult CategoryMenuPartial()
        {
            //init Category ViewModel list
            List<CategoriesViewModel> CategoryViewModelList;
            //init db
            using (Db db = new Db())
            {
                CategoryViewModelList = db.Categories.ToArray().OrderBy(x => x.Sorting).Select(x => new CategoriesViewModel(x)).ToList();
            }

            return PartialView(CategoryViewModelList);
        }

        //category
        //GET: /Shop/Category/name
        [HttpGet]
        public ActionResult Category(string name)
        {
            //Set Bag CategoryName
            ViewBag.CategoryName = "";
            //init var 
            int catId;
            //init view Model
            List<ProductsViewModel> ProductsViewModelList;
            //get category
            using (Db db = new Db())
            {
                //  if (db.Categories.Any(x => x.Name == catName))
                //   {
                //      return RedirectToAction("Index", "Pages");
                //  }
                //db.context.where,order.select.return

                CategoriesDTO dTO = db.Categories.Where(x => x.Slug == name).FirstOrDefault();
                Console.WriteLine(dTO);
                //check slug exists
                if (dTO == null) {
                    return RedirectToAction("Index", "Pages");
                }


                //set category Id
                catId = dTO.Id;
                ViewBag.CategoryName = dTO.Name;
            }

            //get productsList

            using (Db db = new Db())
            {
                //set ProductsList
                ProductsViewModelList = db.Products.ToArray().Where(x => x.CategoriesId == catId).Select(x => new ProductsViewModel(x)).ToList();
            }
            //return ProductsViewModel List   
            return View(ProductsViewModelList);
        }
        //list all new products
        //rozwinąć o paginację !!
        //GET /Shop/ListAllProductsNew/name
            public ActionResult ListAllProductsNew(string name)
        {

            List<ProductsViewModel> productVMList;
            using (Db db = new Db()) {
                productVMList = db.Products.ToArray().Select(x => new ProductsViewModel(x)).ToList();
            }

                return PartialView(productVMList);
        }

        //Product details
        //GET: /Shop/products/{name}
        [HttpGet]
     // [ActionName("produkt-szczegoly")]
        public ActionResult Products(string name)
        {
            //init product view model
            ProductsViewModel productsViewModel;
            //inti id for galleryModels 
            int id;
            using (Db db = new Db())
            {
                if (!db.Products.Any(x => x.Slug.Equals(name))) {
                    return RedirectToAction("Index", "Shop");
                }
                ProductsDTO dTO = db.Products.Where(x=>x.Slug.Equals(name)).FirstOrDefault();
                id = dTO.Id;
                productsViewModel = new ProductsViewModel(dTO);
            }
            //init gallery 
            productsViewModel.GalleryImages = Directory.EnumerateFiles(Server.MapPath("~/Images/Uploads/Products" + id + "/Gallery/Thumbs")).Select(fn => Path.GetFileName(fn));
            //return ViewModel
            return View(productsViewModel);
        }

    }


}