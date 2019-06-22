using System;
using System.Collections.Generic;
using System.Text;
using EnergyHost.Contract;

namespace EnergyHost.Services.Services
{
    public class DataLoggerService
    {
        private readonly ILogService _logService;

        public DataLoggerService(ILogService logService)
        {
            _logService = logService;
        }
        /// <summary>
        /// Periodically refresh device status 
        /// </summary>
        async void _deviceUpdates()
        {
            while (true)
            {

            }
        }
    }
}
