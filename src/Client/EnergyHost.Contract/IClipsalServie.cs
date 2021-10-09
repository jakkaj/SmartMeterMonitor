using EnergyHost.Model.EnergyModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EnergyHost.Contract
{
    public interface IClipsalService
    {
        Task<List<ClipsalUsage>> Get(int days);
        List<ClipsalInflux> Compose(List<ClipsalUsage> usage);
        Task<ClipsalInstant> GetInstant();
        Task CheckOven(List<ClipsalInflux> clipsal);
    }
}
