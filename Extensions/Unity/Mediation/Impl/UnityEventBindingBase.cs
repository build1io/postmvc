using Build1.PostMVC.Extensions.MVCS.Events;

namespace Build1.PostMVC.Extensions.Unity.Mediation.Impl
{
    public abstract class UnityEventBindingBase
    {
        protected IEventDispatcher _dispatcher;
        
        protected UnityEventBindingBase(IEventDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }
        
        public abstract void Destroy();
    }
}