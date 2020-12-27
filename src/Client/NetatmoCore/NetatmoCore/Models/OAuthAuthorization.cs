using System;
using System.Collections.Generic;
using System.Text;

namespace NetatmoCore.Models
{
    public class OAuthAuthorization
    {

        public string access_token { get; set; }
        public int expires_in { get; set; }
        public string refresh_token { get; set; }
    }
}
