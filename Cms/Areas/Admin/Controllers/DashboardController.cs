using System.Web.Mvc;

namespace Cms.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
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