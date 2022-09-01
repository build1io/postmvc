using System;
using Build1.PostMVC.Core.Extensions.MVCS.Injection.Impl;

namespace Build1.PostMVC.Core.Extensions.MVCS.Injection
{
    public interface IInjectionBinding
    {
        Type                 Key                { get; }
        object               Value              { get; }
        InjectionBindingType BindingType        { get; }
        InjectionMode        InjectionMode      { get; }
        bool                 ToConstruct        { get; }
        bool                 ToConstructOnStart { get; }

        void SetValue(object value);
    }
}