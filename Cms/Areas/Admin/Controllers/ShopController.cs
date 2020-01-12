using Cms.Models.Data;
using Cms.Models.ViewModels.Shop;
using System;
using System.Collections.Generic;
using System.Linq;
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
            string  id;

            //context

            using (Db db = new Db()) {

                CategoriesDTO dTO = new CategoriesDTO();
                if(db.Categories.Any(x =>x.Name == catName))
                     return "catexists";
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


    }
}