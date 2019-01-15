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

        private string language;


        private string serviceUrl;
        public string ServiceUrl
        {
            set
            {
                if (serviceUrl != value)
                {
                    serviceUrl = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ServiceUrl"));
                    isModified = true;
                }
            }
            get
            {
                return serviceUrl;
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

        public bool ShouldUpdate { get; set; }
        public string DestinationVersion { get; set; }

        public Settings() { }

    }
}
