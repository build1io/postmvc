using System;
using Build1.PostMVC.Core.Extensions;
using Build1.PostMVC.Core.Modules;

namespace Build1.PostMVC.Core.Contexts
{
    public interface IContext
    {
        int           Id            { get; }
        string        Name          { get; }
        ContextParams Params        { get; }
        bool          IsRootContext { get; }
        bool          IsStarted     { get; }
        bool          IsQuitting    { get; }
        bool          IsStopping    { get; }

        event Action<IContext> OnStarting;
        event Action<IContext> OnStarted;
        event Action<IContext> OnQuitting;
        event Action<IContext> OnStopping;
        event Action<IContext> OnStopped;

        event Action<Module> OnModuleConstructing;
        event Action<Module> OnModuleDisposing;

        IContext AddExtension(Extension extension);
        IContext AddExtension<T>() where T : Extension, new();
        bool     HasExtension<T>() where T : Extension;
        T        GetExtension<T>() where T : Extension;
        bool     TryGetExtension<T>(out T extension) where T : Extension;

        IContext AddModule<T>() where T : Module, new();

        IContext Start();
        void     SetQuitting();
        void     Stop();
    }
}