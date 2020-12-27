using System;
using RestSharp;
using NetatmoCore.Models;
using System.Net;
using RestSharp.Authenticators;

namespace NetatmoCore
{
    public class NetatmoAuth
    {
        private const string AUTHORIZE_URL = "https://api.netatmo.com/oauth2/";

        // OAuth2 scopes
        public const string READ_STATION = "read_station"; // Read weather station's data
        public const string WRITE_THERMOSTAT = "write_thermostat"; // Configure the thermostat
        public const string READ_THERMOSTAT = "read_thermostat"; // Read thermostat's data
        public const string READ_CAMERA = "read_camera"; // Read welcome camera's data
        public const string ACCESS_CAMERA = "access_camera"; // Access welcome camera
        public const string WRITE_CAMERA = "write_camera"; // Write welcome camera's data
        public const string READ_HOMECOACH = "read_homecoach"; // Read data coming from Healthy Home Coach

        private readonly IRestClient _restClient;
        public NetatmoAuth(IRestClient client)
        {
            _restClient = client;
        }

        public NetatmoAuth()
        {
            _restClient = new RestClient();
        }

        public OAuthAuthorization Login(string clientId, string clientSecret, string username, string password, params string[] scope)
        {
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;

            _restClient.BaseUrl = new Uri(AUTHORIZE_URL);
            var request = new RestRequest() { Method = Method.POST, Resource = "/token" };
            request.AddParameter("grant_type", "password")
                .AddParameter("client_id", clientId)
                .AddParameter("client_secret", clientSecret)
                .AddParameter("username", username)
                .AddParameter("password", password)
                .AddParameter("scope", string.Join(" ", scope));

            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");

            var response = _restClient.Execute(request);
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new UnauthorizedAccessException("Failed to authenticate. Check client_id, client_secret, username and password. Reply: " + response.Content);
            }

            return SimpleJson.SimpleJson.DeserializeObject<OAuthAuthorization>(response.Content);
        }

    }
}
