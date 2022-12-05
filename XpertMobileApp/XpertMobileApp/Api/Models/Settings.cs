using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;
using XpertMobileApp.Api.Models;
using XpertMobileApp.Api.Services;
using XpertWebApi.Models;

namespace XpertMobileApp.Models
{
    public class Settings : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (propertyName == "EnableMultiPrinter")
            {
                MessagingCenter.Send(this, "MultiPrinterChanged", "");
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

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

        /* to save display type (line or grid ) ! */
        private bool displayType;
        public bool DisplayType
        {
            get
            {
                return displayType;
            }
            set
            {
                displayType = value;
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
        private bool enableMultiPrinter = false;
        public bool EnableMultiPrinter
        {
            get { return enableMultiPrinter; }
            set
            {
                if (value != enableMultiPrinter)
                {
                    enableMultiPrinter = value;
                    NotifyPropertyChanged();
                }
            }
        }

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

        //Fields to store cridentials for the next time
        private string isChecked;
        public string IsChecked
        {
            set { isChecked = value; }
            get { return isChecked; }
        }

        private string userName;
        public string Username
        {
            get { return userName; }
            set { userName = value; }
        }

        private string password;
        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        //Connect Only with password 
        private bool connectWithPasswordOnly;
        public bool ConnectWithPasswordOnly
        {
            get { return connectWithPasswordOnly; }
            set { connectWithPasswordOnly = value; }
        }

        private string usernameOnly;
        public string UsernameOnly
        {
            get { return usernameOnly; }
            set { usernameOnly = value; }
        }

        private string notifiaction;
        public string Notifiaction
        {
            get { return notifiaction; }
            set { notifiaction = value; }
        }

        private string multiPrinterList;
        public string MultiPrinterList
        {
            get { return multiPrinterList; }
            set { multiPrinterList = value; }
        }

        public Settings()
        {
        }
    }

    public class SYS_MOBILE_PARAMETRE
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string DEFAULT_COMPAGNE_LOT { get; set; }
        public string DEFAULT_ACHATS_MAGASIN { get; set; }
        public string DEFAULT_VENTE_MAGASIN { get; set; }
        public string DEFAULT_UNITE_ACHATS { get; set; }
        public bool DIRECT_LOGIN { get; set; }
        public string NOM_PHARM { get; set; }
        public string ADRESSE_PHARM { get; set; }
        public string TEL_PHARM { get; set; }
        public string PIED_TICKET { get; set; }
        public short INCLUDE_NAME_VENDEUR { get; set; }
        public short AFFICHE_MONNAIE { get; set; }
        public short AFFICHER_NUM_VENTE_TICKET { get; set; }
    }
}
