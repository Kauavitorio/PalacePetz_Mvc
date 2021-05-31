using Newtonsoft.Json.Linq;
using palacepetz.Dados.Auth;
using palacepetz.Dados.Product;
using palacepetz.Models.products;
using palacepetz.Models.shoppingcart;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace palacepetz.Controllers
{
    public class HomeController : Controller
    {
        int id_user, user_type;
        private string name_user, email_user, cpf_user, address_user, complement, zipcode, phone_user, birth_date, img_user, password;
        private int prodPrice;

        public async Task<ActionResult> Index(string email_user = null, string password = null)
        {
            HttpCookie reqCookies = Request.Cookies["userInfo"];
            if (reqCookies != null)
            {
                email_user = Cryption.DecryptAES(reqCookies["User_email"].ToString());
                password = Cryption.DecryptAES(reqCookies["User_password"].ToString());
            }
            else
            {
                email_user = (string)Session["email_user"];
                password = (string)Session["password_user"];
            }
            if(email_user != null && email_user != "")
            {
                string userinfo = await Task.Run(() => Login.Authlogin(email_user, password));
                if (userinfo == "401" || userinfo == "405" || userinfo == "500" || userinfo == "" || userinfo == " " || userinfo == null)
                {
                    RemoveCookie();
                    return RedirectToAction("Login", "Usuario");
                }
                else
                {
                    JObject obj = JObject.Parse(userinfo);
                    id_user = (int)obj["id_user"];
                    name_user = (string)obj["name_user"];
                    cpf_user = (string)obj["cpf_user"];
                    address_user = (string)obj["address_user"];
                    complement = (string)obj["complement"];
                    zipcode = (string)obj["zipcode"];
                    phone_user = (string)obj["phone_user"];
                    birth_date = (string)obj["birth_date"];
                    user_type = (int)obj["user_type"];
                    img_user = (string)obj["img_user"];
                    if (img_user == null || img_user == " ")
                        ViewBag.img_user = "https://www.kauavitorio.com/host-itens/Default_Profile_Image_palacepetz.png";
                    else
                        ViewBag.img_user = img_user;

                    string resultCart = Dados.ShoppingCart.CartActions.GetCartSize(id_user);
                    if (resultCart != "404" && resultCart != "500")
                    {
                        obj = JObject.Parse(resultCart);
                        int cartSize = (int)obj["length"];
                        ViewBag.cartsize = cartSize;
                    }
                    else
                        ViewBag.cartsize = 0;

                    AcProducts ac = new AcProducts();
                    ModelState.Clear();
                    return View(ac.GetAllPopularProducts());
                }
            }
            else
            {
                RemoveCookie();
                return RedirectToAction("Login", "Usuario");
            }
        }

        public async Task<ActionResult> Products(string filter)
        {
            HttpCookie reqCookies = Request.Cookies["userInfo"];
            if (reqCookies != null)
            {
                email_user = Cryption.DecryptAES(reqCookies["User_email"].ToString());
                password = Cryption.DecryptAES(reqCookies["User_password"].ToString());
            }
            else
            {
                email_user = (string)Session["email_user"];
                password = (string)Session["password_user"];
            }
            if (email_user != null && email_user != "")
            {
                string userinfo = await Task.Run(() => Login.Authlogin(email_user, password));
                if (userinfo == "401" || userinfo == "405" || userinfo == "500" || userinfo == "" || userinfo == " " || userinfo == null)
                {
                    RemoveCookie();
                    return RedirectToAction("Login", "Usuario");
                }
                else
                {
                    JObject obj = JObject.Parse(userinfo);
                    id_user = (int)obj["id_user"];
                    name_user = (string)obj["name_user"];
                    cpf_user = (string)obj["cpf_user"];
                    address_user = (string)obj["address_user"];
                    complement = (string)obj["complement"];
                    zipcode = (string)obj["zipcode"];
                    phone_user = (string)obj["phone_user"];
                    birth_date = (string)obj["birth_date"];
                    user_type = (int)obj["user_type"];
                    img_user = (string)obj["img_user"];
                    if (img_user == null || img_user == " ")
                        ViewBag.img_user = "https://www.kauavitorio.com/host-itens/Default_Profile_Image_palacepetz.png";
                    else
                        ViewBag.img_user = img_user;

                    string resultCart = Dados.ShoppingCart.CartActions.GetCartSize(id_user);
                    if (resultCart != "404" && resultCart != "500")
                    {
                        obj = JObject.Parse(resultCart);
                        int cartSize = (int)obj["length"];
                        ViewBag.cartsize = cartSize;
                    }
                    else
                        ViewBag.cartsize = 0;

                    AcProducts ac = new AcProducts();
                    ModelState.Clear();
                    int cd_category = 0;
                    string filterEng = null;
                    if (filter == null || filter == "" || filter == " ")
                    {
                        return View(ac.GetAllProducts());
                    }
                    else if (filter == "Alimentos" || filter == "Medicamentos" || filter == "Acessorios" || filter == "Estetica")
                    {
                        switch (filter)
                        {
                            case "Alimentos":
                                cd_category = 4;
                                break;
                            case "Medicamentos":
                                cd_category = 14;
                                break;
                            case "Acessorios":
                                cd_category = 24;
                                break;
                            case "Estetica":
                                cd_category = 34;
                                break;
                        }
                        return View(ac.Get_AllProducts_With_Category(cd_category));
                    }else if(filter == "menor-preco" || filter == "maior-preco" || filter == "popular")
                    {
                        switch (filter)
                        {
                            case "menor-preco":
                                filterEng = "lowestprice";
                                break;
                            case "maior-preco":
                                filterEng = "biggestprice";
                                break;
                            case "popular":
                                filterEng = "popular";
                                break;
                        }
                        return View(ac.Get_AllProducts_With_PriceFilter(filterEng));
                    }
                    else
                    {
                        return View(ac.Get_AllProducts_With_SpeciesFilter(filter));
                    }
                }
            }
            else
            {
                RemoveCookie();
                return RedirectToAction("Login", "Usuario");
            }
        }

        public async Task<ActionResult> Details(int cd_prod = 0)
        {
            string userinfo;
            HttpCookie reqCookies = Request.Cookies["userInfo"];
            if (reqCookies != null)
            {
                email_user = Cryption.DecryptAES(reqCookies["User_email"].ToString());
                password = Cryption.DecryptAES(reqCookies["User_password"].ToString());
            }
            else
            {
                email_user = (string)Session["email_user"];
                password = (string)Session["password_user"];
            }
            if (email_user != null && email_user != "")
            {
                if(cd_prod != 0)
                {
                    userinfo = await Task.Run(() => Login.Authlogin(email_user, password));
                    if (userinfo == "401" || userinfo == "405" || userinfo == "500" || userinfo == "" || userinfo == " " || userinfo == null)
                    {
                        RemoveCookie();
                        return RedirectToAction("Login", "Usuario");
                    }
                    else
                    {
                        JObject obj = JObject.Parse(userinfo);
                        id_user = (int)obj["id_user"];
                        name_user = (string)obj["name_user"];
                        cpf_user = (string)obj["cpf_user"];
                        address_user = (string)obj["address_user"];
                        complement = (string)obj["complement"];
                        zipcode = (string)obj["zipcode"];
                        phone_user = (string)obj["phone_user"];
                        birth_date = (string)obj["birth_date"];
                        user_type = (int)obj["user_type"];
                        img_user = (string)obj["img_user"];
                        if (img_user == null || img_user == " ")
                            ViewBag.img_user = "https://www.kauavitorio.com/host-itens/Default_Profile_Image_palacepetz.png";
                        else
                            ViewBag.img_user = img_user;

                        string resultCart = Dados.ShoppingCart.CartActions.GetCartSize(id_user);
                        if (resultCart != "404" && resultCart != "500")
                        {
                            obj = JObject.Parse(resultCart);
                            int cartSize = (int)obj["length"];
                            ViewBag.cartsize = cartSize;
                        }
                        else
                            ViewBag.cartsize = 0;

                        AcProducts ac = new AcProducts();
                        ModelState.Clear();
                        string resultDetails = ac.Get_Product_Details(cd_prod);
                        if(resultDetails != "500" || resultDetails != "204")
                        {
                            System.Diagnostics.Debug.WriteLine("" + resultDetails);
                            JObject objProds = JObject.Parse(resultDetails);
                            DtoProduct infoProd = new DtoProduct();
                            JObject objUser = JObject.Parse(userinfo);
                            infoProd.id_user = (int)objUser["id_user"];
                            infoProd.cd_prod = (int)objProds["cd_prod"];
                            infoProd.nm_product = (string)objProds["nm_product"];
                            infoProd.product_price = (float)objProds["product_price"];
                            infoProd.description = (string)objProds["description"];
                            infoProd.image_prod = (string)objProds["image_prod"];
                            return View(infoProd);
                        }
                        else
                            return RedirectToAction("Products", "Home");
                    }
                }
                else
                    return RedirectToAction("Products", "Home");
            }
            else
            {
                RemoveCookie();
                return RedirectToAction("Login", "Usuario");
            }
        }

        [HttpPost]
        public ActionResult Details(DtoProduct product)
        {
            float unit_price = product.product_price;
            System.Diagnostics.Debug.WriteLine("" + unit_price);
            float totalPrice =  unit_price * int.Parse(product.amount);
            System.Diagnostics.Debug.WriteLine("" + product.amount);
            float sub_total = totalPrice;
            int resultInsert = Dados.ShoppingCart.CartActions.Insert_New_Product_OnUserCart(product, totalPrice, sub_total);
            if (resultInsert == 201)
                return RedirectToAction("Products", "Home");
            else if (resultInsert == 409)
                return RedirectToAction("Index", "Home");
            else
                return RedirectToAction("Index", "Home");
        }

        public async Task<ActionResult> ShoppingCart(int cd_cart = 0, int id_user = 0)
        {
            string userinfo;
            HttpCookie reqCookies = Request.Cookies["userInfo"];
            if (reqCookies != null)
            {
                email_user = Cryption.DecryptAES(reqCookies["User_email"].ToString());
                password = Cryption.DecryptAES(reqCookies["User_password"].ToString());
            }
            else
            {
                email_user = (string)Session["email_user"];
                password = (string)Session["password_user"];
            }
            if (email_user != null && email_user != "")
            {
                userinfo = await Task.Run(() => Login.Authlogin(email_user, password));
                if (userinfo == "401" || userinfo == "405" || userinfo == "500" || userinfo == "" || userinfo == " " || userinfo == null)
                {
                    RemoveCookie();
                    return RedirectToAction("Login", "Usuario");
                }
                else
                {
                    JObject obj = JObject.Parse(userinfo);
                    id_user = (int)obj["id_user"];
                    name_user = (string)obj["name_user"];
                    cpf_user = (string)obj["cpf_user"];
                    address_user = (string)obj["address_user"];
                    complement = (string)obj["complement"];
                    zipcode = (string)obj["zipcode"];
                    phone_user = (string)obj["phone_user"];
                    birth_date = (string)obj["birth_date"];
                    user_type = (int)obj["user_type"];
                    img_user = (string)obj["img_user"];
                    if (img_user == null || img_user == " ")
                        ViewBag.img_user = "https://www.kauavitorio.com/host-itens/Default_Profile_Image_palacepetz.png";
                    else
                        ViewBag.img_user = img_user;

                    if (cd_cart == 0)
                    {
                        string resultCart = Dados.ShoppingCart.CartActions.GetCartSize(id_user);
                        if (resultCart != "404" && resultCart != "500")
                        {
                            obj = JObject.Parse(resultCart);
                            int cartSize = (int)obj["length"];
                            ViewBag.cartsize = cartSize;
                        }
                        else
                            ViewBag.cartsize = 0;

                        List<DtoShoppingCart> shoppingList = new List<DtoShoppingCart>();
                        shoppingList = Dados.ShoppingCart.CartActions.Get_user_cart(id_user);

                        if(shoppingList == null)
                            return RedirectToAction("Products", "Home");
                        else
                        {
                            ViewBag.fullprice = float.Parse(Dados.ShoppingCart.CartActions.GetFullPriceCart(id_user));
                            return View(shoppingList);
                        }
                    }
                    else
                    {
                        string resultDelete = Dados.ShoppingCart.CartActions.Remove_item_from_cart(cd_cart, id_user);
                        if(resultDelete == "200")
                            return RedirectToAction("ShoppingCart", "Home");
                        else
                            return RedirectToAction("Products", "Home");
                    }
                        
                }
            }
            else
            {
                RemoveCookie();
                return RedirectToAction("Login", "Usuario");
            }
        }

        public async Task<ActionResult> FinishPurchase()
        {
            string userinfo;
            HttpCookie reqCookies = Request.Cookies["userInfo"];
            if (reqCookies != null)
            {
                email_user = Cryption.DecryptAES(reqCookies["User_email"].ToString());
                password = Cryption.DecryptAES(reqCookies["User_password"].ToString());
            }
            else
            {
                email_user = (string)Session["email_user"];
                password = (string)Session["password_user"];
            }
            if (email_user != null && email_user != "")
            {
                userinfo = await Task.Run(() => Login.Authlogin(email_user, password));
                if (userinfo == "401" || userinfo == "405" || userinfo == "500" || userinfo == "" || userinfo == " " || userinfo == null)
                {
                    RemoveCookie();
                    return RedirectToAction("Login", "Usuario");
                }
                else
                {
                    JObject obj = JObject.Parse(userinfo);
                    id_user = (int)obj["id_user"];
                    name_user = (string)obj["name_user"];
                    cpf_user = (string)obj["cpf_user"];
                    address_user = (string)obj["address_user"];
                    complement = (string)obj["complement"];
                    zipcode = (string)obj["zipcode"];
                    phone_user = (string)obj["phone_user"];
                    birth_date = (string)obj["birth_date"];
                    user_type = (int)obj["user_type"];
                    img_user = (string)obj["img_user"];
                    if (img_user == null || img_user == " ")
                        ViewBag.img_user = "https://www.kauavitorio.com/host-itens/Default_Profile_Image_palacepetz.png";
                    else
                        ViewBag.img_user = img_user;

                    string resultCart = Dados.ShoppingCart.CartActions.GetCartSize(id_user);
                    if (resultCart != "404" && resultCart != "500")
                    {
                        obj = JObject.Parse(resultCart);
                        int cartSize = (int)obj["length"];
                        ViewBag.cartsize = cartSize;
                    }
                    else
                        ViewBag.cartsize = 0;

                    ViewBag.address = address_user;
                    ViewBag.complement = complement;
                    ViewBag.zipcode = zipcode;
                    ViewBag.finalprice = float.Parse(Dados.ShoppingCart.CartActions.GetFullPriceCart(id_user));

                    return View();

                }
            }
            else
            {
                RemoveCookie();
                return RedirectToAction("Login", "Usuario");
            }
        }

        public void RemoveCookie()
        {
            if (Request.Cookies["userInfo"] != null)
            {
                var c = new HttpCookie("userInfo");
                c.Expires = DateTime.Now.AddSeconds(1);
                Response.Cookies.Add(c);
            }
        }

    }

}