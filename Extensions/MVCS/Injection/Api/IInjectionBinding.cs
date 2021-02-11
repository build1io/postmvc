using System;
using Build1.PostMVC.Extensions.MVCS.Injection.Impl;

namespace Build1.PostMVC.Extensions.MVCS.Injection.Api
{
    public interface IInjectionBinding : IInjectionBindingAs, IInjectionBindingByAttribute
    {
        Type                 Key                { get; }
        object               Value              { get; }
        InjectionBindingType BindingType        { get; }
        InjectionMode        InjectionMode      { get; }
        bool                 ToConstructOnStart { get; }

        void SetValue(object value);
    }
}