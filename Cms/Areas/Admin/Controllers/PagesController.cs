using Cms.Models.Data;
using Cms.Models.ViewModels.Pages;
using System;
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
        public ActionResult AddPage() {
            //tytuł.
            ViewBag.Title = "Dodaj stronę";

            return View();
        }
        [HttpPost]
        public ActionResult AddPage(PageViewModel model)
        {
            //checkedModelState
            if (!ModelState.IsValid)
                return View(model);
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
        public ActionResult Details() {

            return View();
        }


    }
}