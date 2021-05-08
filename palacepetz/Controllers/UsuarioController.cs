using Newtonsoft.Json.Linq;
using palacepetz.Dados.Auth;
using palacepetz.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace palacepetz.Controllers
{
    public class UsuarioController : Controller
    {
        public async Task<ActionResult> Login()
        {
            string email_user = string.Empty;
            string password = string.Empty;
            HttpCookie reqCookies = Request.Cookies["userInfo"];
            if (reqCookies != null)
            {
                email_user = reqCookies["User_email"].ToString();
                Session["email_user"] = email_user;
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(DtoUser userinfoDto)
        {
            string email = userinfoDto.email;
            string password = userinfoDto.password;
            bool checkboxPassowrd = userinfoDto.rememberPassword;
            string userinfo = await Task.Run(() => Dados.Auth.Login.Authlogin(email, password));
            if (userinfo == "Email ou senha inválido!" || userinfo == " " || userinfo == "")
            {
                if (Request.Cookies["userInfo"] != null)
                {
                    var c = new HttpCookie("userInfo");
                    c.Expires = DateTime.Now.AddSeconds(1);
                    Response.Cookies.Add(c);
                }
                ViewBag.statusLogin = "Email ou senha invalido!!";
            } else if (userinfo == "error") {
                ViewBag.statusLogin = "Estamos com um problema em nossos servidores, por favor tente mais tarde.";
            } else if (userinfo == "Erro ao enviar a requisição, por favor, tente mais tarde!")
            {
                ViewBag.statusLogin = userinfo;
            } else {
                JObject obj = JObject.Parse(userinfo);
                string email_user = (string)obj["email"];
                if (checkboxPassowrd)
                {
                    HttpCookie userInfo = new HttpCookie("userInfo");
                    userInfo["User_email"] = Cryption.EncryptAES(email_user);
                    userInfo["User_password"] = Cryption.EncryptAES(password);
                    userInfo.Expires.Add(new TimeSpan(0, 1, 0));
                    Response.Cookies.Add(userInfo);
                }
                Session["email_user"] = email_user;
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public ActionResult CreateAccount()
        {
            string email_user = string.Empty;
            string password = string.Empty;
            HttpCookie reqCookies = Request.Cookies["userInfo"];
            if (reqCookies != null)
            {
                return RedirectToAction("Login");
            }
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreateAccount(DtoUser userinfo)
        {
            //return Content("<script language='javascript' type='text/javascript'>alert('Olá, seu usuário foi cadastrado com sucesso :). Mandamos um email de verificação para o email cadastrado, por favor confirmar no email antes de se logar');</script>");

            ViewBag.StatusCreateAccount = "Olá, seu usuário foi cadastrado com sucesso :). Mandamos um email de verificação para o email cadastrado, por favor confirmar no email antes de se logar";

            if (!ModelState.IsValid)
            {
                return View(userinfo);
            }
            else
            {
                string firstname = userinfo.Firstname_user;
                string lastname = userinfo.Lastname_user;
                string name_user = firstname + " " + lastname;
                userinfo.name_user = name_user;
                int createstatus = await Task.Run(() => Dados.Auth.Login.AuthRegister(userinfo));
                if (createstatus == 201)
                {
                    return RedirectToAction("Login");
                }
                else
                {
                    ViewBag.StatusCreateAccount = "Estamos com um problema em nossos servidores, por favor tente mais tarde.";
                }
            }
            return View();
        }

        public ActionResult RecuperarSenha()
        {

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> RecuperarSenha(string email)
        {
            if (email == null || email == " " || email == "")
            {
                ViewBag.statusRecoverPassowrd = "O campo email, não pode estar vazio.";
            }
            else
            {
                string result = await Dados.Auth.Login.RecoverPassword(email);
                ViewBag.statusRecoverPassowrd = result;
            }

            return View();
        }

        public ActionResult SetNewPassword()
        {
            return View();
        }
    }
}