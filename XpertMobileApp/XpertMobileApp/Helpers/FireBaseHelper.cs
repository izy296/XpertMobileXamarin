using Plugin.FirebasePushNotification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XpertMobileApp.Api;
using XpertMobileApp.Models;

namespace XpertMobileApp.Helpers
{
    public class FireBaseHelper
    {
        public void SaveToken()
        {
            string firebaseToken = CrossFirebasePushNotification.Current.Token;
        }

        public void RegisterUser(User user, string IdClient)
        {
            #region FireBase notification

            // FireBasehelper.RegisterClient();
            CrossFirebasePushNotification.Current.UnsubscribeAll();

            // Topic global XpertSoft
            if (!CrossFirebasePushNotification.Current.SubscribedTopics.Contains("XpertSoft"))
            {
                CrossFirebasePushNotification.Current.Subscribe("XpertSoft");
            }

            // Topic AppName XpertPharm, XpertCom ...etc
            if (!CrossFirebasePushNotification.Current.SubscribedTopics.Contains(Constants.AppName))
            {
                CrossFirebasePushNotification.Current.Subscribe(Constants.AppName);
            }

            // Topic privé du client
            string clientTopic = "Client" + IdClient;
            if (!CrossFirebasePushNotification.Current.SubscribedTopics.Contains(clientTopic))
            {
                CrossFirebasePushNotification.Current.Subscribe(clientTopic);
            }

            // Topic privé du user
            string PrivateTopic = "Topic" + user.Id;
            if (!CrossFirebasePushNotification.Current.SubscribedTopics.Contains(PrivateTopic))
            {
                CrossFirebasePushNotification.Current.Subscribe(PrivateTopic);
            }

            #endregion
        }
    }
}
