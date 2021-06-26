using Newtonsoft.Json.Linq;
using palacepetz.Dados.Auth;
using palacepetz.Models.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace palacepetz.Controllers
{
    public class ManagerController : Controller
    {
        int id_user, user_type;
        private string name_user, email_user, cpf_user, address_user, complement, zipcode, phone_user, birth_date, img_user, password;
        private int prodPrice;

        // GET: Manager
        public async Task<ActionResult> RegisterEmployee()
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

                    if (user_type == 0)
                        return RedirectToAction("Index", "Home");
                    else if(user_type == 1 || user_type == 2)
                        return RedirectToAction("Index", "Home");
                    else
                        return View();
                }
            }
            else
                return RedirectToAction("Login", "Usuario");
        }

        [HttpPost]
        public async Task<ActionResult> RegisterEmployee(DtoEmployee employeeInfo)
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
                    user_type = (int)obj["user_type"];

                    if (user_type == 0)
                        return RedirectToAction("Index", "Home");
                    else if(user_type == 1 || user_type == 2)
                        return RedirectToAction("Index", "Home");
                    else
                    {
                        employeeInfo.user_type = int.Parse(Request["employee_type_droplist"]);
                        if (employeeInfo.user_type == 999)
                        {
                            ViewBag.insert_status = "Selecione o tipo de funcionario!!";

                        }
                        else if (!Dados.User.Cpf_actions.IsValid(employeeInfo.cpf_user.ToString()))
                        {
                            ViewBag.insert_status = "CPF informado é invalido!!";
                        }
                        else
                        {
                            if (employeeInfo.num_crmv == null || employeeInfo.num_crmv.Length <= 0)
                                employeeInfo.num_crmv = " ";

                            int resultInsert = Dados.Manager.ManagerActions.Register_New_Employee(employeeInfo, id_user);
                            switch (resultInsert)
                            {
                                case 201: return RedirectToAction("EmployeeList", "Manager");

                                case 406:
                                    ViewBag.insert_status = "O nome do inserido é impróprio para registro";
                                    break;
                                case 401: return RedirectToAction("Login", "Usuario");

                                case 409:
                                    ViewBag.insert_status = "Este funcionário já está registrado!!";
                                    break;
                                case 500:
                                    ViewBag.insert_status = "Estamos com problema em nossos servidores.\nTente Novamente mais tarde";
                                    break;
                            }
                        }
                        return View();
                    }
                }
            }
            else
                return RedirectToAction("Login", "Usuario");
        }

        public async Task<ActionResult> EmployeeList()
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
                    user_type = (int)obj["user_type"];

                    if (user_type == 3)
                        return View(Dados.Employees.EmployeeActions.GetAllEmployee(id_user));
                    else
                        return RedirectToAction("Index", "Home");
                }
            }
            else
                return RedirectToAction("Login", "Usuario");
        }

        public async Task<ActionResult> EditEmployee(int id_employee)
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

                    if (user_type == 3)
                        return View(Dados.Manager.ManagerActions.GetEmployeeInfo(id_employee));
                    else
                        return RedirectToAction("Index", "Home");
                }
            }
            else
                return RedirectToAction("Login", "Usuario");
        }

        [HttpPost]
        public async Task<ActionResult> EditEmployee(DtoEmployee employeeInfo)
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
                    user_type = (int)obj["user_type"];

                    if (user_type == 3){
                        employeeInfo.user_type = int.Parse(Request["employee_type_droplist"]);
                        if (employeeInfo.user_type == 999)
                            ViewBag.insert_status = "Selecione o tipo de funcionario!!";

                        else if (!Dados.User.Cpf_actions.IsValid(employeeInfo.cpf_user.ToString()))
                            ViewBag.insert_status = "CPF informado é invalido!!";

                        else if(employeeInfo.password == null || employeeInfo.password.Length <= 8 || employeeInfo.password.Replace(" ", "") == "")
                            ViewBag.insert_status = "Selecione a senha do funcionario!!\nMínimo de caracteres 8";

                        else
                        {

                            if (employeeInfo.num_crmv == null || employeeInfo.num_crmv.Length <= 0)
                                employeeInfo.num_crmv = " ";

                            int resultUpdate = Dados.Manager.ManagerActions.Edit_Employee(employeeInfo, id_user);
                            switch (resultUpdate)
                            {
                                case 200: return RedirectToAction("EmployeeList", "Manager");

                                case 406:
                                    ViewBag.insert_status = "O nome do inserido é impróprio para edição.";
                                    break;
                                case 401: return RedirectToAction("Login", "Usuario");

                                case 409:
                                    ViewBag.insert_status = "Este funcionário não existe!";
                                    break;

                                case 500:
                                    ViewBag.insert_status = "Estamos com problema em nossos servidores.\nTente Novamente mais tarde";
                                    break;
                            }
                        }
                        return View();
                    }

                    else
                        return RedirectToAction("Index", "Home");
                }
            }
            else
                return RedirectToAction("Login", "Usuario");
        }

        public async Task<ActionResult> RemoveEmployee(int id_employee)
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

                    if (user_type == 3)
                    {
                        int result_Delete = Dados.Manager.ManagerActions.Remove_Employee(id_employee, id_user);
                        if(result_Delete == 200) return RedirectToAction("EmployeeList", "Manager");
                        else return RedirectToAction("EmployeeList", "Manager");
                    }
                    else
                        return RedirectToAction("Index", "Home");
                }
            }
            else
                return RedirectToAction("Login", "Usuario");
        }

        public async Task<ActionResult> SeeSupplier()
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

                    if (user_type > 0)
                    {
                        string employeeInfo = Dados.Employees.EmployeeActions.GetEmployeeInformation(id_user);

                        JObject objEmployee = JObject.Parse(employeeInfo);
                        ViewBag.type_user = user_type;
                        return View();

                    }
                    else
                        return RedirectToAction("Index", "Home");
                }
            }
            else
                return RedirectToAction("Login", "Usuario");
        }
        public async Task<ActionResult> Order_for_Supplier()
        {
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