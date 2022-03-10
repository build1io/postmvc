namespace Build1.PostMVC.Extensions.Unity.Modules.Notifications
{
    public interface INotificationsController
    {
        bool Initialized { get; }
        
        void Initialize();
        void SetEnabled(bool enabled);
        
        void ScheduleNotification(Notification notification);

        void CancelScheduledNotification(Notification notification);
        void CancelAllScheduledNotifications();

        void CleanDisplayedNotifications();
    }
}