using Cms.Models.Data;
using Cms.Models.ViewModels.Shop;
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


    }
}