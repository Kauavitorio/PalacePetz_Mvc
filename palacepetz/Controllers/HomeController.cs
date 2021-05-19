using Newtonsoft.Json.Linq;
using palacepetz.Dados.Auth;
using palacepetz.Dados.Product;
using palacepetz.Models.products;
using System;
using System.Collections.Generic;
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
                        return View(ac.GetAllProductsWithCategory(cd_category));
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
                        return View(ac.GetAllProductsWithPriceFilter(filterEng));
                    }
                    else
                    {
                        return View(ac.GetAllProductsWithSpeciesFilter(filter));
                    }
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

        public void logOut(object sender, EventArgs e)
        {
            RemoveCookie();
            RedirectToAction("Login", "Usuario");
            string testa = "testaaa";
            System.Diagnostics.Debug.WriteLine("" + testa);
        }
    }

}