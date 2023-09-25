using Build1.PostMVC.Core.MVCS.Injection;
using Build1.PostMVC.Core.MVCS.Mediation;

namespace Build1.PostMVC.Core.Contexts
{
    public sealed class ContextParams
    {
        public readonly string          name;
        public readonly MediationMode   mediationMode;
        public readonly InjectionParams injectionParams;

        public ContextParams() { }

        public ContextParams(string name)
        {
            this.name = name;
        }
        
        public ContextParams(InjectionParams injectionParams)
        {
            this.injectionParams = injectionParams;
        }
    }
}