using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace XpertMobileApp.Models
{
    public class Token
    {
        [PrimaryKey]
        public int Id { get; set; }
        public string userID { get; set; }
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public string userName { get; set; }
        public string error_Descriptjion { get; set; }
        public DateTime expire_Date { get; set; }

        public Token() { }
    }
}
