using System;
using System.Collections.Generic;
using System.Text;

namespace XpertMobileApp.Api
{
    public static class Constants
    {
        public static string LOCAL_DB_NAME = "XpertLocalDb.db3";

        public static TimeSpan ImageCashValidityTimeSpan => new TimeSpan(0, 50, 0);
    }
}
