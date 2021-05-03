using DevIO.Business.Notifications;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevIO.Business.Interfaces
{
    public interface INotificator
    {
        bool HasNotification();

        List<Notification> GetNotifications();

        void Handle(Notification notification);
    }
}
