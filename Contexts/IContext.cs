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
        bool IsQuitting    { get; }
        bool IsStopping    { get; }

        event Action<IModule> OnModuleConstructing;
        event Action<IModule> OnModuleDisposing;

        event Action OnStarting;
        event Action OnStarted;
        event Action OnQuitting;
        event Action OnStopping;
        event Action OnStopped;

        IContext AddExtension(IExtension extension);
        IContext AddExtension<T>() where T : IExtension, new();
        bool     HasExtension<T>() where T : IExtension;
        T        GetExtension<T>() where T : IExtension;
        bool     TryGetExtension<T>(out T extension) where T : IExtension;

        IContext AddModule<T>() where T : IModule, new();

        void Start();
        void SetQuitting();
        void Stop();
    }
}