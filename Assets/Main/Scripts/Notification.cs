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

        public String GetMessage()
        {
            return message;
        }

        public NotificationLevel GetNotificationLevel()
        {
            return notificationLevel;
        }
    }

    public enum NotificationLevel
    {
        INFO,
        WARNING
    }
}