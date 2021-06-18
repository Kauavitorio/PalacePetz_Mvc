using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace palacepetz.Models.User
{
    public class DtoPet
    {
        public long cd_animal { get; set; }
        public string nm_animal { get; set; }
        public long id_user { get; set; }
        public string breed_animal { get; set; }
        public string age_animal { get; set; }
        public string weight_animal { get; set; }
        public string species_animal { get; set; }
        public string image_animal { get; set; }

        [DataType(DataType.Upload)]
        [Display(Name = "Upload File")]
        public string file { get; set; }
    }
}