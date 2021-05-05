using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace palacepetz.Models.User
{
    public class DtoUser
    {
        public int id_user { get; set; }
        public string name_user { get; set; }
        public string email { get; set; }
        public string cpf_user { get; set; }
        public string address_user { get; set; }
        public string complement { get; set; }
        public string zipcode { get; set; }
        public string phone_user { get; set; }
        public string birth_date { get; set; }
        public int user_type { get; set; }
        public string img_user { get; set; }

        [Required(ErrorMessage = "Email ou senha invelido")]
        public string password { get; set; }
    }
}