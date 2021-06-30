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
        public int cd_schedule { get; set; }

        [DisplayName("ID Usuário")]
        public int id_user { get; set; }

        [DisplayName("Data do agendamento")]
        public string date_schedule { get; set; }

        [DisplayName("Horário do agendamento")]
        public string time_schedule { get; set; }
        public string name_user { get; set; }

        [DisplayName("Código do animal")]
        public int cd_animal { get; set; }

        [DisplayName("Código do veterinário")]
        public int cd_veterinary { get; set; }

        [DisplayName("Tipo de pagamento")]
        public int payment_type { get; set; }

        [DisplayName("Descrição")]
        public string description { get; set; }

        public string nm_veterinary { get; set; }
        public string nm_animal { get; set; }

        [DisplayName("Tipo de serviço")]
        public int service_type { get; set; }

        [DisplayName("Delivery")]
        public int delivery { get; set; }

        [DisplayName("Status")]
        public int status { get; set; }
    }
}