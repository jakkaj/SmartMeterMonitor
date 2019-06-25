using System.IO;
using System.Threading.Tasks;
using EnergyHost.Contract;
using EnergyHost.Model.Settings;
using Microsoft.Extensions.Options;

namespace EnergyHost.Services
{
    public class AppStartupService : IAppStartupService
    {

        private readonly IOptions<EnergyHostSettings> _options;
        private readonly IDataLoggerService _dataService;

        private readonly ILogService _logService;

        public AppStartupService(
            IOptions<EnergyHostSettings> options,
            IDataLoggerService dataService,
            ILogService logService
           )
        {
            _options = options;
            _dataService = dataService;
            _logService = logService;
           
        }

        public async Task<string> RunApp()
        {
            
            //if (!_validate())
            //{
            //    return "2";
            //}

            //var opts = _options.Value;

            await _dataService.Start();


            await Task.Delay(-1);

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
