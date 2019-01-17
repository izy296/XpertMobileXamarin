

using System;
using System.Threading.Tasks;
using XpertMobileApp.Api.Services;
using XpertMobileApp.Models;

namespace XpertMobileApp.ViewModels
{
    public class ActivationViewModel : BaseViewModel
    {
        public ActivationViewModel()
        {
            Title = AppResources.pn_Activation;
        }

        internal Task<Client> ActivateClient(Client client)
        {
            throw new NotImplementedException();
        }

        internal DateTime GetEndDate(string licenceTxt)
        {
            throw new NotImplementedException();
        }

    }
}