using Plugin.FirebasePushNotification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XpertMobileApp.Api;
using XpertMobileApp.Models;

namespace XpertMobileApp.Helpers
{
    public static class FireBaseHelper
    {
        public static void SaveToken()
        {
            string firebaseToken = CrossFirebasePushNotification.Current.Token;
        }

        public static void RegisterUserForDefaultTopics(User user, string IdClient)
        {

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
            string clientTopic = string.Format("Client_{0}", IdClient);
            if (!string.IsNullOrEmpty(IdClient) && !CrossFirebasePushNotification.Current.SubscribedTopics.Contains(clientTopic))
            {
                CrossFirebasePushNotification.Current.Subscribe(clientTopic);
            }

            // Topic privé du user
            string PrivateTopic = string.Format("Topic_{0}_{1}", IdClient, user.UserName);
            if (!string.IsNullOrEmpty(user.UserName) && !CrossFirebasePushNotification.Current.SubscribedTopics.Contains(PrivateTopic))
            {
                CrossFirebasePushNotification.Current.Subscribe(PrivateTopic);
            }
        }

        public static void UnsubscribeFromAllTopics()
        {
            CrossFirebasePushNotification.Current.UnsubscribeAll();
        }
    }
}
