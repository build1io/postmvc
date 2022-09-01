using System;

namespace Build1.PostMVC.Core.MVCS.Injection
{
    public sealed class InjectionException : Exception
    {
        public InjectionException(InjectionExceptionType type) : base(type.ToString())
        {
        }
        
        public InjectionException(InjectionExceptionType type, object data) : base($"{type} [{data}]")
        {
        }
        
        public InjectionException(InjectionExceptionType type, object data1, object data2) : base($"{type} [{data1}] [{data2}]")
        {
        }
    }
}