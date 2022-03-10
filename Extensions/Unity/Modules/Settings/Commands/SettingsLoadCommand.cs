using System;
using Build1.PostMVC.Extensions.MVCS.Commands;
using Build1.PostMVC.Extensions.MVCS.Events;
using Build1.PostMVC.Extensions.MVCS.Injection;
using Build1.PostMVC.Extensions.Unity.Modules.Logging;

namespace Build1.PostMVC.Extensions.Unity.Modules.Settings.Commands
{
    public sealed class SettingsLoadCommand : Command
    {
        [Log(LogLevel.All)] public ILog                Log                { get; set; }
        [Inject]            public IEventDispatcher    Dispatcher         { get; set; }
        [Inject]            public ISettingsController SettingsController { get; set; }

        public override void Execute()
        {
            if (SettingsController.IsLoaded)
            {
                Log.Debug("Settings already loaded");
                return;
            }

            Log.Debug("Loading settings...");

            Retain();

            Dispatcher.AddListener(SettingsEvent.LoadSuccess, OnSuccess);
            Dispatcher.AddListener(SettingsEvent.LoadFail, OnFail);

            SettingsController.Load();
        }

        private void OnSuccess()
        {
            Log.Debug("Done");

            Dispatcher.RemoveListener(SettingsEvent.LoadSuccess, OnSuccess);
            Dispatcher.RemoveListener(SettingsEvent.LoadFail, OnFail);

            Release();
        }

        private void OnFail(Exception exception)
        {
            Log.Error(exception);

            Dispatcher.RemoveListener(SettingsEvent.LoadSuccess, OnSuccess);
            Dispatcher.RemoveListener(SettingsEvent.LoadFail, OnFail);

            Fail(exception);
        }
    }
}