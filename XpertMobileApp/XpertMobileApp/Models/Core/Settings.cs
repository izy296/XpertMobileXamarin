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

        private string serverName;

        private string port;

        private string language;

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

        public string Language
        {
            set
            {
                if (language != value)
                {
                    language = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Language"));
                    isModified = true;
                }
            }
            get
            {
                return language;
            }
        }

        public Settings() { }

    }
}
