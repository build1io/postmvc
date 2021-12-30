namespace Build1.PostMVC.Extensions.Unity.Modules.Notifications
{
    public interface INotificationsController
    {
        void Initialize();
        void SetEnabled(bool enabled);
        
        void ScheduleNotification(Notification notification);

        void CancelScheduledNotification(string id);
        void CancelScheduledNotification(Notification notification);
        void CancelAllScheduledNotifications();

        void CleanDisplayedNotifications();
    }
}