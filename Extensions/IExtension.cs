using Build1.PostMVC.Contexts;
using Build1.PostMVC.Modules;

namespace Build1.PostMVC.Extensions
{
    public interface IExtension
    {
        IContext Context     { get; }
        IContext RootContext { get; }

        void SetContext(IContext context);
        void SetRootContext(IContext rootContext);

        void OnInitialized();
        void OnSetup();
        void OnDispose();
        
        void OnModuleConstructed(IModule module);
        void OnModuleDispose(IModule module);
        
        void OnContextStarting();
        void OnContextStarted();
        
        void OnContextStopping();
        void OnContextStopped();
    }
}