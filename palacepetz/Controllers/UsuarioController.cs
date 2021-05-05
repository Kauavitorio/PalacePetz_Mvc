using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace palacepetz.Controllers
{
    public class UsuarioController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(string email, string password)
        {
            string userinfo = await Task.Run(() => Dados.Auth.Login.Authlogin(email, password));
            JObject obj = JObject.Parse(userinfo);
            int id_user = (int)obj["id_user"];
            string name_user = (string)obj["name_user"];
            /*email = (string)obj["email"];
            cpf_user = (string)obj["cpf_user"];
            address_user = (string)obj["address_user"];
            complement = (string)obj["complement"];
            zipcode = (string)obj["zipcode"];
            phone_user = (string)obj["phone_user"];
            birth_date = (string)obj["birth_date"];
            user_type = (int)obj["user_type"];
            img_user = (string)obj["img_user"];*/
            ViewBag.test = "ID: " + id_user + "\nNome Usuario: " + name_user;
            return View();
        }
    }
}