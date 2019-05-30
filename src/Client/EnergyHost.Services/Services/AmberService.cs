using EnergyHost.Contract;

namespace EnergyHost.Services.Services
{
    public class AmberService
    {
        private ILogService _logService;
        public AmberService(ILogService logService) => 
            _logService = logService;
    }
}

