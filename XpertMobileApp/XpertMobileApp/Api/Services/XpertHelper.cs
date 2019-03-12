using System;
using System.Collections.Generic;
using System.Text;
using XpertMobileApp.DAL;

namespace XpertMobileApp.Api.Services
{
    public static class XpertHelper
    {

        public static void UpdateItemIndex<T>(List<T> items)
        {
            int i = 0;
            foreach (var item in items)
            {
                i += 1;
                (item as BASE_CLASS).Index = i;
            }
        }
    }
}
