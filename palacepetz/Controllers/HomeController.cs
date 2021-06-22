using Newtonsoft.Json.Linq;
using palacepetz.Dados.Auth;
using palacepetz.Dados.Product;
using palacepetz.Models.products;
using palacepetz.Models.shoppingcart;
using System;
using System.Collections.Generic;
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

            AcProducts ac = new AcProducts();
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

                    if(!CheckEmployee(user_type))
                        return RedirectToAction("Index", "Employee");

                    string resultCart = Dados.ShoppingCart.CartActions.GetCartSize(id_user);
                    if (resultCart != "404" && resultCart != "500")
                    {
                        obj = JObject.Parse(resultCart);
                        int cartSize = (int)obj["length"];
                        ViewBag.cartsize = cartSize;
                    }
                    else
                        ViewBag.cartsize = 0;

                    ModelState.Clear();
                    return View(ac.GetAllPopularProducts(id_user));
                }
            }
            else
            {
                id_user = 0;
                RemoveCookie();
                ViewBag.cartsize = 0;
                ViewBag.img_user = "https://www.kauavitorio.com/host-itens/Default_Profile_Image_palacepetz.png";
                return View(ac.GetAllPopularProducts(id_user));
            }
        }

        public async Task<ActionResult> Products(string filter)
        {
            AcProducts ac = new AcProducts();
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

                    if (!CheckEmployee(user_type))
                        return RedirectToAction("Index", "Employee");

                    string resultCart = Dados.ShoppingCart.CartActions.GetCartSize(id_user);
                    if (resultCart != "404" && resultCart != "500")
                    {
                        obj = JObject.Parse(resultCart);
                        int cartSize = (int)obj["length"];
                        ViewBag.cartsize = cartSize;
                    }
                    else
                        ViewBag.cartsize = 0;

                    ModelState.Clear();
                    int cd_category = 0;
                    string filterEng = null;
                    if (filter == null || filter == "" || filter == " ")
                    {
                        return View(ac.GetAllProducts(id_user));
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
                        return View(ac.Get_AllProducts_With_Category(cd_category, id_user));
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
                        return View(ac.Get_AllProducts_With_PriceFilter(filterEng, id_user));
                    }
                    else if (filter == "Dogs" || filter == "Cats" || filter == "Birds" || filter == "Rabbit" || filter == "Fish" || filter == "Hamster")
                    {
                        return View(ac.Get_AllProducts_With_SpeciesFilter(filter, id_user));
                    }else
                    {
                        return View(ac.Get_AllProducts_With_NameFilter(filter, id_user));
                    }
                }
            }
            else
            {
                RemoveCookie();
                ModelState.Clear();
                ViewBag.img_user = "https://www.kauavitorio.com/host-itens/Default_Profile_Image_palacepetz.png";
                ViewBag.cartsize = 0;
                int cd_category = 0;
                id_user = 0;
                string filterEng = null;
                if (filter == null || filter == "" || filter == " ")
                {
                    return View(ac.GetAllProducts(id_user));
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
                    return View(ac.Get_AllProducts_With_Category(cd_category, id_user));
                }
                else if (filter == "menor-preco" || filter == "maior-preco" || filter == "popular")
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
                    return View(ac.Get_AllProducts_With_PriceFilter(filterEng, id_user));
                }
                else if (filter == "Dogs" || filter == "Cats" || filter == "Birds" || filter == "Rabbit" || filter == "Fish" || filter == "Hamster")
                {
                    return View(ac.Get_AllProducts_With_SpeciesFilter(filter, id_user));
                }
                else
                {
                    return View(ac.Get_AllProducts_With_NameFilter(filter, id_user));
                }
            }
        }

        public async Task<ActionResult> Details(int cd_prod = 0)
        {
            AcProducts ac = new AcProducts();
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

                        if (!CheckEmployee(user_type))
                            return RedirectToAction("Index", "Employee");

                        string resultCart = Dados.ShoppingCart.CartActions.GetCartSize(id_user);
                        if (resultCart != "404" && resultCart != "500")
                        {
                            obj = JObject.Parse(resultCart);
                            int cartSize = (int)obj["length"];
                            ViewBag.cartsize = cartSize;
                        }
                        else
                            ViewBag.cartsize = 0;

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
                            infoProd.product_price = (double)objProds["product_price"];
                            infoProd.description = (string)objProds["description"];
                            infoProd.product_amount = (int)objProds["amount"];
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
                if (cd_prod != 0)
                {
                    ViewBag.img_user = "https://www.kauavitorio.com/host-itens/Default_Profile_Image_palacepetz.png";
                    ViewBag.cartsize = 0;
                    ModelState.Clear();
                    string resultDetails = ac.Get_Product_Details(cd_prod);
                    if (resultDetails != "500" || resultDetails != "204")
                    {
                        System.Diagnostics.Debug.WriteLine("" + resultDetails);
                        JObject objProds = JObject.Parse(resultDetails);
                        DtoProduct infoProd = new DtoProduct();
                        infoProd.id_user = 0;
                        infoProd.cd_prod = (int)objProds["cd_prod"];
                        infoProd.nm_product = (string)objProds["nm_product"];
                        infoProd.product_price = (double)objProds["product_price"];
                        infoProd.description = (string)objProds["description"];
                        infoProd.product_amount = (int)objProds["amount"];
                        infoProd.image_prod = (string)objProds["image_prod"];
                        return View(infoProd);
                    }
                    else
                        return RedirectToAction("Products", "Home");
                }
                else
                    return RedirectToAction("Products", "Home");
            }
        }

        [HttpPost]
        public ActionResult Details(DtoProduct product)
        {
            try
            {
                double unit_price = product.product_price;
                System.Diagnostics.Debug.WriteLine("" + unit_price);
                double totalPrice = unit_price * int.Parse(product.amount);
                System.Diagnostics.Debug.WriteLine("" + product.amount);
                string sub_total = totalPrice + "";
                int resultInsert = Dados.ShoppingCart.CartActions.Insert_New_Product_OnUserCart(product, (totalPrice + "").Replace(",", "."), sub_total.Replace(",", "."));
                if (resultInsert == 201)
                    return RedirectToAction("Products", "Home");
                else if (resultInsert == 409)
                    return RedirectToAction("Index", "Home");
                else
                    return RedirectToAction("Index", "Home");
            }
            catch
            {
                return RedirectToAction("Products", "Home");
            }
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

                    if (!CheckEmployee(user_type))
                        return RedirectToAction("Index", "Employee");

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
                            ViewBag.fullprice = double.Parse(Dados.ShoppingCart.CartActions.GetFullPriceCart(id_user));
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

                    if (!CheckEmployee(user_type))
                        return RedirectToAction("Index", "Employee");

                    string resultCart = Dados.ShoppingCart.CartActions.GetCartSize(id_user);
                    if (resultCart != "404" && resultCart != "500")
                    {
                        obj = JObject.Parse(resultCart);
                        int cartSize = (int)obj["length"];
                        ViewBag.cartsize = cartSize;
                    }
                    else
                        ViewBag.cartsize = 0;

                    List<SelectListItem> cardList = new List<SelectListItem>(Dados.User.Profile.GetCardList(id_user));
                    ViewBag.cardlist = new SelectList(cardList, "Value", "Text");

                    ViewBag.address = address_user;
                    ViewBag.complement = complement;
                    ViewBag.zipcode = zipcode;
                    ViewBag.finalprice = double.Parse(Dados.ShoppingCart.CartActions.GetFullPriceCart(id_user));

                    return View();

                }
            }
            else
            {
                RemoveCookie();
                return RedirectToAction("Login", "Usuario");
            }
        }

        [HttpPost]
        public async Task<ActionResult> FinishPurchase(string finish)
        {
            if(finish == null)
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

                        List<SelectListItem> cardList = new List<SelectListItem>(Dados.User.Profile.GetCardList(id_user));
                        ViewBag.cardlist = new SelectList(cardList, "Value", "Text");

                        ViewBag.address = address_user;
                        ViewBag.complement = complement;
                        ViewBag.zipcode = zipcode;
                        ViewBag.finalprice = double.Parse(Dados.ShoppingCart.CartActions.GetFullPriceCart(id_user));

                        string OrderPrice = "R$" + double.Parse(Dados.ShoppingCart.CartActions.GetFullPriceCart(id_user));
                        if(Request["cardlist"] == null || Request["cardlist"] == "" || Request["cardlist"].Replace(" ", "").Length <= 0)
                        {
                            ViewBag.statusOrderResult = "Não foi possivel completar seu pedido, por favor tente mais tarde.";
                        }
                        else
                        {
                            string cd_card = Request["cardlist"];
                            int result = await Dados.Order.ActionOrder.Finish_Order(id_user, cd_card, OrderPrice, OrderPrice);
                            if (result == 201)
                            {
                                ViewBag.statusOrderResul  = "Necessário selecionar um cartão!!";
                                return RedirectToAction("UserOrders", "Usuario");
                            }
                            else
                            {
                                ViewBag.statusOrderResult = "Não foi possivel completar seu pedido, por favor tente mais tarde.";
                            }
                        }
                    }
                }
                else
                {
                    RemoveCookie();
                    return RedirectToAction("Login", "Usuario");
                }
            }
            return View();
        }

        public async Task<ActionResult> Services()
        {

            AcProducts ac = new AcProducts();
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

                    if (!CheckEmployee(user_type))
                        return RedirectToAction("Index", "Employee");

                    string resultCart = Dados.ShoppingCart.CartActions.GetCartSize(id_user);
                    if (resultCart != "404" && resultCart != "500")
                    {
                        obj = JObject.Parse(resultCart);
                        int cartSize = (int)obj["length"];
                        ViewBag.cartsize = cartSize;
                    }
                    else
                        ViewBag.cartsize = 0;

                    ViewBag.id_user = id_user;

                    ModelState.Clear();
                    return View();
                }
            }
            else
            {
                id_user = 0;
                RemoveCookie();
                ViewBag.cartsize = 0;
                ViewBag.id_user = 0;
                ViewBag.img_user = "https://www.kauavitorio.com/host-itens/Default_Profile_Image_palacepetz.png";
                return View();
            }
        }

        public async Task<ActionResult> HelpCenter()
        {
            return View();
        }

        public async Task<ActionResult> Schedule_Appointment()
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
                userinfo = await Task.Run(() => Dados.Auth.Login.Authlogin(email_user, password));
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

                    if (!CheckEmployee(user_type))
                        return RedirectToAction("Index", "Employee");

                    string resultCart = Dados.ShoppingCart.CartActions.GetCartSize(id_user);
                    if (resultCart != "404" && resultCart != "500")
                    {
                        obj = JObject.Parse(resultCart);
                        int cartSize = (int)obj["length"];
                        ViewBag.cartsize = cartSize;
                    }
                    else
                        ViewBag.cartsize = 0;

                    ViewBag.name_user = name_user;
                    ViewBag.cpf_user = cpf_user;
                    ViewBag.email_user = email_user;
                    ViewBag.street_user = address_user;
                    ViewBag.complement_user = complement;
                    ViewBag.cep_user = zipcode;

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

        public bool CheckEmployee(int user_type)
        {
            if (user_type == 0) return true;
            else return false;
        }
    }

}