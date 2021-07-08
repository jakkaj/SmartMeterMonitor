using EnergyHost.Model.EnergyModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EnergyHost.Contract
{
    public interface IClipsalService
    {
        Task<List<ClipsalUsage>> Get();
    }
}
