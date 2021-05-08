using Newtonsoft.Json.Linq;
using palacepetz.Dados.Auth;
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
        public async Task<ActionResult> Index(string email_user)
        {
            email_user = (string)Session["email_user"];
            string password = string.Empty;
            if (email_user != null)
            {
                ViewBag.getemail = email_user;
            }
            else
            {
                HttpCookie reqCookies = Request.Cookies["userInfo"];
                if(reqCookies != null)
                {
                    email_user = reqCookies["User_email"].ToString();
                    password = reqCookies["User_password"].ToString();
                    string userinfo = await Task.Run(() => Login.Authlogin(Cryption.DecryptAES(email_user), Cryption.DecryptAES(password)));
                    if (userinfo == "error" || userinfo == "EmailOrPassword")
                    {
                        ViewBag.statusLogin = "Email ou senha invalido";
                    }
                    else
                    {
                        JObject obj = JObject.Parse(userinfo);
                        int id_user = (int)obj["id_user"];
                        string name_user = (string)obj["name_user"];
                        string cpf_user = (string)obj["cpf_user"];
                        string address_user = (string)obj["address_user"];
                        string complement = (string)obj["complement"];
                        string zipcode = (string)obj["zipcode"];
                        string phone_user = (string)obj["phone_user"];
                        string birth_date = (string)obj["birth_date"];
                        int user_type = (int)obj["user_type"];
                        string img_user = (string)obj["img_user"];


                        /*if (Request.Cookies["userInfo"] != null)
                        {
                            var c = new HttpCookie("userInfo");
                            c.Expires = DateTime.Now.AddSeconds(1);
                            Response.Cookies.Add(c);
                        }*/
                    }
                }
                else
                {
                    return RedirectToAction("Login", "Usuario");
                }
            }
            return View();
        }
    }
}