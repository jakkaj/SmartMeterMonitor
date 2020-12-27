using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace NetatmoCore.Helpers
{
    public static class UriHelper
    {
        public static Uri ResourceUriBuilder(HttpClient httpClient, string resource, string accessToken)
        {
            return new Uri(httpClient.BaseAddress + resource).AddQuery("access_token", accessToken);
        }
    }
}
