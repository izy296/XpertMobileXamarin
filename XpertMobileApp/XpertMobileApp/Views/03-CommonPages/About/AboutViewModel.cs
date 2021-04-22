using System;
using System.Windows.Input;

using Xamarin.Forms;

namespace XpertMobileApp.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel()
        {
            Title = AppResources.pn_About;

            OpenWebCommand = new Command(() => Device.OpenUri(new Uri("http://xpertsoft-dz.com/")));
        }

        public ICommand OpenWebCommand { get; }
    }
}