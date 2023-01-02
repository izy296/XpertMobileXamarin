
using SQLite;
using System;

namespace Xpert.Common.WSClient.Model
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
        public string fullName { get; set; }
        public string error_Descriptjion { get; set; }
        public DateTime expire_Date { get; set; }

        public string CODE_TIERS { get; set; }
        public string CODE_COMPTE { get; set; }
        public string UserGroup { get; set; }
        public string GroupName { get; set; }
        public string TEL_USER { get; set; } 
        public string PREFIX_USER_MOBILE { get; set; } 

        public Token() { }
    }
}
