using System;
using System.Collections.Generic;
using System.Text;

namespace EnergyHost.Model.EnergyModels
{
    public class LoginData
    {
        public string name { get; set; }
        public object firstName { get; set; }
        public object lastName { get; set; }
        public string postcode { get; set; }
        public string email { get; set; }
        public string idToken { get; set; }
        public string refreshToken { get; set; }
    }

    public class AmberLogin
    {
        public LoginData data { get; set; }
        public int serviceResponseType { get; set; }
        public string message { get; set; }
    }
}
