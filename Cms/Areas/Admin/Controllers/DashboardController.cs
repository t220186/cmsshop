using System.Web.Mvc;

namespace Cms.Areas.Admin.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Admin/Dashboard
        public ActionResult Index()
        {
            ViewBag.Title = "Panel administracyjny";
            return View();
        }
        public ActionResult Add() {
            return View();
        }
    }
}