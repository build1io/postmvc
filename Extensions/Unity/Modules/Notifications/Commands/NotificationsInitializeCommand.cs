using Build1.PostMVC.Extensions.MVCS.Commands;
using Build1.PostMVC.Extensions.MVCS.Events;
using Build1.PostMVC.Extensions.MVCS.Injection;

namespace Build1.PostMVC.Extensions.Unity.Modules.Notifications.Commands
{
    public sealed class NotificationsInitializeCommand : Command
    {
        [Inject] public IEventDispatcher         Dispatcher              { get; set; }
        [Inject] public INotificationsController NotificationsController { get; set; }

        public override void Execute()
        {
            Retain();

            Dispatcher.AddListenerOnce(NotificationsEvent.Initialized, Release);

            NotificationsController.Initialize();
        }
    }
}