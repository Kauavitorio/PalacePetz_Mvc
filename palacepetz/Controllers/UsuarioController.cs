using Newtonsoft.Json.Linq;
using palacepetz.Dados.Auth;
using palacepetz.Models.Cards;
using palacepetz.Models.User;
using System;
using System.Collections.Generic;

using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace palacepetz.Controllers
{
    public class UsuarioController : Controller
    {
        int id_user, user_type;
        private string name_user, email_user, cpf_user, address_user, complement, zipcode, phone_user, birth_date, img_user, password;

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
            }else if (!Dados.User.Cpf_actions.IsValid(userinfo.cpf_user.ToString()))
            {
                ViewBag.StatusCreateAccount = "CPF informado é invalido!!";
                return View();
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
        }

        public ActionResult RecuperarSenha()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RecuperarSenha(string email)
        {
            if (email == null || email == " " || email == "")
            {
                ViewBag.statusRecoverPassowrd = "O campo email, não pode estar vazio.";
                return View();
            }
            else
            {
                int resultRequest = Dados.Auth.Login.RecoverPassword(email);
                if (resultRequest == 200)
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
        public ActionResult SetNewPassword(DtoUser userinfo)
        {
            if (!ModelState.IsValid)
            {
                return View(userinfo);
            }
            else
            {
                int idUserRequest = (int)Session["userId_password"];
                string verify_id = (string)Session["verify_id"];
                int resultChange = Dados.Auth.Login.ChangePassword(verify_id, idUserRequest, userinfo.password);
                if (resultChange == 200)
                {
                    ViewBag.statusRecoverPassowrd = "Sua senha foi alterada, volte a aplicaçao e faça login";
                }
                else if (resultChange == 405)
                {
                    ViewBag.statusRecoverPassowrd = "Você não está autorizado a alterar sua senha, entre em contato com um funcionário!";
                }
                else if (resultChange == 401)
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

        public async Task<ActionResult> AddressRegistration()
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
        public async Task<ActionResult> AddressRegistration(DtoUser userinfoRegister)
        {
            string zipcode_register = userinfoRegister.zipcode;
            string street = userinfoRegister.address_user;
            string number = userinfoRegister.number;
            string complement = userinfoRegister.complement;
            string userinfo;
            if (zipcode_register == null || zipcode_register.Replace("\\s", "").Length <= 0)
                return RedirectToAction("AddressRegistration", "Usuario");
            else
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
                    userinfo = await Task.Run(() => Dados.Auth.Login.Authlogin(email_user, password));
                    if (userinfo == "401" || userinfo == "405" || userinfo == "500" || userinfo == "" || userinfo == " " || userinfo == null)
                    {
                        RemoveCookie();
                        return RedirectToAction("Login", "Usuario");
                    }
                    else
                    {
                        JObject obj = JObject.Parse(userinfo);
                        int id_userRegister = (int)obj["id_user"];


                        string address = street + ", " + number;
                        int result = Dados.User.Profile.updateAddress(address, complement, zipcode_register, id_userRegister);
                        if (result == 202)
                            return RedirectToAction("Index", "Home");
                        else if (result == 400)
                            return RedirectToAction("Login", "Usuario");
                        else
                            return RedirectToAction("Index", "Home");

                    }
                }
                else
                {
                    RemoveCookie();
                    return RedirectToAction("Login", "Usuario");
                }
            }
        }

        public new async Task<ActionResult> Profile()
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
                else {
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
                    if (resultCart != "404" && resultCart != "500") {
                        obj = JObject.Parse(resultCart);
                        int cartSize = (int)obj["length"];
                        ViewBag.cartsize = cartSize;
                    } else
                        ViewBag.cartsize = 0;

                    string[] FullNameUser = name_user.Split(' ');
                    ViewBag.Firstname_user = FullNameUser[0];
                    ViewBag.Lastname_user = FullNameUser[1];
                    ViewBag.cpf_user = cpf_user;
                    ViewBag.address_user = address_user;
                    ViewBag.complement = complement;
                    ViewBag.zipcode = zipcode;

                    return View();

                }
            }
            else
            {
                RemoveCookie();
                return RedirectToAction("Login", "Usuario");
            }
        }

        public async Task<ActionResult> EditProfile()
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

                    string resultCart = Dados.ShoppingCart.CartActions.GetCartSize(id_user);
                    if (resultCart != "404" && resultCart != "500")
                    {
                        obj = JObject.Parse(resultCart);
                        int cartSize = (int)obj["length"];
                        ViewBag.cartsize = cartSize;
                    }
                    else
                        ViewBag.cartsize = 0;

                    string[] FullNameUser = name_user.Split(' ');
                    DtoUser userinfoSet = new DtoUser();
                    userinfoSet.Firstname_user = FullNameUser[0];
                    userinfoSet.Lastname_user = FullNameUser[1];
                    userinfoSet.cpf_user = cpf_user;
                    userinfoSet.address_user = address_user;
                    userinfoSet.phone_user = phone_user;
                    userinfoSet.birth_date = birth_date;

                    return View(userinfoSet);

                }
            }
            else
            {
                RemoveCookie();
                return RedirectToAction("Login", "Usuario");
            }
        }

        [HttpPost]
        public async Task<ActionResult> EditProfile(HttpPostedFileBase file, DtoUser userinformation)
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

                    string resultCart = Dados.ShoppingCart.CartActions.GetCartSize(id_user);
                    if (resultCart != "404" && resultCart != "500")
                    {
                        obj = JObject.Parse(resultCart);
                        int cartSize = (int)obj["length"];
                        ViewBag.cartsize = cartSize;
                    }
                    else
                        ViewBag.cartsize = 0;

                    userinformation.address_user = address_user;
                    userinformation.complement = complement;
                    userinformation.zipcode = zipcode;

                    try
                    {
                        if (file != null)
                        {
                            FileStream stream;
                            if (file.ContentLength > 0)
                            {
                                string path = Path.Combine(Server.MapPath("~/UploadedFiles"), Path.GetFileName(file.FileName));
                                file.SaveAs(path);
                                stream = new FileStream(Path.Combine(path), FileMode.Open);
                                int updateUserResult = await Task.Run(() => Dados.User.Profile.UpdateUserProfileWithImage(stream, id_user, userinformation));
                                if (updateUserResult == 200)
                                {
                                    return RedirectToAction("Profile", "Usuario");
                                }
                                else
                                {
                                    return RedirectToAction("Index", "Home");
                                }

                            }
                            return View();
                        }
                        else
                        {
                            int updateUserResult = await Task.Run(() => Dados.User.Profile.UpdateUserProfile(userinformation, id_user));
                            if(updateUserResult == 200)
                            {
                                return RedirectToAction("Profile", "Usuario");
                            }
                            else
                            {
                                return RedirectToAction("Index", "Home");
                            }
                        }
                        
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine("" + ex);
                        return View();
                    }

                }
            }
            else
            {
                RemoveCookie();
                return RedirectToAction("Login", "Usuario");
            }
        }

        public async Task<ActionResult> RegisterCard()
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

        [HttpPost]
        public async Task<ActionResult> RegisterCard(DtoCards cardInfo)
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

                    string number_card = cardInfo.number_card;
                    int cvv_card = cardInfo.cvv_card;
                    string nmUser_card = cardInfo.nmUser_card;
                    string shelflife_card = cardInfo.shelflife_card;
                    string flag_card = cardInfo.flag_card;

                    if (number_card == null || number_card.Replace(" ", "") == "" || number_card == " " || number_card.Length < 18 || cvv_card == 0 || nmUser_card == null || nmUser_card.Replace(" ", "") == "" || shelflife_card.Replace(" ", "") == "" || shelflife_card.Length < 7 || shelflife_card == null || flag_card == null)
                    {
                        ViewBag.errorregisterCard = "Verifique se os campos estão preenchidos corretamente.";
                        return View();
                    }
                    else
                    {
                        int result = Dados.Cards.CardAction.registerCard(id_user, number_card, cvv_card, nmUser_card, shelflife_card, flag_card);
                        if(result == 409)
                        {
                            ViewBag.errorregisterCard = "Cartão já cadastrado.";
                            return View();
                        }
                        else if(result == 406)
                        {
                            ViewBag.errorregisterCard = "O nome informado no cartão não é permitido.";
                            return View();
                        }
                        else if(result == 201)
                            return RedirectToAction("Index", "Home");
                        else
                        {
                            ViewBag.errorregisterCard = "Estamos com problemas em nossos servidores, tente novamente mais tarde.";
                            return View();
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