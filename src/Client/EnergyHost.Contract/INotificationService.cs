﻿using System.Threading.Tasks;

namespace EnergyHost.Contract
{
    public interface INotificationService
    {
        Task SendNotification(string text, string title = null);
        Task SendPrice(double price);
    }
}