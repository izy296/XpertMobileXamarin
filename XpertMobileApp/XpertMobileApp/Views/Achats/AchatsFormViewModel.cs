using System;
using System.Collections.Generic;
using System.Text;
using XpertMobileApp.DAL;
using XpertMobileApp.Models;
using XpertMobileApp.ViewModels;

namespace XpertMobileApp.Views.Achats
{
    public class AchatsFormViewModel : ItemRowsDetailViewModel<View_ACH_DOCUMENT, View_ACH_DOCUMENT_DETAIL>
    {
        private BSE_CHAUFFEUR selectedChauffeur;
        public BSE_CHAUFFEUR SelectedChauffeur
        {
            get { return selectedChauffeur; }
            set { SetProperty(ref selectedChauffeur, value); }
        }

        private string selectedImmat;
        public string SelectedImmat
        {
            get { return selectedImmat; }
            set { SetProperty(ref selectedImmat, value); }
        }

        public AchatsFormViewModel(View_ACH_DOCUMENT obj, string itemId) : base(obj, itemId)
        {

        }

    }
}
