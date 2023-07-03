using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace XpertMobileApp.Api.Models
{
    public class UrlService : INotifyPropertyChanged
    {
        public enum TypeService
        {
            Local,
            Remote
        }

        //Urlservice Model 
        public bool Selected
        {
            get;
            set;
        }
        public string DisplayUrlService
        {
            get;
            set;
        }

        //title for the urlService
        public string Title
        {
            get;
            set;
        }

        public TypeService TypeUrl 
        { 
            get;
            set;
        } = TypeService.Remote;

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
