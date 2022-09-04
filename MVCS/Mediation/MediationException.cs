using System;

namespace Build1.PostMVC.Core.MVCS.Mediation
{
    public sealed class MediationException : Exception
    {
        public MediationException(MediationExceptionType type) : base(type.ToString())
        {
        }
        
        public MediationException(MediationExceptionType type, object data) : base($"{type} [{data}]")
        {
        }
        
        public MediationException(MediationExceptionType type, object data1, object data2) : base($"{type} [{data1}] [{data2}]")
        {
        }
    }
}