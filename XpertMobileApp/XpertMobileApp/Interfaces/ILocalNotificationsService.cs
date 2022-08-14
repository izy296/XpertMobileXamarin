using System.Collections.Generic;

namespace XpertMobileApp
{
    public interface ILocalNotificationsService
    {
        void ShowNotification(string title, string message, IDictionary<string, string> data);
    }
}
