using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EnergyHost.Model.EnergyModels.Status.Base;

namespace EnergyHost.Contract
{
    public interface ISystemStatusService
    {
        Task SendStatus(StatusBase status);

        (T latest, List<T> history) GetStatus<T>()
            where T:StatusBase;

        event EventHandler<StatusUpdatedEventArgs> StatusUpdatedEvent;
    }
}