using SQLite;


namespace XpertMobileApp.Models
{
    public class Client
    {
        [PrimaryKey]
        public string Id { get; set; }

        public string Email { get; set; }
        public string Phone { get; set; }
        public string ClientId { get; set; }
        public string DeviceId { get; set; }
        public string AppName { get; set; }
        public string Mobile_Version { get; set; }
        public string LicenceTxt { get; set; }

        public Client() { }

        public Client(string email, string clientId)
        {
            Email = email;
            ClientId = clientId;
        }
    }
}
