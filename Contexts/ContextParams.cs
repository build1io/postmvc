using Build1.PostMVC.Core.MVCS.Injection;
using Build1.PostMVC.Core.MVCS.Mediation;

namespace Build1.PostMVC.Core.Contexts
{
    public sealed class ContextParams
    {
        public string          name;
        public MediationParams mediationParams;
        public InjectionParams injectionParams;

        public ContextParams() { }

        public ContextParams(string name)
        {
            this.name = name;
        }

        public ContextParams(InjectionParams injectionParams)
        {
            this.injectionParams = injectionParams;
        }

        public ContextParams(InjectionParams injectionParams, MediationParams mediationParams)
        {
            this.injectionParams = injectionParams;
            this.mediationParams = mediationParams;
        }
    }
}