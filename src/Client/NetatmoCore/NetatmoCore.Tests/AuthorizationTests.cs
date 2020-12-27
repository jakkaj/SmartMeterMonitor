using System;
using Xunit;
using netatmocore;

namespace NetatmoCore.Tests
{
    public class AuthorizationTests
    {
        [Fact]
        public void Authorization_Success_With_Correct_User()
        {
            // Fill these with own values for testing purpose
            var clientId = "5fe6e052c08fbf05e129397c";
            var clientSecret = "mrnFSCPRLi8GuJ0OkAvGOe5TTFt";
            var username = "jakkaj@gmail.com";
            var password = "aJuDnQM!m6h82LW";
            var device_id = "70:ee:50:3d:02:0a";

            var auth = new NetatmoAuth();
            var token = auth.Login(clientId, clientSecret, username, password, new[] { NetatmoAuth.READ_STATION});

            var netatmo = new NetAtmoClient(token.access_token);
            
            var result = netatmo.Getthermostatsdata(device_id).Result;
        }
    }
}
