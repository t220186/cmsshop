using Cms.Models.Data;
using Cms.Models.ViewModels.Pages;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Cms.Areas.Admin.Controllers
{
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

        public ActionResult Edit(int id)
        {
            //Declarete PageView Model

            PageViewModel model;

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
        public ActionResult SortPages(int[] id) {

            int count = 1;
            using (Db db = new Db())
            {
                PageDTO dTO;
                //sort pages 

                foreach(var item in id ){
                    dTO = db.Pages.Find(item);
                    dTO.Sorting = count;
                    db.SaveChanges();
                    count++;
                }

            }
            return View();
        }

        //Edit SideBar 
        public ActionResult EditSidebar() {
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
            using (Db db = new Db()) {
                //sidebar context dto
                SideBarDTO dTO = db.SideBar.Find(id);
                //get model Data
                dTO.Body = model.Body;
                db.SaveChanges();
            }
            TempData["Sm"] = "SideBar został zaktualizowany.";
            return RedirectToAction("Index");
        }

    }
}