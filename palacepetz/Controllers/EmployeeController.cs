using palacepetz.Models.category;
using palacepetz.Models.products;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace palacepetz.Controllers
{
    public class EmployeeController : Controller
    {
        // GET: Employee
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult RegisterProduct()
        {
            try
            {
                List<SelectListItem> categoryList = new List<SelectListItem>(Dados.Category.actions.GetAllCategorys());
                ViewBag.category = new SelectList(categoryList, "Value", "Text");
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error on load category list: " + ex);
            }
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> RegisterProduct(DtoProduct prodInfo, HttpPostedFileBase file)
        {
            try
            {
                FileStream stream;
                if (file.ContentLength > 0)
                {
                    string path = Path.Combine(Server.MapPath("~/UploadedFiles"), Path.GetFileName(file.FileName));
                    file.SaveAs(path);
                    stream = new FileStream(Path.Combine(path), FileMode.Open);
                    string resultUpload = await Task.Run(() => Dados.Product.actions.Upload(stream, file.FileName));
                    if (resultUpload != "500")
                    {
                        prodInfo.image_prod = resultUpload;
                        prodInfo.cd_category = Request["category"];
                        prodInfo.species = Request["species"];
                        int result = await Dados.Product.actions.InsertNewProduct(prodInfo);
                        if (result == 201)
                        {
                            ViewBag.statusInsert = "Produto inserido com sucesso";
                        }
                        else if (result == 501)
                        {
                            ViewBag.statusInsert = "Categoria informada não esta cadastrada";
                        }
                        else if (result == 507)
                        {
                            ViewBag.statusInsert = "Alguma informaçao informada esta vazia";
                        }
                        else
                        {
                            ViewBag.statusInsert = "Erro no servidor";
                        }
                    }
                    else
                    {
                        ViewBag.statusInsert = "Erro ao updar a imagem";
                    }
                }
                List<SelectListItem> categoryList = Dados.Category.actions.GetAllCategorys();
                ViewBag.category = new SelectList(categoryList);
                return View();
            }catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error on insert new product: " + ex);
                ViewBag.statusInsert = "Erro ao inserir um produto " + ex;
                List<SelectListItem> categoryList = Dados.Category.actions.GetAllCategorys();
                ViewBag.category = new SelectList(categoryList);
                return View();
            }
        }
    }
}