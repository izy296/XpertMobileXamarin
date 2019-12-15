using System;
using System.Collections.Generic;
using System.Text;

namespace XpertMobileApp.Models
{
    public class LicenceInfos
    {
        public string DeviceId { get; set; }

        public string ProductId { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string ClientId { get; set; }
        
        public string ClientName { get; set; }

        public int Mobile_Edition { get; set; }

        public DateTime ActivationDate { get; set; }

        public DateTime ExpirationDate { get; set; }

        public string Mobile_Remote_URL { get; set; }
    }
}
