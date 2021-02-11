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

        public string Version     => Application.version;
        public int    BuildNumber { get; private set; }
        public bool   IsPaused    { get; private set; }

        private AppAgent _agent;
        private string   _mainSceneName;

        [PostConstruct]
        private void PostConstruct()
        {
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
            // Required for correctly work in Editor.
            if (!Application.isPlaying)
                return;

            Logger.Debug(() => $"OnFocus({focused})");
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