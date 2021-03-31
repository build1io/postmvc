using Build1.PostMVC.Contexts;
using Build1.PostMVC.Extensions.MVCS.Events;
using Build1.PostMVC.Extensions.MVCS.Injection;
using Build1.PostMVC.Extensions.Unity.Modules.Agents;
using Build1.PostMVC.Extensions.Unity.Modules.Logging;
using UnityEngine;
using UnityEngine.SceneManagement;
using Logger = Build1.PostMVC.Extensions.Unity.Modules.Logging.Logger;
using ILogger = Build1.PostMVC.Extensions.Unity.Modules.Logging.ILogger;

namespace Build1.PostMVC.Extensions.Unity.Modules.App.Impl
{
    public sealed class AppController : IAppController
    {
        public const string BuildNumberFileName = "build-number";

        [Logger(LogLevel.Verbose)] public  ILogger           Logger           { get; set; }
        [Inject]                   public  IEventDispatcher  Dispatcher       { get; set; }
        [Inject]                   public  IContext          Context          { get; set; }
        [Inject]                   private IAgentsController AgentsController { get; set; }

        public string PersistentDataPath { get; private set; }
        public string Version            => Application.version;
        public int    BuildNumber        { get; private set; }

        public bool IsPaused  { get; private set; }
        public bool IsFocused { get; private set; }

        private AppAgent _agent;
        private string   _mainSceneName;

        [PostConstruct]
        private void PostConstruct()
        {
            PersistentDataPath = GetPersistentDataPath(PathAttribute.Internal | PathAttribute.Persistent | PathAttribute.Canonical);
            BuildNumber = GetBuildNumber();
            
            _mainSceneName = GetMainSceneName();

            _agent = AgentsController.Create<AppAgent>();
            _agent.Pause += OnPause;
            _agent.Focus += OnFocus;
            _agent.Quitting += OnQuitting;

            Logger.Debug(() => $"MainScene: \"{_mainSceneName}\" BuildNumber: {BuildNumber}");
        }

        [PreDestroy]
        private void PreDestroy()
        {
            _agent.Pause -= OnPause;
            _agent.Focus -= OnFocus;
            _agent.Quitting -= OnQuitting;
            AgentsController.Destroy(ref _agent);
        }

        /*
         * Public.
         */

        public void Restart()
        {
            // Signalling that app is about to restart.
            Dispatcher.Dispatch(AppEvent.Restarting);

            // Stopping the root context.
            // It must stop all nested all contexts and dispose all dependencies.
            Context.RootContext.Stop();

            // Reloading the main scene.
            SceneManager.LoadScene(_mainSceneName);
        }

        /*
         * Private.
         */

        private string GetPersistentDataPath(PathAttribute pathAttribute)
        {
            #if UNITY_ANDROID && !UNITY_EDITOR

            // Automatic => /storage/emulated/0/Android/data/[PACKAGE_NAME]/files: OK

            // Internal, Persistent, Absolute => /data/user/0/[PACKAGE_NAME]/files: OK
            // Internal, Persistent, Default => /data/user/0/[PACKAGE_NAME]/files: OK
            // Internal, Persistent, Canonical => /data/data/[PACKAGE_NAME]/files: OK

            // Internal, Cached, Absolute => /data/user/0/[PACKAGE_NAME]/cache: OK
            // Internal, Cached, Default => /data/user/0/[PACKAGE_NAME]/cache: OK
            // Internal, Cached, Canonical => /data/data/[PACKAGE_NAME]/cache: OK

            // External, Persistent, Absolute => /storage/emulated/0/Android/data/[PACKAGE_NAME]/files: OK
            // External, Persistent, Default => /storage/emulated/0/Android/data/[PACKAGE_NAME]/files: OK
            // External, Persistent, Canonical => /storage/emulated/0/Android/data/[PACKAGE_NAME]/files: OK

            // External, Cached, Absolute => /storage/emulated/0/Android/data/[PACKAGE_NAME]/cache: OK
            // External, Cached, Default => /storage/emulated/0/Android/data/[PACKAGE_NAME]/cache: OK
            // External, Cached, Canonical => /storage/emulated/0/Android/data/[PACKAGE_NAME]/cache: OK

            if (pathAttribute == PathAttribute.Automatic)
                return Application.persistentDataPath;
            
            var activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            var activity = activityClass.GetStatic<AndroidJavaObject>("currentActivity");

            var endMethod = string.Empty;
            if ((pathAttribute & PathAttribute.Absolute) == PathAttribute.Absolute)
                endMethod = "getAbsolutePath";
            else if ((pathAttribute & PathAttribute.Canonical) == PathAttribute.Canonical)
                endMethod = "getCanonicalPath";
            else if ((pathAttribute & PathAttribute.Default) == PathAttribute.Default)
                endMethod = "getPath";

            if ((pathAttribute & PathAttribute.Internal) == PathAttribute.Internal && (pathAttribute & PathAttribute.Persistent) == PathAttribute.Persistent)
                return activity.Call<AndroidJavaObject>("getFilesDir").Call<string>(endMethod);
            
            if ((pathAttribute & PathAttribute.Internal) == PathAttribute.Internal && (pathAttribute & PathAttribute.Cached) == PathAttribute.Cached)
                return activity.Call<AndroidJavaObject>("getCacheDir").Call<string>(endMethod);
            
            if ((pathAttribute & PathAttribute.External) == PathAttribute.External && (pathAttribute & PathAttribute.Persistent) == PathAttribute.Persistent)
                return activity.Call<AndroidJavaObject>("getExternalFilesDir", (object)null).Call<string>(endMethod);
            
            if ((pathAttribute & PathAttribute.External) == PathAttribute.External && (pathAttribute & PathAttribute.Cached) == PathAttribute.Cached)
                return activity.Call<AndroidJavaObject>("getExternalCacheDir").Call<string>(endMethod);
            
            throw new ArgumentOutOfRangeException(nameof(pathAttribute), pathAttribute, null);
            
            #else
            
            // PathAttributes are applicable only to Android platform at the moment.
            return Application.persistentDataPath;
            
            #endif
        }
        
        private string GetMainSceneName()
        {
            return System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(0));
        }

        /*
         * Event handlers.
         */

        private void OnPause(bool paused)
        {
            if (IsPaused == paused)
                return;

            Logger.Debug(() => $"OnPause({paused})");

            IsPaused = paused;
            Dispatcher.Dispatch(AppEvent.Pause, paused);
        }

        private void OnFocus(bool focused)
        {
            if (IsFocused == focused)
                return;
            
            Logger.Debug(() => $"OnFocus({focused})");

            IsFocused = focused;
            Dispatcher.Dispatch(AppEvent.Focus, focused);
        }

        private void OnQuitting()
        {
            Dispatcher.Dispatch(AppEvent.Quitting);
        }

        /*
         * Static.
         */

        public static int GetBuildNumber()
        {
            var text = Resources.Load<TextAsset>(BuildNumberFileName);
            if (text != null && int.TryParse(text.text, out var buildNumber))
                return buildNumber;
            return 0;
        }
    }
}