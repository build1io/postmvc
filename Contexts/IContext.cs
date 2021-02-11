using System;
using Build1.PostMVC.Extensions;
using Build1.PostMVC.Modules;

namespace Build1.PostMVC.Contexts
{
    public interface IContext
    {
        IContext RootContext { get; }

        bool IsRootContext { get; }
        bool IsStarted     { get; }
        bool IsStopping    { get; }

        Action OnStart { get; set; }
        Action OnStop  { get; set; }

        IContext AddExtension(IExtension extension);
        IContext AddExtension<T>() where T : IExtension, new();
        bool     HasExtension<T>() where T : IExtension;
        T        GetExtension<T>() where T : IExtension;
        bool     TryGetExtension<T>(out T extension) where T : IExtension;

        IContext AddModule<T>() where T : IModule, new();

        IContext Start();
        void     Stop();
    }
}