using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace palacepetz.Models.shoppingcart
{
    public class DtoShoppingCart
    {
        public long cd_cart { get; set; }
        public long cd_prod { get; set; }
        public long id_user { get; set; }
        public string product_amount { get; set; }
        public long length { get; set; }
        public long amount { get; set; }
        public string product_price { get; set; }
        public string totalPrice { get; set; }
        public string sub_total { get; set; }
        public string nm_product { get; set; }
        public string image_prod { get; set; }
    }
}