using Cms.Models.Data;
using Cms.Models.ViewModels.Cart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;

namespace Cms.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        public ActionResult Index()
        {

            //init Cart
            /**
             * ?? jeżeli w sesii nic nie będzie to utworzy nową listę Cart VieModel(pustą)
             */
            var Cart = Session["Cart"] as List<CartViewModel> ?? new List<CartViewModel>();
            //sprawdza czy koszyk jest pusty
            if (Cart.Count == 0 || Session["Cart"] == null)
            {
                ViewBag.Message = "Twój koszyk jest pusty";
                //jeżeli koszytk jest pusty zwracamy pusty widok oraz informację o tym.
                return View();
            }
           
            //oblicz sumarycznej wartości koszyk
            decimal Total = 0m;
            foreach (var item in Cart)
            {
                Total += item.Total;
            }
            ViewBag.GrandTotal = Total;


            //return model
            return View(Cart);
        }

        public ActionResult CartViewPartial()
        {

            //init cart view model

            CartViewModel model = new CartViewModel();

            //get Price and quantiti
            int Quantity = 0;
            decimal Price = 0m;

            //session sprawdza czy iestnieje juz koszyk w sesii 

            if (Session["Cart"] != null)
            {

                //get value from session - sess jest obiektem - zrzutowanie na  CartViewModel
                var list = (List<CartViewModel>)Session["Cart"];
                //get value
                foreach (var item in list)
                {
                    //var Quantity  zawiera sum ilos
                    Quantity += item.Quantity;
                    Price += item.Quantity * item.Price;
                }

            }
            else
            {
                //Ustalamy ilosc i cene na 0
                Quantity = 0;
                Price = 0;
            }
            //set model 
            model.Quantity = Quantity;
            model.Price = Price;

            return PartialView(model);
        }

        //GET: Cart/AddToCartPartial/{id}
        [HttpGet]
        public ActionResult AddToCartPartial(int id)
        {



            //init Model
            List<CartViewModel> Cart = Session["Cart"] as List<CartViewModel> ?? new List<CartViewModel>();
            //Set Session
            //init Cart View Modcel
            CartViewModel model = new CartViewModel();
            //get Product to model 
            //sprawdz czy produkt istnieje
            using (Db db = new Db())
            {
                if (!db.Products.Any(x => x.Id.Equals(id))) return null;
                ProductsDTO product = db.Products.Find(id);
                var productInCart = Cart.FirstOrDefault(x => x.ProductId.Equals(id));
                //jeżeli produkt istnieje to zwiększ jego ilość jak nie to dodaj
                if (productInCart == null)
                {
                    //addProduct 2 cart
                    Cart.Add(new CartViewModel()
                    {
                        //init ViewModel 
                        ProductId = product.Id,
                        ProductName = product.Name,
                        Price = product.Price,
                        Quantity = 1,
                        Image = product.ImageName


                    });
                }
                else
                {
                    //zwieksz jego ilość
                    productInCart.Quantity++;
                }
            }
            //pobieram ilosc i cene i dodaje do modelu
            int Quantity = 0;

            decimal Price = 0m;
            foreach (var itemInCart in Cart)
            {
                Quantity += itemInCart.Quantity;
                Price += itemInCart.Quantity * itemInCart.Price;

            }
            model.Quantity = Quantity;
            model.Price = Price;
            // zapis w sesii \  cart zmieną Cart
            Session["Cart"] = Cart;


            //return Model
            return PartialView("CartViewPartial",model);
        }
        //zwrot danych json
       
        public JsonResult ChangeQuantity(int productid, int? me) {
            //cartView Model list
            List<CartViewModel> Cart = Session["Cart"] as List<CartViewModel>;
            //pobiera cart view model //konkretny produkt z listy w koszyku
            CartViewModel model = Cart.FirstOrDefault(x => x.ProductId.Equals(productid));
            //jeżeli method 1 or 2 or 3
            if(me == 1)
            {
                model.Quantity++;
            }
            else
            {
                if(model.Quantity >= 1)
                {
                    model.Quantity--;
                }
                else
                {
                    //usuniecie produktu z koszyka
                    model.Quantity = 1;
                }
               
            }
            //get total quantity
            int TotalQuantity = 0;
            decimal Total = 0m;
            foreach (var item in Cart)
            {
                Total += item.Total;
                TotalQuantity += item.Quantity;
            }
           
           
            //zwiększamy Quantity

            //////
            ///@TODO  constructor Result 
            var result = new { me = me,qty = model.Quantity, price = model.Price,total = Total, totalQuantity = TotalQuantity };
            //
            return Json(result,JsonRequestBehavior.AllowGet);
        }

        public JsonResult RemoveItem(int productid) {
            string res = "success";
            int TotalItems = 0;
            //init Cart View model 
            List<CartViewModel> Cart = Session["Cart"] as List<CartViewModel>;
            //get Model
            CartViewModel model = Cart.FirstOrDefault(x => x.ProductId.Equals(productid));
            //usuwamy item z koszyka
            if (!Cart.Remove(model))
            {
                res = "Error";
            }
            foreach (var item in Cart)
            {
                TotalItems += item.Quantity;
            }


            //response
            var result = new { response = res, TotalItems };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PaypalPartial()
        {
            List<CartViewModel> Cart = Session["Cart"] as List<CartViewModel>;
            return PartialView(Cart);

        }
       
        [HttpPost]
        public ActionResult PlaceOrder() {
            //pobiera koszyk 
            List<CartViewModel> Cart = Session["Cart"] as List<CartViewModel>;
            //dodaje nowe zamówienie ->jeżeli jest zalogowany użytkownik jeżeli nie to przekierowuje na stronę formularza dodania nowego uzytkownika oraz szczegolow uzytkownika
         
                //redirect ro createAccount
              
            string userName = User.Identity.Name;
            if (string.IsNullOrEmpty(userName))
            { // przekierowanie na stronę sklepu 
              //   return Redirect("~/Pages/Index");
              //bazowy adres strony profilowej 
                return  Redirect("~/Account/CreateAccount");
            }

            //numer zamowienia
            int orderId = 0;
            using (Db db = new Db()) {
                //get OrderId
                OrderDTO orderDTO = new OrderDTO();
                //geet user ID
                var UserId = db.Users.FirstOrDefault(x => x.Email.Equals(userName));
                //SET Dto Order 
                orderDTO.UserId = UserId.Id;
                orderDTO.CreatedD = DateTime.Now;
                db.Orders.Add(orderDTO);
                db.SaveChanges();
                //getOrder Id
                orderId = orderDTO.OrderId;
                //init order details dto
                OrderDetailsDTO orderDetailsDto = new OrderDetailsDTO();
                foreach (var cartItem in Cart) {
                    orderDetailsDto.OrderId = orderId;
                    orderDetailsDto.ProductId = cartItem.ProductId;
                    orderDetailsDto.Quantity = cartItem.Quantity;
                    db.OrdersDetails.Add(orderDetailsDto);
                    db.SaveChanges();
                }
                //Send email to admin with info about new Order
                var client = new SmtpClient("smtp.mailtrap.io", 2525)
                {
                    Credentials = new NetworkCredential("cf2f1012b786c8", "2e684b470d2a06"),
                    EnableSsl = true
                };
                client.Send("shop@example.com", "admin@example.com", "Nowe zamowienie " + orderId + " .", "Nowe zamowienie " + orderId + " .");

                //reset sesion Cart

                Session["Cart"] = null;
                return View();
            }


        }

        /**
        * Akcja typu void nie zwraca nic
        */

    }
}