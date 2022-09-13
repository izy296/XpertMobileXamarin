using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XpertMobileApp.Base
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class XBasePage : ContentPage,ISearchPage
    {
        public XBasePage()
        {
            InitializeComponent();
            SearchBarTextChanged += HandleSearchBarTextChanged;
        }

        public event EventHandler<string> SearchBarTextChanged;

        void ISearchPage.OnSearchBarTextChanged(string text)
        {
            SearchBarTextChanged?.Invoke(this, text);
        }

        public virtual void HandleSearchBarTextChanged(object sender, string searchBarText)
        {
            //Logic to handle updated search bar text
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
    }
}