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
        int id_user, user_type;
        string name_user, cpf_user, address_user, complement, zipcode, phone_user, birth_date, img_user;

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
                    if (userinfo == "401")
                    {
                        RemoveCookie();
                        System.Diagnostics.Debug.WriteLine("Retorno Api: " + userinfo);
                        ViewBag.statusLogin = "Email ou senha invalido!!";
                    }
                    else if (userinfo == "405")
                    {
                        RemoveCookie();
                        System.Diagnostics.Debug.WriteLine("Retorno Api: " + userinfo);
                        ViewBag.statusLogin = "O seu email não foi verificado!";
                    }
                    else if (userinfo == "500")
                    {
                        System.Diagnostics.Debug.WriteLine("Retorno Api: " + userinfo);
                        ViewBag.statusLogin = "Estamos com um problema em nossos servidores, por favor tente mais tarde.";
                    }
                    else if (userinfo == "" || userinfo == " " || userinfo == null)
                    {
                        System.Diagnostics.Debug.WriteLine("Retorno Api: " + userinfo);
                        ViewBag.statusLogin = "Estamos com um problema em nossos servidores, por favor tente mais tarde.";
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