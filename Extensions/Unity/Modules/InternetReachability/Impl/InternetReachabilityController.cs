using System;
using System.Collections;
using Build1.PostMVC.Extensions.ContextView.Contexts;
using Build1.PostMVC.Extensions.MVCS.Injection;
using UnityEngine;
using UnityEngine.Networking;

namespace Build1.PostMVC.Extensions.Unity.Modules.InternetReachability.Impl
{
    public sealed class InternetReachabilityController : IInternetReachabilityController
    {
        [Inject] public IContextView ContextView { get; set; }

        private MonoBehaviour _coroutineProvider;
        
        [PostConstruct]
        public void PostConstruct()
        {
            _coroutineProvider = ContextView.As<MonoBehaviour>();
        }

        [PreDestroy]
        public void PreDestroy()
        {
            _coroutineProvider = null;
        }
        
        /*
         * Public.
         */
        
        public void Check(Action<bool> onComplete)
        {
            _coroutineProvider.StartCoroutine(CheckImpl(onComplete));
        }
        
        /*
         * Private.
         */
        
        private IEnumerator CheckImpl(Action<bool> onComplete)
        {
            bool result;
            using (var request = UnityWebRequest.Head("https://google.com"))
            {
                request.timeout = 5;
                yield return request.SendWebRequest();
                result = request.responseCode == 200;
            }
            
            if (_coroutineProvider != null)
                onComplete.Invoke(result);
        }
    }
}