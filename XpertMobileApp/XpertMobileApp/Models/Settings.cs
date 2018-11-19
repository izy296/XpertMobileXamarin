using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace XpertMobileApp.Models
{
    public class Settings : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public bool isModified;

        [PrimaryKey]
        public int Id { get; set; }

        private string serverName { get; set; }

        private string port { get; set; }

        private bool autoRefreshVente { get; set; }

        public string Port
        {
            set
            {
                if (port != value)
                {
                    port = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Port"));
                    isModified = true;
                }
            }
            get
            {
                return port;
            }
        }

        public string ServerName
        {
            set
            {
                if (serverName != value)
                {
                    serverName = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ServerName"));
                    isModified = true;
                }
            }
            get
            {
                return serverName;
            }
        }

        public Settings() { }

    }
}
