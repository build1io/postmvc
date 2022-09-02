using System;
using Build1.PostMVC.Core.Extensions;
using Build1.PostMVC.Core.Modules;

namespace Build1.PostMVC.Core.Contexts
{
    public interface IContext
    {
        string Name          { get; }
        bool   IsRootContext { get; }
        bool   IsStarted     { get; }
        bool   IsQuitting    { get; }
        bool   IsStopping    { get; }

        event Action<Module> OnModuleConstructing;
        event Action<Module> OnModuleDisposing;

        event Action OnStarting;
        event Action OnStarted;
        event Action OnQuitting;
        event Action OnStopping;
        event Action OnStopped;

        IContext AddExtension(Extension extension);
        IContext AddExtension<T>() where T : Extension, new();
        bool     HasExtension<T>() where T : Extension;
        T        GetExtension<T>() where T : Extension;
        bool     TryGetExtension<T>(out T extension) where T : Extension;

        IContext AddModule<T>() where T : Module, new();

        void Start();
        void SetQuitting();
        void Stop();
    }
}