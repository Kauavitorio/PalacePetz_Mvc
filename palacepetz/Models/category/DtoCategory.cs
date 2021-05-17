using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace palacepetz.Models.category
{
    public class DtoCategory
    {

        [DisplayName("Código")]
        public string cd_category { get; set; }

        [DisplayName("Nome")]
        public string nm_category  { get; set; }

        [DisplayName("Imagem")]
        public string img_category { get; set; }
    }
}