using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using EnergyHost.Contract;
using EnergyHost.Model.EnergyModels.Status;

namespace EnergyHost.Services.Services.AlertServices
{
    public class TimeAlertService : ITimeAlertService
    {
        private readonly ISystemStatusService _statusService;

        public TimeAlertService(ISystemStatusService statusService)
        {
            _statusService = statusService;
            _statusService.StatusUpdatedEvent += _statusService_StatusUpdatedEvent;
        }

        private void _statusService_StatusUpdatedEvent(object sender, Model.EnergyModels.Status.Base.StatusUpdatedEventArgs e)
        {
            if (e.StatusName == nameof(TimeStatus))
            {
                _timeThreshold();
            }
        }

        void _timeThreshold()
        {
            var time = _statusService.GetStatus<TimeStatus>();
            Debug.WriteLine($"Time Pump status update {time.latest.CurrentDateTime}");
        }
    }
}
