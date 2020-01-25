using Cms.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Cms
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //ApplicationAuthenticzteionRequest

        }
        protected void Application_AuthenticateRequest() {
            //sprawdzenie czy jest Autoryzacja
            if(User == null)
            {
                return;
            }
            //
            string userName = Context.User.Identity.Name;
            //deklaracja tablicy z rolami 
            string[] RolesArray = null;
            //get Roles
            using (Db db = new Db())
            {
                ///get User data
                UsersDTO dTO = db.Users.FirstOrDefault(x => x.Email.Equals(userName));
                //get array
                RolesArray = db.UsersRole.Where(x => x.UserId.Equals(dTO.Id)).Select(x=>x.Roles.Name).ToArray();
            }

            //tworzenie IPrincipal object
            IIdentity userIdentity = new GenericIdentity(userName);//string name
            IPrincipal  newUserObj = new GenericPrincipal(userIdentity, RolesArray);//przekazanie IIdentity User oraztablicy z rolami
            //update Context

            Context.User = newUserObj;


        }
    }
}
