using Cms.Models.Data;
using Cms.Models.ViewModels.Shop;
using PagedList;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace Cms.Areas.Admin.Controllers
{
    public class ShopController : Controller
    {
        // GET: Admin/Shop/Categories
        public ActionResult Categories()
        {
            //action name
            ViewBag.Title = "Kategorie";
            //get All categories and list
            List<CategoriesViewModel> categoriesList;
            using (Db db = new Db())
            {
                categoriesList = db.Categories
                    .ToArray()
                    .OrderBy(x => x.Sorting)
                    .Select(x => new CategoriesViewModel(x))
                    .ToList();
            }


            return View(categoriesList);
        }
        /**
         * Add new Categories to db 
         * return true or TS
         * 
         **/
        //Post: Admin/Shop/AddCategories
        [HttpPost]
        //async response
        public string AddCategories(string catName)
        {
            //set string id
            string id;

            //context

            using (Db db = new Db())
            {

                CategoriesDTO dTO = new CategoriesDTO();
                if (db.Categories.Any(x => x.Name == catName))
                {
                    return "catexists";
                }
                //dto 
                dTO.Name = catName;
                dTO.Slug = catName.Replace(" ", "_").ToLower();
                dTO.Sorting = 100;

                db.Categories.Add(dTO);
                db.SaveChanges();

                //get new id
                id = dTO.Id.ToString();

            }

            return id;
        }

        //POST: Admin/Shop/ReorderCategories
        [HttpPost]
        public string ReorderCategories(int[] id)
        {
            string status = "false"; ;

            using (Db db = new Db())
            {
                //init couter
                int count = 1;
                //get dto 
                CategoriesDTO dTO;
                //sort category
                foreach (var catId in id)
                {

                    dTO = db.Categories.Find(catId);
                    dTO.Sorting = count;
                    //save db changess
                    db.SaveChanges();
                    //count sorting
                    count++;
                    status = "true";
                }


            }

            return status;
        }
        //GET: Admin/Shop/DeleteCategory
        [HttpGet]
        public ActionResult DeleteCategory(int id)
        {
            using (Db db = new Db())
            {
                CategoriesDTO dTO = db.Categories.Find(id);
                db.Categories.Remove(dTO);
                db.SaveChanges();
            }
            //
            return RedirectToAction("Categories");
        }

        //POST RenameCategory
        [HttpPost]
        public string RenameCategory(string newCatName, int id)
        {
            string response;

            //using 
            using (Db db = new Db())
            {
                //check if newCatName exist return "catexists"

                CategoriesDTO dTO = new CategoriesDTO();
                if (db.Categories.Any(x => x.Name == newCatName))
                {
                    return "catexists";
                }
                /**
                 * 
                 * @todo
                 * -add modification date
                 * */
                //find eleement to edit
                dTO = db.Categories.Find(id);
                //
                dTO.Name = newCatName;
                //set slug
                dTO.Slug = newCatName.Replace(" ", "-").ToLower();
                db.SaveChanges();
                response = "true";

            }
            return response;

        }


        /**
         * Add new Products
         * 
         **/
        //GET Admin/Shop/AddProducts
        [HttpGet]
        public ActionResult AddProduct()
        {

            ViewBag.Title = "Nowy produkt";

            //getAllCategories List {Id:id, Categories: Name}
            //initModel

            ProductsViewModel model = new ProductsViewModel();
            //    CategoriesViewModel categories = new CategoriesViewModel();
            using (Db db = new Db())
            {
                model.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");


            }

            return View(model);
        }
        //AddProduct - save 
        //POST: /Admin/Shop/AddProduct
        //HttpPostedFileBase - zapis plików i przesyłanie
        [HttpPost]
        public ActionResult AddProduct(ProductsViewModel model, HttpPostedFileBase file)
        {


            //valid
            if (!ModelState.IsValid)
            {
                //if model !is valid must return Categories Select List
                using (Db db = new Db())
                {

                    model.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");
                    return View(model);
                }
            }
            int id;
            //
            using (Db db = new Db())
            {
                //valid is Any Products exists
                if (db.Products.Any(x => x.Name == model.Name))
                {
                    ModelState.AddModelError("", "Produkt o tej samej nazwie już istnieje");
                    return View(model);
                }
                ////dto
                ProductsDTO product = new ProductsDTO();
                product.Name = model.Name;
                product.Slug = model.Name.Replace(" ", "-").ToLower();
                product.Price = model.Price;
                product.Description = model.Description;
                product.CategoriesId = model.CategoriesId;

                db.Products.Add(product);
                db.SaveChanges();

                //get new  add productId
                id = product.Id;

            }


            //Redirect to 
            TempData["Sm"] = "Dodałeś nowy produkt";
            #region Upload Image
            //create folders 
            //wskazuje na główny katalog i dodaje Images\Uploads
            var originDir = new DirectoryInfo(string.Format("{0}Images\\Uploads", Server.MapPath(@"\")));
            //path string 1
            var pathString1 = Path.Combine(originDir.ToString(), "Products");
            //idProduct2String file save to products and Product Id
            var pathString2 = Path.Combine(originDir.ToString(), "Products" + id.ToString());
            //var thumbs
            var pathString3 = Path.Combine(originDir.ToString(), "Products" + id.ToString() + "\\Thumbs");
            //var gallery
            var pathString4 = Path.Combine(originDir.ToString(), "Products" + id.ToString() + "\\Gallery");
            //var gallery thumbs
            var pathString5 = Path.Combine(originDir.ToString(), "Products" + id.ToString() + "\\Gallery\\Thumbs");

            //check and create
            if (!Directory.Exists(pathString1))
            {
                //create new directory
                Directory.CreateDirectory(pathString1);
            }
            //2
            if (!Directory.Exists(pathString2))
            {
                //create new directory
                Directory.CreateDirectory(pathString2);
            }
            //3

            if (!Directory.Exists(pathString3))
            {
                //create new directory
                Directory.CreateDirectory(pathString3);
            }
            //4
            if (!Directory.Exists(pathString4))
            {
                //create new directory
                Directory.CreateDirectory(pathString4);
            }
            //
            if (!Directory.Exists(pathString5))
            {
                //create new directory
                Directory.CreateDirectory(pathString5);
            }

            //file extension check
            if (file != null && file.ContentLength > 0)
            {
                //this is image- file extension
                string ext = file.ContentType.ToLower();
                if (ext != "image/jpg" && ext != "image/jpeg" && ext != "image/png" && ext != "image/gif")
                {
                    using (Db db = new Db())
                    {

                        model.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");
                        ModelState.AddModelError("", "Obraz nie został przesłany ponieważ jest nieprawidłowe rozszerzenie obrazu");
                        return View(model);
                    }

                }
                string imageName = file.FileName;

                using (Db db = new Db())
                {

                    ProductsDTO dTO = db.Products.Find(id);
                    dTO.ImageName = imageName;
                    //updateImage
                    db.SaveChanges();


                }
                //zapis obrazka na serwerze
                var path = string.Format("{0}\\{1}", pathString2, imageName);
                //zapisz miniaturkę
                var path2 = string.Format("{0}\\{1}", pathString3, imageName);

                //zapis pliku
                file.SaveAs(path);
                //zapis miniaturki -webimage() <-przekazujemy plik
                WebImage img = new WebImage(file.InputStream);
                img.Resize(200, 200);
                img.Save(path2);

            }

            #endregion

            return RedirectToAction("AddProduct");

        }
        /**
         * lista produktów // paginacja
         * */
        ///Products - base
        /**
         *  int? - > parametr int  może być null
         *  catId - >filtrowanie po kategoriach produktów
         **/
        //GET: /Admin/Shop/Products
        [HttpGet]
        public ActionResult Products(int? page, int? catId)
        {
            //action name
            ViewBag.Title = "Lista produktów ";
            //get All categories and list
            //lista produktów
            //list<ViewModel> nazwa zmiennej
            List<ProductsViewModel> listOfProductViewModel;
            //set page 4 pagination if(??)[querystring] to the first page 
            var pageNumber = page ?? 1;
            using (Db db = new Db())
            {
                //get Product list to array
                /**
                 * jezeli catId jest null lub 0 lub przyjmuje wartosc CategoriesId
                 * */
                listOfProductViewModel = db.Products
                    .ToArray()
                    .Where(x => catId == null || catId == 0 || x.CategoriesId == catId)
                    .Select(x => new ProductsViewModel(x))
                    .ToList();
                //categories DropdownList
                ViewBag.Categories = new SelectList(db.Categories.ToList(),"Id","Name");
                //ustawiam wybraną kategorię 
                ViewBag.SelectedCat = catId.ToString();

            }
            //ustawienie paginacji

            ViewBag.productDir = "Products";
            var onePageOfProducts = listOfProductViewModel.ToPagedList(pageNumber, 5);
            //view bag do widoku(pagedList)
            ViewBag.OnePageOfProducts = onePageOfProducts;
            //zwraca widok z listą produktów
            return View(listOfProductViewModel);
        }
    }
}