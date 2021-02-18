using Xamarin.Forms;
using Xamarin.Forms.Internals;
using SampleBrowser.SfListView;
using XpertMobileApp.Api;
using Acr.UserDialogs;
using System;
using XpertMobileApp.DAL;
using Xpert.Common.WSClient.Helpers;
using XpertMobileApp.Models;

namespace XpertMobileApp.ViewModels.Feedback
{
    /// <summary>
    /// ViewModel for review page.
    /// </summary>
    [Preserve(AllMembers = true)]
    public class ReviewPageViewModel : BaseViewModel
    {
        #region Command

        /// <summary>
        /// Gets or sets the value for upload command.
        /// </summary>
        public Command<object> UploadCommand { get; set; }

        /// <summary>
        /// Gets or sets the value for submit command.
        /// </summary>
        public Command<object> SubmitCommand { get; set; }

        #endregion

        #region Fields

        private Product item;
        public Product Item
        {
            get { return item; }
            set { SetProperty(ref item, value); }
        }

        private decimal note;
        public decimal Note
        {
            get { return note; }
            set { SetProperty(ref note, value); }
        }

        private string comment;
        public string Comment
        {
            get { return comment; }
            set { SetProperty(ref comment, value); }
        }

        #endregion

        #region Constructor
        public ReviewPageViewModel(Product item)
        {
            Item = item;
            
            this.UploadCommand = new Command<object>(this.OnUploadTapped);
            this.SubmitCommand = new Command<object>(this.OnSubmitTapped);
        }
        #endregion

        #region Methods

        /// <summary>
        /// Invoked when the Upload button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private void OnUploadTapped(object obj)
        {
            // Do something
        }

        /// <summary>
        /// Invoked when the Submit button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private async void OnSubmitTapped(object obj)
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;
                UserDialogs.Instance.ShowLoading(AppResources.txt_Waiting);
                var eval = new Evaluation()
                {
                    CODE_PRODUIT = Item.Id,
                    NOTE = this.Note,
                    ID_USER = App.User.Token.userID,
                    AVIS_USER = this.comment,
                    PRENOM = App.User.Token.userName
                };
                var res = await BoutiqueManager.SetProducEval(eval);
                UserDialogs.Instance.HideLoading();

                await UserDialogs.Instance.AlertAsync("Merci pour votre évaluation!", AppResources.alrt_msg_Info, AppResources.alrt_msg_Ok);
                await (obj as ContentPage).Navigation.PopAsync();
                return;
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                await UserDialogs.Instance.AlertAsync(WSApi2.GetExceptionMessage(ex), AppResources.alrt_msg_Alert,
                    AppResources.alrt_msg_Ok);
                return ;
            }
            finally
            {
                IsBusy = false;
            }

        }

        #endregion
    }
}
