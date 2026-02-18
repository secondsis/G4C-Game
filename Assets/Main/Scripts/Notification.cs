using System;

namespace Main.Scripts
{
    public class Notification
    {
        private String message;
        private NotificationLevel notificationLevel;

        public Notification (string msg, NotificationLevel level)
        {
            message = msg;
            notificationLevel = level;
        }
    }

    public enum NotificationLevel
    {
        INFO,
        WARNING
    }
}