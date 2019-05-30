using System.IO;
using EnergyHost.Contract;
using EnergyHost.Model.Settings;
using Microsoft.Extensions.Options;

namespace EnergyHost.Services
{
    public class AppStartupService : IAppStartupService
    {

        private readonly IOptions<EnergyHostSettings> _options;
       
        private readonly ILogService _logService;

        public AppStartupService(
            IOptions<EnergyHostSettings> options
           )
        {
            _options = options;
           
        }

        public string RunApp()
        {
            if (!_validate())
            {
                return "2";
            }

            var opts = _options.Value;

           

            return "";
        }

        bool _validate()
        {
            var opts = _options.Value;

            
            return true;

        }

        bool _check(string val, string errorText)
        {
            if (string.IsNullOrWhiteSpace(val))
            {
                _logService.WriteError($"Please check inputs: {errorText}");
                return false;
            }

            return true;
        }
    }
}
