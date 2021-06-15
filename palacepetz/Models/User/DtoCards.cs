using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace palacepetz.Models.Cards
{
    public class DtoCards
    {
        public long cd_card { get; set; }
        public int id_user { get; set; }
        public int length { get; set; }
        public string flag_card { get; set; }
        public string number_card { get; set; }
        public string shelflife_card { get; set; }
        public int cvv_card { get; set; }
        public string nmUser_card { get; set; }
    }
}