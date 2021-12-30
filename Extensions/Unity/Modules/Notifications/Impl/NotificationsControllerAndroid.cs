#if UNITY_ANDROID

using System;
using Build1.PostMVC.Extensions.MVCS.Events;
using Build1.PostMVC.Extensions.MVCS.Injection;
using Build1.PostMVC.Extensions.Unity.Modules.Logging;
using Unity.Notifications.Android;

namespace Modules.Notifications.Impl.Android
{
    internal sealed class NotificationsControllerAndroid : INotificationsController
    {
        private const string     DefaultChannelId          = "main";
        private const string     DefaultChannelName        = "Main Channel";
        private const string     DefaultChannelDescription = "Main notifications channel";
        private const Importance DefaultChannelImportance  = Importance.High;
        private const string     DefaultIcon               = "main";

        [Log(LogLevel.Warning)] public ILog             Log        { get; set; }
        [Inject]                public IEventDispatcher Dispatcher { get; set; }

        public bool Initialized { get; private set; }
        public bool Enabled     { get; private set; }

        /*
         * Initialization.
         */

        public void Initialize()
        {
            if (Initialized)
            {
                Log.Warn("Already initialized.");
                return;
            }

            var channel = new AndroidNotificationChannel
            {
                Id = DefaultChannelId,
                Name = DefaultChannelName,
                Importance = DefaultChannelImportance,
                Description = DefaultChannelDescription,
            };
            AndroidNotificationCenter.RegisterNotificationChannel(channel);

            Initialized = true;
            Dispatcher.Dispatch(NotificationsEvent.Initialized);
        }

        /*
         * Public.
         */

        public void SetEnabled(bool enabled)
        {
            Enabled = enabled;
        }

        public void ScheduleNotification(Notification notification)
        {
            if (!Initialized)
            {
                Log.Error("Notification not initialized.");
                return;
            }

            if (!Enabled)
            {
                Log.Debug("Notifications disabled.");
                return;
            }

            var androidNotification = new AndroidNotification();
            androidNotification.Title = notification.title;
            androidNotification.Text = notification.text;

            if (notification.largeIcon != null)
                androidNotification.LargeIcon = notification.largeIcon;
            else if (notification.smallIcon != null)
                androidNotification.SmallIcon = notification.smallIcon;
            else
                androidNotification.LargeIcon = DefaultIcon;

            androidNotification.FireTime = DateTime.Now.AddSeconds(notification.timeoutSeconds);

            AndroidNotificationCenter.SendNotification(androidNotification, DefaultChannelId);
        }

        public void CancelScheduledNotifications()
        {
            if (!Initialized)
            {
                Log.Error("Notification not initialized.");
                return;
            }

            if (!Enabled)
            {
                Log.Debug("Notifications disabled.");
                return;
            }

            AndroidNotificationCenter.CancelAllScheduledNotifications();
        }

        public void ClearDisplayedNotifications()
        {
            if (!Initialized)
            {
                Log.Error("Notification not initialized.");
                return;
            }

            if (!Enabled)
            {
                Log.Debug("Notifications disabled.");
                return;
            }

            AndroidNotificationCenter.CancelAllDisplayedNotifications();
        }
    }
}

#endif