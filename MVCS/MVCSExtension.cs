using System;
using System.Collections.Generic;
using System.Linq;
using Build1.PostMVC.Core.Contexts;
using Build1.PostMVC.Core.Extensions;
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
using Module = Build1.PostMVC.Core.Modules.Module;

namespace Build1.PostMVC.Core.MVCS
{
    public sealed class MVCSExtension : Extension
    {
        public IEventDispatcher    EventDispatcher    { get; private set; }
        public IInjectionBinder    InjectionBinder    { get; private set; }
        public IInjectionReflector InjectionReflector { get; private set; }
        public ICommandBinder      CommandBinder      { get; private set; }
        public IMediationBinder    MediationBinder    { get; private set; }

        public override void Initialize()
        {
            CommandBinder = new CommandBinder();
            EventDispatcher = new EventDispatcherWithCommandProcessing((CommandBinder)CommandBinder);
            InjectionReflector = new InjectionReflector();
            InjectionBinder = new InjectionBinder(InjectionReflector);
            MediationBinder = new MediationBinder(Context.Params.mediationParams, InjectionBinder);

            Context.OnStarting += OnContextStarting;
            Context.OnStarted += OnContextStarted;
            Context.OnQuitting += OnContextQuitting;
            Context.OnStopped += OnContextStopped;

            Context.OnModuleConstructing += OnModuleConstructing;
            Context.OnModuleDisposing += OnModuleDisposing;

            InjectionBinder.Bind(Context);
            InjectionBinder.Bind(EventDispatcher);
            InjectionBinder.Bind(InjectionReflector);
            InjectionBinder.Bind(InjectionBinder);
            InjectionBinder.Bind(CommandBinder).ConstructValue();
            InjectionBinder.Bind(MediationBinder);

            InjectionBinder.Bind<IEventBus, EventBus>();
            InjectionBinder.Bind<IEventMapCore, EventMapProvider, Inject>();
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

        private void OnContextStarting(IContext context)
        {
            var prepareMediatorsReflectionData = (Context.Params.reflectionParams & ReflectionParams.PrepareMediatorsReflectionInfoOnContextStart) == ReflectionParams.PrepareMediatorsReflectionInfoOnContextStart;
            var prepareViewsReflectionData = (Context.Params.reflectionParams & ReflectionParams.PrepareViewsReflectionInfoOnContextStart) == ReflectionParams.PrepareViewsReflectionInfoOnContextStart;
            
            if (prepareMediatorsReflectionData || prepareViewsReflectionData)
            {
                IEnumerable<Type> types;

                if (Context.Params.assemblyTypesGetter != null)
                {
                    types = Context.Params.assemblyTypesGetter.Invoke();
                }
                else
                {
                    var assembly = AppDomain.CurrentDomain.GetAssemblies().First(assembly => assembly.GetName().Name == "Assembly-CSharp");
                    types = assembly.GetTypes();
                }
                
                foreach (var type in types)
                {
                    if (!type.IsSealed)
                        continue;
                    
                    if (prepareMediatorsReflectionData && typeof(Mediator).IsAssignableFrom(type))
                        InjectionReflector.Get(type);
                    else if (prepareViewsReflectionData && typeof(IView).IsAssignableFrom(type))
                        InjectionReflector.Get(type);
                }
            }

            var prepareCommandsReflectionInfo = (Context.Params.reflectionParams & ReflectionParams.PrepareCommandsReflectionInfoOnContextStart) == ReflectionParams.PrepareCommandsReflectionInfoOnContextStart;
            if (prepareCommandsReflectionInfo)
            {
                CommandBinder.ForEachBinding(binding =>
                {
                    foreach (var commandType in binding.Commands)
                        InjectionReflector.Get(commandType);
                });
            }
            
            var prepareBindingsReflectionInfo = (Context.Params.reflectionParams & ReflectionParams.PrepareBindingsReflectionInfoOnContextStart) == ReflectionParams.PrepareBindingsReflectionInfoOnContextStart;

            InjectionBinder.ForEachBinding(binding =>
            {
                if (prepareBindingsReflectionInfo && binding.ToConstruct)
                {
                    Type type;
                    if (binding.Value is Type value)
                        type = value;
                    else
                        type = binding.Value.GetType();

                    InjectionReflector.Get(type);    
                }
                
                if (binding.ToConstructOnStart)
                    InjectionBinder.GetInstance(binding);
            });
        }

        private void OnContextStarted(IContext context)
        {
            EventDispatcher.Dispatch(ContextEvent.Started, context);
        }

        private void OnContextQuitting(IContext context)
        {
            ((CommandBinder)CommandBinder).UnbindOnQuit();
        }

        private void OnContextStopped(IContext context)
        {
            EventDispatcher.Dispatch(ContextEvent.Stopped, context);
        }

        /*
         * Modules.
         */

        private void OnModuleConstructing(Module module) { InjectionBinder.Construct(module, true); }
        private void OnModuleDisposing(Module module)    { InjectionBinder.Destroy(module, true); }
    }
}