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
        string name_user, cpf_user, address_user, complement, zipcode, phone_user, birth_date, img_user;

        public async Task<ActionResult> Index(string email_user)
        {
            email_user = (string)Session["email_user"];
            string password = string.Empty;
            if (email_user != null)
            {
                ViewBag.getemail = email_user;
                AcProducts ac = new AcProducts();
                ModelState.Clear();
                return View(ac.GetAllCategories());
            }
            else
            {
                HttpCookie reqCookies = Request.Cookies["userInfo"];
                if(reqCookies != null)
                {
                    email_user = reqCookies["User_email"].ToString();
                    password = reqCookies["User_password"].ToString();
                    string userinfo = await Task.Run(() => Login.Authlogin(Cryption.DecryptAES(email_user), Cryption.DecryptAES(password)));
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
                            ViewBag.imguser = "https://www.kauavitorio.com/host-itens/Default_Profile_Image_palacepetz.png";
                        else
                            ViewBag.imguser = img_user;
                        AcProducts ac = new AcProducts();
                        ModelState.Clear();
                        return View(ac.GetAllCategories());
                    }
                }
                else
                {
                    return RedirectToAction("Login", "Usuario");
                }
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