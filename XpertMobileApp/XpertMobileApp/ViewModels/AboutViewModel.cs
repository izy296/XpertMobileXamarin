using System;
using System.Windows.Input;

using Xamarin.Forms;

namespace XpertMobileApp.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel()
        {
            Title = "À propos";

            OpenWebCommand = new Command(() => Device.OpenUri(new Uri("https:www.google.com")));
        }

        public ICommand OpenWebCommand { get; }
    }
}