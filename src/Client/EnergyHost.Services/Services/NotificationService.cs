using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using EnergyHost.Contract;
using EnergyHost.Model.Settings;
using Microsoft.Extensions.Options;

namespace EnergyHost.Services.Services
{
    public class NotificationService : INotificationService
    {
        private readonly ILogService _logService;
        private readonly IOptions<EnergyHostSettings> _settings;

        public NotificationService(ILogService logService, IOptions<EnergyHostSettings> settings)
        {
            _logService = logService;
            _settings = settings;
        }

        public async Task SendNotification(string text, string title = null)
        {
            //await sendAlexa(text);
            await sendPushover(text, title);
        }

        private async Task sendPushover(string text, string title)
        {
            var notification = HttpUtility.UrlEncode(text);
            var token = _settings.Value.PUSHOVER_TOKEN;
            var user = _settings.Value.PUSHOVER_USER;

            if (string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(user))
            {
                _logService.WriteLog("Could not send Pushover notification - config PUSHOVER_TOKEN and/or PUSHOVER_USER  not set. See: https://pushover.net/api");
                return;
            }

            using (var client = new HttpClient())
            {
                var uri = new Uri(
                    $"https://api.pushover.net/1/messages.json?token={token}&user={user}&message={notification}&title={title}");
                var result = await client.PostAsync(uri, null);
                if (result.IsSuccessStatusCode)
                {
                    _logService.WriteLog($"Sent notification to Pushover: {title} - {text}");
                }
                else
                {
                    _logService.WriteError($"Pushover notification failed with code: {result.StatusCode}");
                }
            }
        }

        private async Task sendAlexa(string text)
        {
            var notification = HttpUtility.UrlEncode(text);
            var code = _settings.Value.ALEXA_NOTIFICATION_KEY;
            if (string.IsNullOrWhiteSpace(code))
            {
                _logService.WriteLog("Could not send Alexa notification - config ALEXA_NOTIFICATION_KEY not set. See: https://www.thomptronics.com/about/notify-me");
                return;
            }
            using (var client = new HttpClient())
            {
                var uri = new Uri(
                    $"https://api.notifymyecho.com/v1/NotifyMe?notification={notification}&accessCode={code}");
                var result = await client.GetAsync(uri);
                if (result.IsSuccessStatusCode)
                {
                    _logService.WriteLog($"Sent notification to Alexa: {text}");
                }
                else
                {
                    _logService.WriteError($"Alexa notification failed with code: {result.StatusCode}");
                }
            }
        }
    }
}
