using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EnergyHost.Contract.Devices;
using EnergyHost.Model.DeviceModels;

namespace EnergyHost.Devices.TPLink
{
    /// <summary>
    /// Big thanks to https://github.com/iqmeta/tplink-smartplug
    /// </summary>
    public class TPLinkPlugDevice : ISwitchDevice
    {
        private readonly DeviceSettings _settings;

        public string Name => _settings.DeviceName;
        public string Type { get; }
        public string DeviceType => _settings.DeviceType;

        public TPLinkPlugDevice(DeviceSettings settings)
        {
            _settings = settings;
        }

        async void _getStatus()
        {

        }

        public bool IsOn { get; set; }
        public Task SwitchOff()
        {
            throw new NotImplementedException();
        }

        public Task SwitchOn()
        {
            throw new NotImplementedException();
        }
    }
}
