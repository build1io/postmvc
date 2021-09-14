using Build1.PostMVC.Contexts;
using Build1.PostMVC.Extensions.MVCS.Commands;
using Build1.PostMVC.Extensions.MVCS.Commands.Impl;
using Build1.PostMVC.Extensions.MVCS.Contexts;
using Build1.PostMVC.Extensions.MVCS.Events;
using Build1.PostMVC.Extensions.MVCS.Events.Impl;
using Build1.PostMVC.Extensions.MVCS.Injection;
using Build1.PostMVC.Extensions.MVCS.Injection.Impl;
using Build1.PostMVC.Extensions.MVCS.Mediation;
using Build1.PostMVC.Extensions.MVCS.Mediation.Impl;
using Build1.PostMVC.Modules;

namespace Build1.PostMVC.Extensions.MVCS
{
    public sealed class MVCSExtension : Extension
    {
        public IEventDispatcher EventDispatcher { get; }
        public IInjectionBinder InjectionBinder { get; }
        public ICommandBinder   CommandBinder   { get; }
        public IMediationBinder MediationBinder { get; }

        public MVCSExtension() : this(MediationMode.Strict)
        {
        }

        public MVCSExtension(MediationMode mediationMode)
        {
            CommandBinder = new CommandBinder();
            EventDispatcher = new EventDispatcherWithCommandProcessing((CommandBinder)CommandBinder);
            InjectionBinder = new InjectionBinder();
            MediationBinder = new MediationBinder(mediationMode, InjectionBinder);
        }

        public override void Initialize()
        {
            Context.OnStarting += OnContextStarting;
            Context.OnStarted += OnContextStarted;
            Context.OnQuitting += OnContextQuitting;
            Context.OnStopped += OnContextStopped;

            Context.OnModuleConstructing += OnModuleConstructing;
            Context.OnModuleDisposing += OnModuleDisposing;

            InjectionBinder.Bind<IContext>().ToValue(Context);
            InjectionBinder.Bind<IEventDispatcher>().ToValue(EventDispatcher).ConstructValue();
            InjectionBinder.Bind<IEventMap>().ToProvider<EventMapProvider>();
            InjectionBinder.Bind<IInjectionBinder>().ToValue(InjectionBinder);
            InjectionBinder.Bind<ICommandBinder>().ToValue(CommandBinder).ConstructValue();
            InjectionBinder.Bind<IMediationBinder>().ToValue(MediationBinder);
        }

        public override void Dispose()
        {
            Context.OnStarting -= OnContextStarting;
            Context.OnStarted -= OnContextStarted;
            Context.OnQuitting -= OnContextQuitting;
            Context.OnStopped -= OnContextStopped;

            Context.OnModuleConstructing -= OnModuleConstructing;
            Context.OnModuleDisposing -= OnModuleDisposing;
            
            CommandBinder.UnbindAll();
            InjectionBinder.UnbindAll();
        }

        /*
         * Context.
         */

        private void OnContextStarting()
        {
            InjectionBinder.ForEachBinding(binding =>
            {
                if (binding.ToConstructOnStart)
                    InjectionBinder.GetInstance(binding);
            });
        }

        private void OnContextStarted()
        {
            InjectionBinder.GetInstance<IEventDispatcher>().Dispatch(ContextEvent.Started);
        }

        private void OnContextQuitting()
        {
            CommandBinder.UnbindOnQuit();
        }

        private void OnContextStopped()
        {
            InjectionBinder.GetInstance<IEventDispatcher>().Dispatch(ContextEvent.Stopped);
        }

        /*
         * Modules.
         */

        private void OnModuleConstructing(IModule module) { InjectionBinder.Construct(module, true); }
        private void OnModuleDisposing(IModule module)    { InjectionBinder.Destroy(module, true); }
    }
}