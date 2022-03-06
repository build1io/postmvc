using System;
using Build1.PostMVC.Extensions.MVCS.Events;

namespace Build1.PostMVC.Extensions.Unity.Modules.Settings
{
    public static class SettingsEvent
    {
        public static readonly Event            LoadSuccess = new Event();
        public static readonly Event<Exception> LoadFail    = new Event<Exception>();

        public static readonly Event            UnloadSuccess = new Event();
        public static readonly Event<Exception> UnloadFail    = new Event<Exception>();

        public static readonly Event<Setting> SettingChanged = new Event<Setting>();

        public static readonly Event<Exception> SaveFail = new Event<Exception>();

        public static readonly Event Reset = new Event();
    }
}