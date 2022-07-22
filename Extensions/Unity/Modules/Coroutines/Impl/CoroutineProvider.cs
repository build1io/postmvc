using System.Collections;
using Build1.PostMVC.Extensions.ContextView.Contexts;
using Build1.PostMVC.Extensions.MVCS.Injection;
using UnityEngine;

namespace Build1.PostMVC.Extensions.Unity.Modules.Coroutines.Impl
{
    internal sealed class CoroutineProvider : ICoroutineProvider
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
            if (!_coroutineProvider)
                return;
            
            _coroutineProvider.StopAllCoroutines();
            _coroutineProvider = null;
        }
        
        /*
         * Public.
         */

        public Coroutine StartCoroutine(IEnumerator routine)
        {
            return _coroutineProvider.StartCoroutine(routine);
        }

        public void StartCoroutine(IEnumerator routine, out Coroutine coroutine)
        {
            coroutine = _coroutineProvider.StartCoroutine(routine);
        }

        public void StopCoroutine(Coroutine routine)
        {
            if (routine != null)
                _coroutineProvider.StopCoroutine(routine);
        }
        
        public void StopCoroutine(ref Coroutine routine)
        {
            if (routine == null) 
                return;
            
            if (_coroutineProvider)
                _coroutineProvider.StopCoroutine(routine);
            
            routine = null;
        }
    }
}