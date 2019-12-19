using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using Xpert.Common.WSClient.Model;

namespace XpertMobileApp.Models
{
    public class User
    {
        [PrimaryKey]
        public string Id { get; set; }
        public string ClientId { get; set; } 
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public string CODE_TIERS { get; set; }
        public string UserGroup { get; set; }
        public string GroupName { get; set; }
        
        public Token Token { get; set; }


        public User() { }

        public User(string userName, string passWord)
        {
            UserName = userName;
            PassWord = passWord;
        }
    }
}
