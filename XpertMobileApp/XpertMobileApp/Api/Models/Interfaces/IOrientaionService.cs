using System;
using System.Collections.Generic;
using System.Text;

namespace XpertMobileApp.Services
{
    public interface IOrientaionService
    {
        void Landscape();
        void ReverseLandscape();
        void Portrait();
        void ReversePortrait();
    }
}
