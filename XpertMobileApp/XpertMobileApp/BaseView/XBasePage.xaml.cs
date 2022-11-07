using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XpertMobileApp.Base
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class XBasePage : ContentPage, ISearchPage
    {
        public Timer timer;

        private TimeSpan _totalSeconds = new TimeSpan(0, 0, 0, 1);

        public TimeSpan TotalSeconds

        {

            get { return _totalSeconds; }

            set { _totalSeconds = value; }

        }

        private string searchBarText;
        public string SearchBarText
        {
            get { return searchBarText; }
            set { value=searchBarText; }
        }



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

        public virtual void SearchCommand()
        {
        }

        public virtual void HandleSearchBarTextChanged(object sender, string searchBarText)
        {
            if (searchBarText != "")
            {
                if (timer != null)
                {
                    timer.Stop();
                    timer.Dispose();
                }
                timer = new Timer();
                timer.Interval = 1000;
                timer.Elapsed += t_Tick;
                timer.Start();
            }
            else
            {
                SearchBarText = searchBarText;
                SearchCommand();
                if (timer != null)
                {
                    timer.Stop();
                    timer.Dispose();
                }
                TotalSeconds = new TimeSpan(0, 0, 0, 1);
            }
        }

        async void t_Tick(object sender, EventArgs e)
        {
            if (TotalSeconds == new TimeSpan(0, 0, 0, 0))
            {
                //do something after hitting 0, in this example it just stops/resets the timer
                SearchCommand();

                timer.Stop();
                timer.Dispose();
                TotalSeconds = new TimeSpan(0, 0, 0, 1);
            }
            else
            {
                if (TotalSeconds != (new TimeSpan(0, 0, 0, 0)))
                    TotalSeconds = TotalSeconds.Subtract(new TimeSpan(0, 0, 0, 1));

            }
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
    }
}