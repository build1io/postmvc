using System;
using Build1.PostMVC.Core.MVCS.Mediation.Api;

namespace Build1.PostMVC.Core.MVCS.Mediation
{
    public interface IMediationBinder
    {
        IMediationBindingTo Bind<T>() where T : class, IView;
        IMediationBindingTo Bind<T, I>() where T : class, I where I : class, IView;
        
        IMediationBindingTo Bind(Type viewType);
        IMediationBindingTo Bind(Type viewType, Type viewInterfaceType);
        
        IMediationBindingTo Rebind(Type viewType);
        IMediationBindingTo Rebind(Type viewType, Type viewInterfaceType);
        
        void Unbind(Type viewType);
        
        void OnViewAdd(IView view);
        void OnViewRemove(IView view);

        void UpdateViewInjections(IView view);
    }
}