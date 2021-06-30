using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace palacepetz.Models.User
{
    public class DtoUser
    {
        [DisplayName("ID")]
        public int id_user { get; set; }

        [DisplayName("Nome")]
        public string name_user { get; set; }

        [DisplayName("Primeiro nome")]
        public string Firstname_user { get; set; }

        [DisplayName("Último nome")]
        public string Lastname_user { get; set; }

        [DisplayName("Email")]
        public string email { get; set; }

        [DisplayName("CPF")]
        public string cpf_user { get; set; }

        [DisplayName("Endereço")]
        public string address_user { get; set; }

        [DisplayName("Complemento")]
        public string complement { get; set; }

        [DisplayName("CEP")]
        public string zipcode { get; set; }

        [DisplayName("Celular")]
        public string phone_user { get; set; }

        [DisplayName("Data de nascimento")]
        public string birth_date { get; set; }

        [DisplayName("Tipo do usuário")]
        public int user_type { get; set; }
        public int status { get; set; }

        [DisplayName("Imagem")]
        public string img_user { get; set; }
        public string card_flag { get; set; }

        public string number { get; set; }

        [DisplayName("Senha")]
        [Required]
        public string password { get; set; }

        [DisplayName("Senha")]
        [Compare("password", ErrorMessage = "Senhas não são iguais, digite novamente!")]
        public string confirmpassword { get; set; }

        public bool rememberPassword { get; set; }

        [DataType(DataType.Upload)]
        [Display(Name = "Upload File")]
        public string file { get; set; }
    }
}