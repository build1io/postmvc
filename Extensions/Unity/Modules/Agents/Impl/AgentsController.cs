using Build1.PostMVC.Extensions.ContextView.Context;
using Build1.PostMVC.Extensions.MVCS.Injection;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Build1.PostMVC.Extensions.Unity.Modules.Agents.Impl
{
    internal sealed class AgentsController : IAgentsController
    {
        [Inject] public IContextView     ContextView     { get; set; }
        [Inject] public IInjectionBinder InjectionBinder { get; set; }

        private GameObject _agentsRoot;

        [PostConstruct]
        public void PostConstruct()
        {
            _agentsRoot = CreateAgentsRoot();
        }

        /*
         * Public.
         */

        public T Create<T>() where T : MonoBehaviour
        {
            var agent = _agentsRoot.AddComponent<T>(); 
            InjectionBinder.Construct(agent, true);
            return agent;
        }

        public void Destroy<T>(T agent) where T : MonoBehaviour
        {
            if (agent == null || !_agentsRoot.TryGetComponent(agent.GetType(), out var component)) 
                return;
            InjectionBinder.Destroy(agent, true);
            Object.Destroy(component);
        }

        public void Destroy<T>(ref T agent) where T : MonoBehaviour
        {
            Destroy(agent);
            agent = default;
        }

        /*
         * Private.
         */

        private GameObject CreateAgentsRoot()
        {
            var root = new GameObject("Agents");
            root.transform.SetParent(ContextView.As<Transform>());
            return root;
        }
    }
}