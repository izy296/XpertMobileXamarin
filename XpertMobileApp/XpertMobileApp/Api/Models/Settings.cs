using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using XpertMobileApp.Api.Services;
using XpertWebApi.Models;

namespace XpertMobileApp.Models
{
    public class Settings : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public bool isModified;

        [PrimaryKey]
        public int Id { get; set; }

        private string language = "fr";

        private string serviceUrl;
        public string ServiceUrl
        {
            set
            {
                if (serviceUrl != value)
                {
                    serviceUrl = value?.Trim();
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ServiceUrl"));
                    isModified = true;
                }
            }
            get
            {
                return serviceUrl;
            }
        }

        public string PrinterType { get; set; }

        private string printerName;
        public string PrinterName
        {
            set
            {
                if (printerName != value)
                {
                    printerName = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("PrinterName"));
                    isModified = true;
                }
            }
            get
            {
                return printerName;
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

        public bool SubscribedToFBNotifications { get; set; } = false;

        public string ClientId { get; set; }
        public int Mobile_Edition { get; set; }
        public bool ShouldUpdate { get; set; }
        public string DestinationVersion { get; set; }
        public string MachineName { get; set; } = XpertHelper.GetMachineName();
        public string ClientName { get; set; } = "Nom du client ...";

        private string defaultMagasinVente;
        public string DefaultMagasinVente
        {
            set
            {
                if (defaultMagasinVente != value)
                {
                    defaultMagasinVente = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DefaultMagasinVente"));
                    isModified = true;
                }
            }
            get
            {
                return defaultMagasinVente;
            }
        }

        private string caisseDedier;
        public string CaisseDedier
        {
            set
            {
                if (caisseDedier != value)
                {
                    caisseDedier = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CaisseDedier"));
                    isModified = true;
                }
            }
            get
            {
                return caisseDedier;
            }
        }
        public Settings() { }
    }

    public class SYS_MOBILE_PARAMETRE
    {
        public string DEFAULT_COMPAGNE_LOT { get; set; }
        public string DEFAULT_ACHATS_MAGASIN { get; set; }
        public string DEFAULT_UNITE_ACHATS { get; set; }
    }
}
