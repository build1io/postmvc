using System;

namespace Build1.PostMVC.Core.MVCS.Injection
{
    public sealed class BindingException : Exception
    {
        public BindingException(BindingExceptionType type) : base(type.ToString())
        {
        }
        
        public BindingException(BindingExceptionType type, object data) : base($"{type} [{data}]")
        {
        }
        
        public BindingException(BindingExceptionType type, object data1, object data2) : base($"{type} [{data1}] [{data2}]")
        {
        }
    }
}