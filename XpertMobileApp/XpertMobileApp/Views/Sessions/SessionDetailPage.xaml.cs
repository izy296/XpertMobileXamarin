using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XpertMobileApp.DAL;
using XpertMobileApp.Helpers;
using XpertMobileApp.Models;
using XpertMobileApp.ViewModels;

namespace XpertMobileApp.Views.Encaissement
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SessionDetailPage : ContentPage
	{
        ItemDetailViewModel<TRS_JOURNEES> viewModel;

        public SessionDetailPage(TRS_JOURNEES session)
        {
            InitializeComponent();

            BindingContext = this.viewModel = new ItemDetailViewModel<TRS_JOURNEES>(session);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

    }
}