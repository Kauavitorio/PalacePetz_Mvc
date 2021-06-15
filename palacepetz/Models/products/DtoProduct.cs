using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace palacepetz.Models.products
{
    public class DtoProduct
    {
        [DisplayName("Código")]
        public long cd_prod { get; set; }

        public int id_user { get; set; }

        [DisplayName("Código Categoria")]
        public string cd_category { get; set; }

        [DisplayName("Nome")]
        public string nm_product { get; set; }

        [DisplayName("Quantidade")]
        public string amount { get; set; }

        [DisplayName("Especie")]
        public string species { get; set; }

        [DisplayName("Preço")]
        public double product_price { get; set; }

        public int product_amount { get; set; }

        [DisplayName("Descrição")]
        public string description { get; set; }

        [DisplayName("Data de cadastro")]
        public string date_prod { get; set; }

        [DisplayName("Categoria")]
        public string nm_category { get; set; }

        [DisplayName("Popular")]
        public string popular { get; set; }

        [DisplayName("Data de Validade")]
        public string shelf_life { get; set; }

        [DisplayName("Imagem")]
        public string image_prod { get; set; }
        [DataType(DataType.Upload)]
        [Display(Name = "Upload File")]
        public string file { get; set; }

        public string img_user { get; set; }

        public List<Dictionary<string, object>> Search { get; set; }
    }
}