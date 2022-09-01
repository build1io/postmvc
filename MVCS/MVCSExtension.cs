using Build1.PostMVC.Core.Extensions;
using Build1.PostMVC.Core.Modules;
using Build1.PostMVC.Core.MVCS.Commands;
using Build1.PostMVC.Core.MVCS.Commands.Impl;
using Build1.PostMVC.Core.MVCS.Contexts;
using Build1.PostMVC.Core.MVCS.Events;
using Build1.PostMVC.Core.MVCS.Events.Impl;
using Build1.PostMVC.Core.MVCS.Events.Impl.Bus;
using Build1.PostMVC.Core.MVCS.Events.Impl.Map;
using Build1.PostMVC.Core.MVCS.Injection;
using Build1.PostMVC.Core.MVCS.Injection.Impl;
using Build1.PostMVC.Core.MVCS.Mediation;
using Build1.PostMVC.Core.MVCS.Mediation.Impl;

namespace Build1.PostMVC.Core.MVCS
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

            InjectionBinder.Bind(Context);
            InjectionBinder.Bind(EventDispatcher);
            InjectionBinder.Bind(InjectionBinder);
            InjectionBinder.Bind(CommandBinder).ConstructValue();
            InjectionBinder.Bind(MediationBinder);
            
            InjectionBinder.Bind<IEventBus, EventBus>();
            InjectionBinder.Bind<IEventMap, EventMapProvider, Inject>();
        }

        public override void Dispose()
        {
            Context.OnStarting -= OnContextStarting;
            Context.OnStarted -= OnContextStarted;
            Context.OnQuitting -= OnContextQuitting;
            Context.OnStopped -= OnContextStopped;

            Context.OnModuleConstructing -= OnModuleConstructing;
            Context.OnModuleDisposing -= OnModuleDisposing;

            ((CommandBinder)CommandBinder).UnbindAll();
            ((InjectionBinder)InjectionBinder).UnbindAll();
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
            ((CommandBinder)CommandBinder).UnbindOnQuit();
        }

        private void OnContextStopped()
        {
            InjectionBinder.GetInstance<IEventDispatcher>().Dispatch(ContextEvent.Stopped);
        }

        /*
         * Modules.
         */

        private void OnModuleConstructing(Module module) { InjectionBinder.Construct(module, true); }
        private void OnModuleDisposing(Module module)    { InjectionBinder.Destroy(module, true); }
    }
}