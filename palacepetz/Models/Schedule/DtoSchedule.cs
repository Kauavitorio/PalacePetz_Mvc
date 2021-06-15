using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace palacepetz.Models.Schedule
{
    public class DtoSchedule
    {
        [DisplayName("Código da consulta")]
        public long cd_shedule { get; set; }

        [DisplayName("Animal")]
        public int pet_user { get; set; }

        [DisplayName("Nome do veterinário")]
        public string name_vet { get; set; }

        [DisplayName("Data")]
        public string date { get; set; }

        [DisplayName("Horário")]
        public string time { get; set; }

        [DisplayName("CPF")]
        public string cpf { get; set; }

        [DisplayName("Descrição")]
        public string description { get; set; }

        [DisplayName("Forma de pagamento")]
        public string payment { get; set; }
    }
}