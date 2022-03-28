using System;
using Build1.PostMVC.Extensions.MVCS.Commands;
using Build1.PostMVC.Extensions.MVCS.Events;
using Build1.PostMVC.Extensions.MVCS.Injection;
using Build1.PostMVC.Extensions.Unity.Modules.Logging;

namespace Build1.PostMVC.Extensions.Unity.Modules.Settings.Commands
{
    public sealed class SettingsUnloadCommand : Command
    {
        [Log(LogLevel.Warning)] public ILog                Log                { get; set; }
        [Inject]                public IEventDispatcher    Dispatcher         { get; set; }
        [Inject]                public ISettingsController SettingsController { get; set; }

        public override void Execute()
        {
            Log.Debug("Execute");

            Retain();

            Dispatcher.AddListener(SettingsEvent.UnloadSuccess, OnSuccess);
            Dispatcher.AddListener(SettingsEvent.UnloadFail, OnFail);

            SettingsController.Unload();
        }

        private void OnSuccess()
        {
            Log.Debug("OnSuccess");

            Dispatcher.RemoveListener(SettingsEvent.UnloadSuccess, OnSuccess);
            Dispatcher.RemoveListener(SettingsEvent.UnloadFail, OnFail);

            Release();
        }

        private void OnFail(Exception exception)
        {
            Log.Error(exception);

            Dispatcher.RemoveListener(SettingsEvent.UnloadSuccess, OnSuccess);
            Dispatcher.RemoveListener(SettingsEvent.UnloadFail, OnFail);

            Fail(exception);
        }
    }
}