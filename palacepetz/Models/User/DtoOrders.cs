using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace palacepetz.Models.User
{
    public class DtoOrders
    {
        public long cd_order { get; set; }
        public int id_user { get; set; }
        public string cpf_user { get; set; }
        public string discount { get; set; }
        public string coupom { get; set; }
        public string sub_total { get; set; }
        public string totalPrice { get; set; }
        public int product_amount { get; set; }
        public string date_order { get; set; }
        public int cd_card { get; set; }
        public string status { get; set; }
        public long deliveryTime { get; set; }
        public string payment { get; set; }
    }
}