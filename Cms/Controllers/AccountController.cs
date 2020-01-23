using Cms.Models.Data;
using Cms.Models.ViewModels.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Cms.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            return Redirect("~/Account/Login");
        }
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        //GET: /Account/CreateUser//
        public ActionResult CreateAccount()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateAccount(UsersViewModel model)
        {
            //walidacja rejestracji użytkownika
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            //check password s-s
            if (!model.Password.Equals(model.ConfirmPassword)) { 
                ModelState.AddModelError("", "Hasła nie pasują do siebie");
                return View(model);
                }
            //find any account email
            using (Db db = new Db()) {
                if (db.Users.Any(x => x.Email.Equals(model.Email))) {
                    ModelState.AddModelError("", "Użytkownik o podanym adresie e-mail już jest zapisany");
                    return View(model);
                }
                //using md5
                using (MD5 md5Hash = MD5.Create()) {
                    string stringPass = GetMd5Hash(md5Hash, model.Password);
                    model.Password = stringPass;
                }


                    //dodaj nowego użytkownika
                    UsersDTO Dto = new UsersDTO()

                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Email,
                        Username = model.Email,
                        Password = model.Password
                    };
                //get it and set new UsersRole
               
                db.Users.Add(Dto);

                db.SaveChanges();




            }
            //return model
            TempData["Sm"] = "Użykownik nie może zostać dodany";
            return RedirectToAction("Login");
        }
        

        public ActionResult Login() {
            //sprawdzenie czy user jest już zalogowany 

            //logowanie użytkownika
            string UserName = User.Identity.Name;
            if (!string.IsNullOrEmpty(UserName)) { // przekierowanie na stronę sklepu 
            //   return Redirect("~/Pages/Index");
                //bazowy adres strony profilowej 
                return RedirectToAction("ProfileAccount");
            }


            return View();
        }


        /////ProfileAccount
        ///
        public ActionResult ProfileAccount() {


            return View();
        }


        ///Partrial View Dla AccountMenu
        ///
        public ActionResult ProfileAccountMenuPartial()
        {


            return View();
        }


        /**
         * 
         */
        static string GetMd5Hash(MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }
    }
}