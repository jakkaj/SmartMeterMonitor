using System;
using System.Collections.Generic;
using System.Text;

namespace EnergyHost.Contract
{
    public interface ILogService
    {
        void WriteError(string error);
        void WriteWarning(string warning);
        void WriteLog(string log);
        void WriteDebug(string message);
        void WriteError(Exception ex);
    }
}
