using Cms.Models;
using Cms.Models.Data;
using Cms.Models.ViewModels.Shop;
using Cms.Models.ViewModels.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;

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
            int IdUzytkownika;
            string sendMessage = "";
            //check password s-s
            if (!model.Password.Equals(model.ConfirmPassword))
            {
                ModelState.AddModelError("", "Hasła nie pasują do siebie");
                return View(model);
            }
            //find any account email
            using (Db db = new Db())
            {
                if (db.Users.Any(x => x.Email.Equals(model.Email)))
                {
                    ModelState.AddModelError("", "Użytkownik o podanym adresie e-mail już jest zapisany");
                    return View(model);
                }
                //using md5
                using (MD5 md5Hash = MD5.Create())
                {
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
                //get Default Role Id

                UsersRoleDTO usersRoleDTO = new UsersRoleDTO()
                {
                    UserId = Dto.Id,
                    RoleId = 1
                };
                IdUzytkownika = Dto.Id;
                db.UsersRole.Add(usersRoleDTO);
                db.SaveChanges();
                //
                ////////////
                ///@TODO
                /// send email to activate users;

                //testDrive send email
                sendMessage = SendMsg(model);

                //end Context
            }
            //return model
            TempData["sm"] = sendMessage;
            return RedirectToAction("Login");
        }


        public ActionResult Login()
        {
            //sprawdzenie czy user jest już zalogowany 

            //logowanie użytkownika
            string UserName = User.Identity.Name;
            if (!string.IsNullOrEmpty(UserName))
            { // przekierowanie na stronę sklepu 
              //   return Redirect("~/Pages/Index");
              //bazowy adres strony profilowej 
                return RedirectToAction("ProfileAccount");
            }


            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginUserViewModel model)
        {
            //sprawdzenie czy formularz jest dobrze wypełniony 
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            using (MD5 md5Hash = MD5.Create())
            {
                string stringPass = GetMd5Hash(md5Hash, model.Password);
                model.Password = stringPass;
            }
            //isValid 
            bool isValid = false;
           
            //context
            using (Db db = new Db())
            {
                //czy istnieje uzytkownik o podanym adresie email
                if (db.Users.Any(x => x.Username.Equals(model.UserName) && x.Password.Equals(model.Password)))
                {
                    isValid = true;
                }
            }
            //set user is valid 
            if (!isValid)
            {
                ModelState.AddModelError("", "Nieprawidłowa nazwa użytkownika lub hasło");
                return View(model);
            }
            
           
            
            
                //forms Auth
                FormsAuthentication.SetAuthCookie(model.UserName, model.Remember);
            ///setRoles
           
            
            //redirect to default
            return Redirect(FormsAuthentication.GetRedirectUrl(model.UserName, model.Remember));
        }


        /////ProfileAccount
        ///
        public ActionResult ProfileAccount()
        {
            //check users is identity

            bool IsAuthenticated = User.Identity.IsAuthenticated;
            if (!IsAuthenticated) { return RedirectToAction("Login"); }
            //get Authenticated User
            string UserName = User.Identity.Name;
            //get User Model
            ViewBag.UserName = UserName;
            UserProfileViewModel model;
            using (Db db = new Db())
            {
                UsersDTO dto = db.Users.FirstOrDefault(x => x.Username.Equals(UserName));
                model = new UserProfileViewModel(dto);
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult ProfileAccount(UserProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (!string.IsNullOrWhiteSpace(model.Password))
            {
                if (!model.Password.Equals(model.ConfirmPassword))
                {
                    ModelState.AddModelError("", "Hasła nie pasują do siebie");
                    return View(model);
                }
            }

            if (!string.IsNullOrWhiteSpace(model.Password))
            {
                using (MD5 md5Hash = MD5.Create())
                {
                    //hash passwsord
                    string newPass = GetMd5Hash(md5Hash, model.Password);
                    model.Password = newPass;
                }
            }

            using (Db db = new Db()) {
                string userName = User.Identity.Name;
                if(db.Users.Where(x=>x.Id != model.Id).Any(x => x.Username.Equals(userName)))
                {
                    ModelState.AddModelError("", "Użytkownik o podanym adresie email:"+ userName +" już istnieje");
                    model.Username = "";
                    model.Email = "";
                }

                UsersDTO dTO = db.Users.Find(model.Id);
                dTO.FirstName = model.FirstName;
                dTO.LastName = model.LastName;
                dTO.Email = model.Email;
                dTO.Username = model.Email;
                if (!string.IsNullOrWhiteSpace(model.Password))
                {
                    dTO.Password = model.Password;
                }
                    //zapis danych

                    db.SaveChanges();

            }//
            TempData["sm"] = "Profil został zaktualizowany";
            return RedirectToAction("ProfileAccount");

        }

        ///Partrial View Dla AccountMenu
        ///
        public ActionResult ProfileAccountMenuPartial()
        {

            string UserName = User.Identity.Name;
            bool IsAuthenticated = User.Identity.IsAuthenticated;
            if (IsAuthenticated) { ViewBag.UserName = UserName;      
            }
            //get UserData and  Set ViewModel 
            UserNavPartialViewModel model;
                using (Db db = new Db())
                {
                    UsersDTO dTO = db.Users.FirstOrDefault(x => x.Username.Equals(UserName));
                    model = new UserNavPartialViewModel
                    {
                        Name = dTO.FirstName + " " + dTO.LastName,
                        Email = dTO.Email,

                    };

                }
          
            return PartialView(model);
        }

        public ActionResult Logout()
        {
            bool IsAuthenticated = User.Identity.IsAuthenticated;
            if (IsAuthenticated == true)
            {
                FormsAuthentication.SignOut();
            }
            //Wylogowanie i przekierowanie do srony głównej sklepu
            TempData["sm"] = "Poprawnie wylogowano";
            return Redirect("~/");
        }
        //GET: /account/orders
        public ActionResult Orders() {
            //init view Model 
            List<OrdersForUserViewModel> ordersForUser = new List<OrdersForUserViewModel>();
            using (Db db = new Db())
            {
                //pobierz userId            
                UsersDTO user = db.Users.Where(x => x.Email.Equals(User.Identity.Name)).FirstOrDefault();
                int userId = user.Id;
                //zamowienia dla uzytkownika 
                //towrzymy listę zamowien / zamowienia/ pobieramy z tabeli /gdzie warunkiem jest id (w tym przypadku id uzytkownika)/ do Tablicy / pobieramy towrzac nowy ViewModel zamowienia
                List<OrderViewModel> orders = db.Orders.Where(x => userId.Equals(userId)).ToArray().Select(x => new OrderViewModel(x)).ToList();
                ///iteracja po wszystkich zamowieniach 
                foreach (var order in orders)
                {
                    ///inicjalizacja slownika
                    ///
                    Dictionary<string, int> ProductsAndQty = new Dictionary<string, int>();
                    //total
                    decimal total = 0m;
                    //szczegoly zamowienia 
                    List<OrderDetailsDTO> orderDetailsDTO = db.OrdersDetails.Where(x => x.OrderId.Equals(order.OrderId)).ToList();
                    ///foreach - iteracja po poszczególnych szczegółach zamówienia
                    ///
                    foreach(var orderDetailsItem in orderDetailsDTO)
                    {
                        ProductsDTO productsDTO = db.Products.Where(x => x.Id.Equals(orderDetailsItem.ProductId)).FirstOrDefault();
                        //pobierz cena
                        decimal price = productsDTO.Price;
                        string productName = productsDTO.Name;
                        //add products to dictionary
                        ProductsAndQty.Add(productName,orderDetailsItem.Quantity);
                        // suma total
                        total += orderDetailsItem.Quantity * price;


                    }
                    //set list ordersForUser 
                    ordersForUser.Add(new OrdersForUserViewModel()
                    {
                        //set new List view Model
                        OrdersNumber = order.OrderId,
                        Total = total,
                        ProductsAndQuantity = ProductsAndQty,
                        CreatedD = order.CreatedD,
                    }) ;
                }
            }
            //partial view
            return PartialView(ordersForUser);
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

        //testDrive 
        static string SendMsg(UsersViewModel model)
        {
            string errorMessage = "";
            string response = "success";
            Config config = new Config();
            try
            {
                WebMail.SmtpServer = config.SmtpServerName;
                WebMail.SmtpPort = config.SmtpPort;
                WebMail.UserName = config.SmtpUserName;
                WebMail.Password = config.SmtpPassword;
                WebMail.From = config.SmtpFrom;

                // Send email
                WebMail.Send(to: model.Email,
                    subject: "Użytkownik zarejestrowany - " + model.Username,
                    body: "Gratulacje zarejestrowano użytkownika: " + model.FirstName + " " + model.LastName + "."
                );

            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }


            if (errorMessage == "") { return response; }
            return errorMessage;

        }
    }
}