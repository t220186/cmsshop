using Cms.Models.Data;
using Cms.Models.ViewModels.Pages;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Cms.Controllers
{
    public class PagesController : Controller
    {
        // GET: Pages  - /index/{pages} || 
        //string page = "domyślnie"
        [HttpGet]
        public ActionResult Index(string page = "")
        {
            //set home page 
            if (page == "")
            {
                page = "home";
                ViewBag.IsHome = "true";
            }
            //set pageViewModel PageDTo
            PageViewModel model;
            PageDTO dTo;
            //check page exists
            using (Db db = new Db())
            {
                if (!db.Pages.Any(x => x.Slug.Equals(page)))
                {
                    return RedirectToAction("Index", new { page = "" });
                }
            }
            //get PageDTO 
            using (Db db = new Db())
            {
                dTo = db.Pages.Where(x => x.Slug == page).FirstOrDefault();
            }

            //set Page Title 
            ViewBag.PageTitle = dTo.Title;
            //set sideBar
            if (dTo.HasSidebar == true)
            {
                //get sidebar
                ViewBag.Sidebar = "1";
            }
            else
            {
                ViewBag.Sidebar = "0";
            }
            //init model PageViewModel
            model = new PageViewModel(dTo);
            //model PageViewModel
            return View(model);
        }
        //po dodaniu kodu partial, dodać  routing 
        public ActionResult PagesMenuPartial()
        {
            //declaration Page ViewModel
            List<PageViewModel> pageVMList;
            //set List
            using (Db db = new Db())
            {
                //get PageVmList
                pageVMList = db.Pages.ToArray().OrderBy(x => x.Sorting).Where(x => x.Slug != "home").Select(x => new PageViewModel(x)).ToList();

            }

            //zwracamy pageViewModelList
            return PartialView(pageVMList);

        }

        /**
         * @name Partial PagesSideBarPartial
         */
        //GET: /Pages/PagesSideBarPartial
        [HttpGet]
        public ActionResult PagesSideBarPartial()
        {
            //init pageSideBarView
            SideBarViewModel pageSideBarView;
            //get sideBar 
            using (Db db = new Db())
            {
                SideBarDTO dTO = db.SideBar.Find(1);
                //set SideBarViewModel
                pageSideBarView = new SideBarViewModel(dTO);

            }
            //return model view model
            return PartialView(pageSideBarView);

        }
        public ActionResult CaruselPartial()
        {
            //init AdvertisementList
            List<AdvertisementViewModel> advertisementViewModels = new List<AdvertisementViewModel>();
            //

            return PartialView(advertisementViewModels);
        }

    }

}