using System;
using System.Collections.Generic;
using System.Text;


namespace itMobileApp.Data
{
    public interface INetworkConnection
    {
        bool IsConnected { get; }
        void CheckNetworkConnexction();
    }
}
