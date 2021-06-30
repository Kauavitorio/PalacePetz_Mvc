using Newtonsoft.Json.Linq;
using palacepetz.Dados.Auth;
using palacepetz.Dados.Product;
using palacepetz.Models.products;
using palacepetz.Models.User;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace palacepetz.Controllers
{
    public class EmployeeController : Controller
    {
        int id_user, user_type;
        private string name_user, email_user, cpf_user, address_user, complement, zipcode, phone_user, birth_date, img_user, password;
        private int prodPrice;

        // GET: Employee
        public async Task<ActionResult> Index()
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
                        ViewBag.Name = name_user;
                        ViewBag.Role = (string)objEmployee["role"];
                        ViewBag.CPF = cpf_user;
                        ViewBag.Address = address_user + "\n" + complement;
                        ViewBag.Phone = phone_user;
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

        public async Task<ActionResult> RegisterProduct()
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
                        return View();
                    else
                        return RedirectToAction("Index", "Home");
                }
            }
            else
                return RedirectToAction("Login", "Usuario");
        }

        [HttpPost]
        public async Task<ActionResult> RegisterProduct(DtoProduct prodInfo, HttpPostedFileBase file)
        {
            prodInfo.cd_category = Request["category"];

            if (prodInfo.cd_category == "999" || prodInfo.cd_category == null)
                ViewBag.statusInsert = "Selecione uma categoria.";
            else if (prodInfo.amount == null || prodInfo.amount.Length <= 0)
                ViewBag.statusInsert = "Insira uma quantidade";
            else if (prodInfo.nm_product == null || prodInfo.nm_product.Length <= 0)
                ViewBag.statusInsert = "Insira o nome do produto";
            else if (prodInfo.product_price == 0)
                ViewBag.statusInsert = "Insira o preço do produto";
            else if (prodInfo.description == null || prodInfo.description.Length <= 0)
                ViewBag.statusInsert = "Insira a descrição do produto";
            else if (prodInfo.shelf_life == null || prodInfo.shelf_life.Length <= 0)
                ViewBag.statusInsert = "Insira a validade do produto";
            else if (file == null)
                ViewBag.statusInsert = "Insira a imagem do produto";
            else
            {
                try
                {
                    FileStream stream;
                    if (file.ContentLength > 0)
                    {
                        string path = Path.Combine(Server.MapPath("~/UploadedFiles"), Path.GetFileName(file.FileName));
                        file.SaveAs(path);
                        stream = new FileStream(Path.Combine(path), FileMode.Open);
                        string file_name = file.FileName;
                        string resultUpload = await Task.Run(() => actions.Upload(stream, file_name.Replace(" ","_").Replace("c", "_")));
                        prodInfo.species = Request["species"];
                        if (resultUpload != "500")
                        {
                            prodInfo.image_prod = resultUpload;
                            int result = await actions.InsertNewProduct(prodInfo);
                            if (result == 201)
                                ViewBag.statusInsert = "Produto inserido com sucesso";
                            else if (result == 501)
                                ViewBag.statusInsert = "Categoria informada não esta cadastrada";
                            else if (result == 507)
                                ViewBag.statusInsert = "Alguma informaçao informada esta vazia";
                            else
                                ViewBag.statusInsert = "Erro no servidor";
                        }
                        else
                        {
                            ViewBag.statusInsert = "Erro ao updar a imagem";
                        }
                    }

                    List<SelectListItem> categoryList = new List<SelectListItem>(Dados.Category.actions.GetAllCategorys());
                    ViewBag.category = new SelectList(categoryList, "Value", "Text");
                    return View();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("" + ex);
                    ViewBag.statusInsert = "Preencha todos os campos";

                    List<SelectListItem> categoryList = new List<SelectListItem>(Dados.Category.actions.GetAllCategorys());
                    ViewBag.category = new SelectList(categoryList, "Value", "Text");
                    return View();
                }
            }

            return View();
        }

        public async Task<ActionResult> Statistics()
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

                    ViewBag.type_user = user_type;

                    if (user_type > 0)
                    {
                        string StatisticsInfo = Dados.Employees.EmployeeActions.GetStatistics();

                        JObject objStatistics = JObject.Parse(StatisticsInfo);
                        ViewBag.user_amount = (int)objStatistics["user_amount"];
                        ViewBag.products_amount = (int)objStatistics["products_amount"];
                        ViewBag.food_percentage = (int)objStatistics["food_percentage"];
                        ViewBag.medicines_percentage = (int)objStatistics["medicines_percentage"];
                        ViewBag.aesthetics_percentage = (int)objStatistics["aesthetics_percentage"];
                        ViewBag.accessories_percentage = (int)objStatistics["accessories_percentage"];
                        string getPrice = (string)objStatistics["totalPrice"];
                        var val = double.Parse(getPrice,
                            NumberStyles.AllowThousands | NumberStyles.AllowDecimalPoint | NumberStyles.AllowCurrencySymbol);
                        ViewBag.totalPrice = val;
                        return View();

                    }
                    else
                        return RedirectToAction("Index", "Home");
                }
            }
            else
                return RedirectToAction("Login", "Usuario");
        }

        public async Task<ActionResult> ListProducts(string filter)
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

                    if (user_type > 0)
                    {
                        if(filter != null && filter.Replace(" ", "").Length > 0) return View(ac.Get_AllProducts_With_NameFilter(filter, id_user));
                        else return View(ac.GetAllProducts(id_user));

                    }
                    else
                        return RedirectToAction("Index", "Home");
                }
            }
            else
                return RedirectToAction("Login", "Usuario");
        }

        public async Task<ActionResult> FunctionsProducts()
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
                        return View();

                    }
                    else
                        return RedirectToAction("Index", "Home");
                }
            }
            else
                return RedirectToAction("Login", "Usuario");
        }

        public async Task<ActionResult> DeleteProducts(int cd_prod)
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

                    if (user_type > 0)
                    {
                        return View();
                    }
                    else
                        return RedirectToAction("Index", "Home");
                }
            }
            else
                return RedirectToAction("Login", "Usuario");
        }

        public async Task<ActionResult> EditProduct(int cd_prod = 0)
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

                    if (user_type > 0)
                    {
                        if (cd_prod != 0)
                        {
                            ModelState.Clear();
                            string resultDetails = ac.Get_Product_Details(cd_prod);
                            if (resultDetails != "500" || resultDetails != "204")
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
                                infoProd.shelf_life = (string)objProds["shelf_life"];
                                infoProd.amount = (string)objProds["amount"];
                                infoProd.description = (string)objProds["description"];

                                List<SelectListItem> category = new List<SelectListItem>(Dados.Category.actions.GetAllCategorys());
                                ViewBag.category = new SelectList(category, "Value", "Text", 999);
                                return View(infoProd);
                            }
                            else
                                return RedirectToAction("ListProducts", "Employee");
                        }
                        else
                            return RedirectToAction("ListProducts", "Employee");
                    }
                    else
                        return RedirectToAction("Index");
                }
            }
            else
                return RedirectToAction("Login", "Usuario");
        }

        [HttpPost]
        public async Task<ActionResult> EditProduct(DtoProduct prodinfo, HttpPostedFileBase image_prod)
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

                    if (user_type > 0)
                    {
                        int category = int.Parse(Request["category"]);
                        string species = Request["species"];
                        if (species == null || species.Length <= 0)
                            ViewBag.status_update = "Selecione uma especie!!";
                        else
                        {
                            prodinfo.species = species;
                            prodinfo.cd_category = category + "";
                            if (image_prod != null)
                            {
                                FileStream stream;
                                if (image_prod.ContentLength > 0)
                                {
                                    string path = Path.Combine(Server.MapPath("~/UploadedFiles"), Path.GetFileName(image_prod.FileName));
                                    image_prod.SaveAs(path);
                                    stream = new FileStream(Path.Combine(path), FileMode.Open);
                                    string resultUpload = await Task.Run(() => actions.Upload(stream, prodinfo.nm_product));
                                    prodinfo.image_prod = resultUpload;
                                    int resultpload = await Dados.Employees.EmployeeActions.UpdateProduct(prodinfo, (int)prodinfo.cd_prod);
                                    if (resultpload == 200)
                                        return RedirectToAction("ListProducts", "Employee");
                                }
                            }
                            else
                            {
                                string resultDetails = ac.Get_Product_Details((int)prodinfo.cd_prod);
                                JObject objProds = JObject.Parse(resultDetails);
                                prodinfo.image_prod = (string)objProds["image_prod"];
                                int resultpload = await Dados.Employees.EmployeeActions.UpdateProduct(prodinfo, (int)prodinfo.cd_prod);
                                if (resultpload == 200)
                                    return RedirectToAction("ListProducts", "Employee");
                            }
                        }
                        return View();
                    }
                    else
                        return RedirectToAction("Index");
                }
            }
            else
                return RedirectToAction("Login", "Usuario");
        }

        public async Task<ActionResult> CheckCustommers()
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

                        ViewBag.type_user = user_type;

                    if (user_type > 0)
                    {
                        return View(Dados.Employees.EmployeeActions.GetCustomers(id_user));

                    }
                    else
                        return RedirectToAction("Index", "Home");
                }
            }
            else
                return RedirectToAction("Login", "Usuario");
        }

        public async Task<ActionResult> EditCustommers(int id_user_edit)
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

                    ViewBag.type_user = user_type;

                    if (user_type > 0)
                    {
                        return View(Dados.Employees.EmployeeActions.GetCustomerInfo(id_user_edit));
                    }
                    else
                        return RedirectToAction("Index", "Home");
                }
            }
            else
                return RedirectToAction("Login", "Usuario");
        }

        [HttpPost]
        public async Task<ActionResult> EditCustommers(DtoUser usereditinfo)
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

                    ViewBag.type_user = user_type;

                    if (user_type > 0)
                    {
                        if (usereditinfo.cpf_user == null || usereditinfo.cpf_user.Length <= 0)
                            ViewBag.status_edit = "CPF não pode ser vazio!!";
                        else if (!Dados.User.Cpf_actions.IsValid(usereditinfo.cpf_user.ToString()))
                            ViewBag.status_edit = "CPF informado é invalido!!";
                        else if (usereditinfo.name_user == null || usereditinfo.name_user.Length <= 0)
                            ViewBag.status_edit = "Nome informado é invalido!!";
                        else
                        {
                            if (usereditinfo.password == null || usereditinfo.password.ToString().Length <= 0 || usereditinfo.password == " ")
                                usereditinfo.password = "no update";
                            int resultUpdate = await Dados.Employees.EmployeeActions.UpdateCustomerProfile(usereditinfo, usereditinfo.id_user);
                            if (resultUpdate == 200)
                                return RedirectToAction("CheckCustommers");
                            else
                                return RedirectToAction("CheckCustommers");
                        }
                        return View(Dados.Employees.EmployeeActions.GetCustomerInfo(usereditinfo.id_user));
                    }
                    else
                        return RedirectToAction("Index", "Home");
                }
            }
            else
                return RedirectToAction("Login", "Usuario");
        }

        public async Task<ActionResult> DisabledUser(int id_user_disable)
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

                    ViewBag.type_user = user_type;

                    if (user_type > 0)
                    {
                        int result_disable = await Dados.Employees.EmployeeActions.DisableCustomer(id_user_disable, id_user);
                        if (result_disable != 0)
                            return RedirectToAction("CheckCustommers");
                        else
                            return RedirectToAction("CheckCustommers");
                    }
                    else
                        return RedirectToAction("Index", "Home");
                }
            }
            else
                return RedirectToAction("Login", "Usuario");
        }

        public async Task<ActionResult> EnableUser(int id_user_enable)
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

                    ViewBag.type_user = user_type;  

                    if (user_type > 0)
                    {
                        int result_disable = await Dados.Employees.EmployeeActions.EnableCustomer(id_user_enable, id_user);
                        if (result_disable != 0)
                            return RedirectToAction("CheckCustommers");
                        else
                            return RedirectToAction("CheckCustommers");
                    }
                    else
                        return RedirectToAction("Index", "Home");
                }
            }
            else
                return RedirectToAction("Login", "Usuario");
        }

        public async Task<ActionResult> UserScheduledServices()
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

                    ViewBag.type_user = user_type;

                    if (user_type > 0)
                    {
                        return View(Dados.Employees.EmployeeActions.GetAllScheduledServices(id_user, user_type));
                    }
                    else
                        return RedirectToAction("Index", "Home");
                }
            }
            else
                return RedirectToAction("Login", "Usuario");
        }

        public async Task<ActionResult> DatailsScheduledServices(int cd_schedule, int id_user)
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
                    var id_user_employee = (int)obj["id_user"];
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

                    ViewBag.type_user = user_type;

                    if (user_type > 0)
                    {
                        return View(Dados.Employees.EmployeeActions.GetScheduledServicenfo(id_user_employee, cd_schedule, id_user));
                    }
                    else
                        return RedirectToAction("Index", "Home");
                }
            }
            else
                return RedirectToAction("Login", "Usuario");
        }

        public async Task<ActionResult> AllOrder()
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
                    var id_user_employee = (int)obj["id_user"];
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

                    ViewBag.type_user = user_type;

                    if (user_type > 0)
                    {
                        return View(Dados.Employees.EmployeeActions.GetAllOrder(id_user_employee));
                    }
                    else
                        return RedirectToAction("Index", "Home");
                }
            }
            else
                return RedirectToAction("Login", "Usuario");
        }

        public async Task<ActionResult> OrderControl(int cd_order)
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
                    var id_user_employee = (int)obj["id_user"];
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

                    ViewBag.type_user = user_type;

                    if (user_type > 0)
                    {
                        return View(Dados.Employees.EmployeeActions.GetDetailsOrder(id_user_employee, cd_order));
                    }
                    else
                        return RedirectToAction("Index", "Home");
                }
            }
            else
                return RedirectToAction("Login", "Usuario");
        }

        public async Task<ActionResult> UpdateOrderStatus(DtoOrders orderinfo)
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
                    var id_user_employee = (int)obj["id_user"];
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

                    ViewBag.type_user = user_type;

                    if (user_type > 0)
                    {
                        if (orderinfo.status != null)
                        {
                            int resultUpdate = await Dados.Employees.EmployeeActions.UpdateOrderStatus(orderinfo, id_user_employee);
                            if(resultUpdate == 200)
                                return RedirectToAction("AllOrder");
                            else
                                return RedirectToAction("AllOrder");
                        }
                        else
                            return RedirectToAction("AllOrder");
                    }
                    else
                        return RedirectToAction("Index", "Home");
                }
            }
            else
                return RedirectToAction("Login", "Usuario");

        }

        public async Task<ActionResult> Informations()
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

                    ViewBag.type_user = user_type;

                    if (user_type > 0)
                        return View();
                    else
                        return RedirectToAction("Index", "Home");
                }
            }
            else
                return RedirectToAction("Login", "Usuario");
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