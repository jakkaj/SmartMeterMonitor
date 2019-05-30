using System;
using EnergyHost.Contract;
using EnergyHost.Model.Settings;
using Microsoft.Extensions.Options;

namespace EnergyHost.Services.Services
{
    public class LogService : ILogService
    {
        private readonly bool _supress;

        public LogService(IOptions<EnergyHostSettings> options)
        {
            var opts = options.Value;
            _supress = opts.SuppressWarning;
        }

        public void WriteError(string error)
        {
            _print("ERROR", error, ConsoleColor.Red);
        }

        public void WriteWarning(string warning)
        {
            if (!_supress)
                _print("WARNING", warning, ConsoleColor.Yellow);
        }

        public void WriteLog(string log)
        {
            if (!_supress)
                _print("INFO", log, ConsoleColor.Cyan);
        }

        void _print(string type, string message, ConsoleColor color)
        {
            var timestamp = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
            Console.ForegroundColor = color;
            Console.Write(type);
            Console.ResetColor();
            Console.Write($" [{timestamp}]: {message}");
            Console.WriteLine();
        }
    }
}