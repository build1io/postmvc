using System;

namespace Build1.PostMVC.Core.Extensions.MVCS.Mediation.Api
{
    public interface IMediationBindingTo
    {
        IMediationBinding To<T>() where T : class, IMediator, new();
        IMediationBinding To(Type mediatorType);
    }
}