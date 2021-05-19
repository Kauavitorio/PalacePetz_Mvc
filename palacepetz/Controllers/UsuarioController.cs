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
        DtoUser dto = new DtoUser();
        public ActionResult Login(int logoutid = 0)
        {
            if(logoutid != 0)
            {
                RemoveCookie();
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
                Response.Cache.SetNoStore();
                Session["email_user"] = null;
                return View();
            }
            else
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

            
        }

        [HttpPost]
        public async Task<ActionResult> Login(DtoUser userinfoDto)
        {
            string email = userinfoDto.email;
            string password = userinfoDto.password;
            bool checkboxPassowrd = userinfoDto.rememberPassword;
            string userinfo = await Task.Run(() => Dados.Auth.Login.Authlogin(email, password));
            if (userinfo == "401"){
                RemoveCookie();
                System.Diagnostics.Debug.WriteLine("Retorno Api: " + userinfo);
                ViewBag.statusLogin = "Email ou senha invalido!!";
            } else if (userinfo == "405"){
                RemoveCookie();
                System.Diagnostics.Debug.WriteLine("Retorno Api: " + userinfo);
                ViewBag.statusLogin = "O seu email não foi verificado!";
            }else if (userinfo == "500"){
                RemoveCookie();
                System.Diagnostics.Debug.WriteLine("Retorno Api: " + userinfo);
                ViewBag.statusLogin = "Estamos com um problema em nossos servidores, por favor tente mais tarde.";
            }else if(userinfo == "" || userinfo == " " || userinfo == null){
                RemoveCookie();
                System.Diagnostics.Debug.WriteLine("Retorno Api: " + userinfo);
                ViewBag.statusLogin = "Estamos com um problema em nossos servidores, por favor tente mais tarde.";
            }else{
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
                Session["password_user"] = password;
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
                    return RedirectToAction("Login");
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
                return View();
            }
            else
            {
                int resultRequest = await Dados.Auth.Login.RecoverPassword(email);
                if(resultRequest == 200)
                {
                    ViewBag.statusRecoverPassowrd = "Enviamos um e-mail para você redefinir sua senha";
                    return View();
                }
                else
                {
                    ViewBag.statusRecoverPassowrd = "Estamos com um problema em nossos servidores, por favor tente mais tarde.";
                    return View();
                }
            }
        }

        public ActionResult SetNewPassword(string requestId, int userId = 0)
        {
            if (requestId.Substring(0, 4) != "pswd" || requestId.Substring(requestId.Length - 2) != "p0")
                return RedirectToAction("Index", "Home");
            else
            {
                Session["userId_password"] = userId;
                Session["verify_id"] = requestId;
                return View(dto);
            }
        }

        [HttpPost]
        public async Task<ActionResult> SetNewPassword(DtoUser userinfo)
        {
            if (!ModelState.IsValid)
            {
                return View(userinfo);
            }
            else
            {
                int idUserRequest = (int)Session["userId_password"];
                string verify_id = (string)Session["verify_id"];
                int resultChange = await Dados.Auth.Login.ChangePassword(verify_id, idUserRequest, userinfo.password);
                if(resultChange == 200)
                {
                    ViewBag.statusRecoverPassowrd = "Sua senha foi alterada, volte a aplicaçao e faça login";
                }else if(resultChange == 405)
                {
                    ViewBag.statusRecoverPassowrd = "Você não está autorizado a alterar sua senha, entre em contato com um funcionário!";
                }else if(resultChange == 401)
                {
                    ViewBag.statusRecoverPassowrd = "Seu e-mail não foi verificado";
                }
                else
                {
                    ViewBag.statusRecoverPassowrd = "Estamos com um problema em nossos servidores, por favor tente mais tarde.";
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