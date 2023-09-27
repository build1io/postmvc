using System;
using System.Collections.Generic;
using Build1.PostMVC.Core.MVCS.Injection;
using Build1.PostMVC.Core.MVCS.Mediation;

namespace Build1.PostMVC.Core.Contexts
{
    public sealed class ContextParams
    {
        public string                  name;
        public MediationParams         mediationParams;
        public ReflectionParams        reflectionParams;
        public Func<IEnumerable<Type>> assemblyTypesGetter;

        public ContextParams() { }

        public ContextParams(string name)
        {
            this.name = name;
        }

        public ContextParams(ReflectionParams reflectionParams)
        {
            this.reflectionParams = reflectionParams;
        }

        public ContextParams(ReflectionParams reflectionParams, MediationParams mediationParams)
        {
            this.reflectionParams = reflectionParams;
            this.mediationParams = mediationParams;
        }
    }
}