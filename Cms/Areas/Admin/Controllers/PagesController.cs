using Cms.Models.Data;
using Cms.Models.ViewModels.Pages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cms.Areas.Admin.Controllers
{
    public class AllowHtmlBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext,
            ModelBindingContext bindingContext)
        {
            var request = controllerContext.HttpContext.Request;
            var name = bindingContext.ModelName;
            return request.Unvalidated[name]; //magic happens here
        }
    }
    public class PagesController : Controller
    {
        // GET: Admin/Pages
        public ActionResult Index()
        {
            ViewBag.Title = "Przegląd stron ";
            //declaration of page Model
            List<PageViewModel> pagesList;
            //korzystanie z db w usingu dzieki czemu po wykonaniu akcji - następuje zamknięcie działania i oczyszczenie pamięci;
            using (Db db = new Db())
            {
                ///get data to list and using :Lambda expression x =>x.ColumnName 
                ///Get All Page List
                pagesList = db.Pages.ToArray().OrderBy(x => x.Sorting).Select(x => new PageViewModel(x)).ToList();
            }

            //pageList to view 

            return View(pagesList);
        }
        //GET: Admin/Pages/Add
        [HttpGet]
        public ActionResult AddPage()
        {
            //tytuł.
            ViewBag.Title = "Dodaj stronę";

            return View();
        }
        [HttpPost]
        public ActionResult AddPage(PageViewModel model)
        {
            //checkedModelState
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (Db db = new Db())
            {
                string slug;
                int sorting;

                PageDTO dTO = new PageDTO();

                dTO.Title = model.Title;

                if (string.IsNullOrWhiteSpace(model.Slug))
                {
                    slug = model.Title.Replace(" ", "-").ToLower();
                }
                else
                {
                    slug = model.Slug;
                }
                //if slug exists!
                if (db.Pages.Any(x => x.Slug == slug))
                {
                    ModelState.AddModelError("", "Strona o podanym adresie już istnieje");
                    return View(model);
                }
                dTO.Slug = slug;
                dTO.Body = model.Body;
                dTO.HasSidebar = model.HasSidebar;
                //get model.sorting ->vvar sorting;


                sorting = model.Sorting;

                if (db.Pages.Any(x => x.Sorting == sorting))
                {
                    sorting = db.Pages.Count() + 1;
                }

                dTO.Sorting = sorting;
                //add and save
                db.Pages.Add(dTO);
                db.SaveChanges();


            }
            TempData["Sm"] = "Dodałeś nową stronę";
            return RedirectToAction("AddPage");
        }

        //details Page 
        [HttpGet]
        public ActionResult Details(int id)
        {
            //action title
            ViewBag.Title = "Szczegóły strony";
            //Model
            PageViewModel model;
            //using

            using (Db db = new Db())
            {
                //set pageDTo
                PageDTO dTO = db.Pages.Find(id);
                if (dTO == null)
                {
                    return Content("Strona nie istnieje");
                }
                else
                {
                    model = new PageViewModel(dTO);
                }
            }
            //
            return View(model);
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            //Declarete PageView Model
            PageViewModel model;
            ViewBag.Title = "Edycja strony";

            //Get db Context
            using (Db db = new Db())
            {
                //set PageDTO
                PageDTO dTO = db.Pages.Find(id);
                if (dTO == null)
                {
                    return Content("Strona nie istnieje");

                }
                else
                {
                    model = new PageViewModel(dTO);
                }
            }
            return View(model);
        }



        [HttpPost]
        public ActionResult Edit(PageViewModel model)
        {
            int id = model.Id;
            //checkedModelState
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (Db db = new Db())
            {
                //start Using
                PageDTO dTO = db.Pages.Find(id);
                //init slug
                string slug = "home";

                //get page to edit 
                dTO.Title = model.Title;
                if (model.Slug != "home")
                {

                    if (string.IsNullOrWhiteSpace(model.Slug)) { slug = model.Title.Replace(" ", "-").ToLower(); } else { slug = model.Slug.Replace(" ", "-").ToLower(); }
                    dTO.Slug = slug;
                }
                //check page to prevent duplicate title or (Adress)Slug
                if (db.Pages.Where(x => x.Id != id).Any(x => x.Title == model.Title) || db.Pages.Where(x => x.Id != id).Any(x => x.Slug == slug))
                {
                    ModelState.AddModelError("", "Strona lub tytuł już istnieje");
                }

                dTO.Slug = slug;
                dTO.Body = model.Body;
                dTO.HasSidebar = model.HasSidebar;
                dTO.Sorting = model.Sorting;

                //update  //save
                db.SaveChanges();


                //end Using
            }

            TempData["Sm"] = "Strona została zaktualizowana";
            return RedirectToAction("Index");
        }


        public ActionResult Delete(int id)
        {
            ViewBag.Title = "Usuń stronę";
            PageViewModel model;

            //using
            using (Db db = new Db())
            {
                PageDTO dTO = db.Pages.Find(id);
                if (dTO == null)
                {
                    return Content("Strona nie istnieje");

                }
                else
                {
                    model = new PageViewModel(dTO);
                }
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(PageViewModel model)
        {
            //model
            int id = model.Id;
            //using 
            using (Db db = new Db())
            {
                PageDTO dTO = db.Pages.Find(id);
                if (dTO.Slug == "home")
                {
                    TempData["Sm"] = "Strona" + model.Title + " nie została usunięta ze względu na to że jest to strona Domowa";
                    return RedirectToAction("Index");
                }


                db.Pages.Remove(dTO);
                db.SaveChanges();
            }

            TempData["Sm"] = "Strona" + model.Title + " została Usunięta";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult SortPages(int[] id)
        {

            int count = 1;
            using (Db db = new Db())
            {
                PageDTO dTO;
                //sort pages 

                foreach (var item in id)
                {
                    dTO = db.Pages.Find(item);
                    dTO.Sorting = count;
                    db.SaveChanges();
                    count++;
                }

            }
            return View();
        }

        //Edit SideBar 
        public ActionResult EditSidebar()
        {
            ViewBag.Title = "Edycja paska bocznego";

            //get view model - sidebar
            SideBarViewModel model;

            //using
            using (Db db = new Db())
            {
                //get Context
                SideBarDTO dTO = db.SideBar.Find(1);
                model = new SideBarViewModel(dTO);
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult EditSideBar(SideBarViewModel model)
        {
            int id = model.Id;

            //using context 
            using (Db db = new Db())
            {
                //sidebar context dto
                SideBarDTO dTO = db.SideBar.Find(id);
                //get model Data
                dTO.Body = model.Body;
                db.SaveChanges();
            }
            TempData["Sm"] = "SideBar został zaktualizowany.";
            return RedirectToAction("Index");
        }
        //GET  /Admin/Pges/LeadAdvertisement/
        [HttpGet]
        public ActionResult LeadAdvertisement()
        {
            //getAdvertisementViewModel
            List<AdvertisementViewModel> advertisementViewModels;
            //getAll Advertisement and count item in
            int countItemsIn = 0;
            using (Db db = new Db())
            {
                //get All and Count Item in;
                advertisementViewModels = db.Advertisement.ToArray().OrderBy(x => x.Create).Select(x => new AdvertisementViewModel(x)).ToList();
                foreach (var counterItem in advertisementViewModels)
                {
                    List<AdvertisementItemViewModel> advertisementItemViewModels;
                    if (!db.AdvertisementItem.Where(x => x.IdAvertisement.Equals(counterItem.Id)).Any())
                    {
                        countItemsIn = 0;
                    }
                    else
                    {
                        advertisementItemViewModels = db.AdvertisementItem.ToArray().Where(x => x.IdAvertisement.Equals(counterItem.Id)).OrderBy(x => x.Create).Select(x => new AdvertisementItemViewModel(x)).ToList();
                        countItemsIn = advertisementItemViewModels.Count();
                    }
                }
            }
            return View(advertisementViewModels);
        }
        public ActionResult LeadAdvertisementNew()
        {
            AdvertisementViewModel model = new AdvertisementViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult LeadAdvertisementNew(AdvertisementViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            using (Db db = new Db())
            {
                AdvertisementDTO dTO = new AdvertisementDTO();
                if (db.Advertisement.Any(x => x.Name.Equals(model.Name)))
                {
                    ModelState.AddModelError("", "Reklama o podanje nazwie juz istnieje");
                    model.Name = "";
                    return View(model);
                }
                dTO.Name = model.Name;
                dTO.Create = DateTime.Now;
                dTO.Description = model.Description;
                db.Advertisement.Add(dTO);
                db.SaveChanges();
            }
            TempData["sm"] = "Dodano nową reklamę";
            return RedirectToAction("LeadAdvertisement");
        }

        //GET /Admin/Pages/ConfigAdvertisement
        [HttpGet]
        public ActionResult ConfigAdvertisement(int id)
        {

            AdvertisementViewModel model;

            //set model
            using (Db db = new Db())
            {
                AdvertisementDTO Dto = db.Advertisement.Find(id);
                model = new AdvertisementViewModel(Dto);
            }

            //check list of item exists



            return View(model);
        }

        public ActionResult AdvertisementItemList(int id)
        {
            List<AdvertisementItemViewModel> advertisementItemViewModels;
            using (Db db = new Db())
            {
                //set List 
                advertisementItemViewModels = db.AdvertisementItem.ToArray().Where(x => x.IdAvertisement.Equals(id)).Select(x => new AdvertisementItemViewModel(x)).ToList();


            }
            ViewBag.IdAdvert = id;
            return PartialView(advertisementItemViewModels);
        }
        [HttpGet]
        public ActionResult AddNewAdvertisementItem(int id)
        {
            AdvertisementItemViewModel model = new AdvertisementItemViewModel();
            model.IdAvertisement = id;

            using (Db db = new Db())
            {

                model.Products = new SelectList(db.Products.ToList(), "Id", "Name");

            }

            return View(model);
        }
        [HttpPost]
        public ActionResult AddNewAdvertisementItem(AdvertisementItemViewModel model, HttpPostedFileBase file)
        {
            //set valid state
            if (!ModelState.IsValid)
            {
                using (Db db = new Db())
                {

                    model.Products = new SelectList(db.Products.ToList(), "Id", "Name");

                }
                return View(model);

            }
             //  Initialize id
            int id;
            //check any others advert has idAdvertisement and Primary
            using (Db db = new Db())
            {
               
                //Save 
                AdvertisementItemDTO Dto = new AdvertisementItemDTO();
                Dto.IdAvertisement = model.IdAvertisement;
                
                Dto.LeadText = model.LeadText;
                Dto.LinkTo = model.LinkTo;
                Dto.Create = DateTime.Now;
                Dto.Update = DateTime.Now;
                //save
                db.AdvertisementItem.Add(Dto);
                db.SaveChanges();
                id = Dto.Id;
            }
            #region ImageUpload

            //set path
            var originDir = new DirectoryInfo(string.Format("{0}Images\\Uploads", Server.MapPath(@"\")));
            var pathString1 = Path.Combine(originDir.ToString(), "Advertise");
         
            var pathString2 = Path.Combine(originDir.ToString(), "Advertise\\" + model.IdAvertisement.ToString() + "\\");
            //check path exists
            if (!Directory.Exists(pathString1))
            {
                //create new directory
                Directory.CreateDirectory(pathString1);
            }
            if (!Directory.Exists(pathString2))
            {
                //create new directory
                Directory.CreateDirectory(pathString2);
            }

            //add file to path 

            if (file != null && file.ContentLength > 0)
            {
                //this is image- file extension
                string ext = file.ContentType.ToLower();
                if (ext != "image/jpg" && ext != "image/jpeg" && ext != "image/png" && ext != "image/gif")
                {
                    using (Db db = new Db())
                    {

                        model.Products = new SelectList(db.Products.ToList(), "Id", "Name");
                        ModelState.AddModelError("", "Obraz nie został przesłany ponieważ jest nieprawidłowe rozszerzenie obrazu");
                        return View(model);
                    }

                }
                string imageName = file.FileName;

                using (Db db = new Db())
                {

                    AdvertisementItemDTO Dto = db.AdvertisementItem.Find(id);
                    Dto.Image = imageName;
                    //updateImage
                    db.SaveChanges();


                }
                //zapis obrazka na serwerze
                var path = string.Format("{0}\\{1}", pathString2, imageName);
                //zapis pliku
                file.SaveAs(path);
            }

            //end

            #endregion
            //return 
            return RedirectToAction("ConfigAdvertisement", new { id = model.IdAvertisement });
        }
    }
}