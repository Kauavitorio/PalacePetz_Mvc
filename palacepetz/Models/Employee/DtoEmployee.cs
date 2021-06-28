using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace palacepetz.Models.Employee
{
    public class DtoEmployee
    {
        public long id_user { get; set; }

        [DisplayName("Código Funcionario")]
        public long id_employee { get; set; }

        [DisplayName("Nome")]
        public string name_user { get; set; }

        [DisplayName("Nome de usuario")]
        public string username { get; set; }

        [DisplayName("E-mail")]
        public string email { get; set; }

        [DisplayName("CPF")]
        public string cpf_user { get; set; }

        [DisplayName("Endereço")]
        public string address_user { get; set; }

        [DisplayName("Complemento")]
        public string complement { get; set; }

        [DisplayName("Telefone")]
        public string phone_user { get; set; }

        [DisplayName("CEP")]
        public string zipcode { get; set; }

        [DisplayName("Imagem")]
        public string img_user { get; set; }

        [DisplayName("Data de aniversário")]
        public string birth_date { get; set; }

        [DisplayName("Tipo do usuário")]
        public int user_type { get; set; }

        [DisplayName("Cargo")]
        public string role { get; set; }

        [DisplayName("Numero do CTPS")]
        public string number_ctps { get; set; }

        [DisplayName("Numero do CRMV")]
        public string num_crmv { get; set; }

        [DisplayName("Senha")]
        public string password { get; set; }

        [DataType(DataType.Upload)]
        [Display(Name = "Upload File")]
        public string file { get; set; }

    }
}