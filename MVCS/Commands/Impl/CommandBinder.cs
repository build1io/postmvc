using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using Build1.PostMVC.Core.MVCS.Events;
using Build1.PostMVC.Core.MVCS.Events.Impl;
using Build1.PostMVC.Core.MVCS.Injection;
using Build1.PostMVC.Core.Utils.Pooling;
using Event = Build1.PostMVC.Core.MVCS.Events.Event;

namespace Build1.PostMVC.Core.MVCS.Commands.Impl
{
    public sealed class CommandBinder : ICommandBinder
    {
        [Inject] public IEventDispatcher Dispatcher      { get; set; }
        [Inject] public IInjectionBinder InjectionBinder { get; set; }

        private readonly CommandParams NoParams = new();

        private readonly Dictionary<EventBase, IList> _bindings;
        private readonly List<CommandBindingBase>     _bindingsToUnbind;

        private readonly Pool<CommandBase>      _commandsPool;
        private readonly Pool<ICommandParams>   _commandsParamsPool;
        private readonly Dictionary<Type, bool> _commandsPoolableData;
        private          int                    _commandsExecutionIterationTokens;

        public CommandBinder()
        {
            _bindings = new Dictionary<EventBase, IList>();
            _bindingsToUnbind = new List<CommandBindingBase>(8);

            _commandsPool = new Pool<CommandBase>();
            _commandsParamsPool = new Pool<ICommandParams>();
            _commandsPoolableData = new Dictionary<Type, bool>(64);
        }

        /*
         * Binding.
         */

        public CommandBinding Bind(Event @event)
        {
            var binding = new CommandBinding(@event, this, _commandsParamsPool);
            if (_bindings.TryGetValue(@event, out var bindings))
                ((List<CommandBinding>)bindings).Add(binding);
            else
                _bindings[@event] = new List<CommandBinding> { binding };
            return binding;
        }

        public CommandBinding<T1> Bind<T1>(Event<T1> @event)
        {
            var binding = new CommandBinding<T1>(@event, this, _commandsParamsPool);
            if (_bindings.TryGetValue(@event, out var bindings))
                ((List<CommandBinding<T1>>)bindings).Add(binding);
            else
                _bindings[@event] = new List<CommandBinding<T1>> { binding };
            return binding;
        }

        public CommandBinding<T1, T2> Bind<T1, T2>(Event<T1, T2> @event)
        {
            var binding = new CommandBinding<T1, T2>(@event, this, _commandsParamsPool);
            if (_bindings.TryGetValue(@event, out var bindings))
                ((List<CommandBinding<T1, T2>>)bindings).Add(binding);
            else
                _bindings[@event] = new List<CommandBinding<T1, T2>> { binding };
            return binding;
        }

        public CommandBinding<T1, T2, T3> Bind<T1, T2, T3>(Event<T1, T2, T3> @event)
        {
            var binding = new CommandBinding<T1, T2, T3>(@event, this, _commandsParamsPool);
            if (_bindings.TryGetValue(@event, out var bindings))
                ((List<CommandBinding<T1, T2, T3>>)bindings).Add(binding);
            else
                _bindings[@event] = new List<CommandBinding<T1, T2, T3>> { binding };
            return binding;
        }

        /*
         * Unbinding.
         */

        public void Unbind(ICommandBinding binding)
        {
            switch (binding)
            {
                case CommandBindingBase bindingBase:
                    UnbindImpl(bindingBase);
                    break;
                case ICommandBindingComposite bindingComposite:
                    bindingComposite.ForEachBinding(UnbindImpl);
                    break;
                default:
                    throw new CommandBinderException(CommandBinderExceptionType.UnknownBindingType, binding.GetType().FullName);
            }
        }

        public void UnbindAll(EventBase type)
        {
            if (!_bindings.TryGetValue(type, out var bindings))
                return;

            foreach (CommandBindingBase binding in bindings)
                binding.Dispose();

            _bindings.Remove(type);
        }

        public void UnbindAll()
        {
            foreach (var bindings in _bindings.Values)
            {
                foreach (CommandBindingBase binding in bindings)
                    binding.Dispose();
            }

            _bindings.Clear();
        }

        public void UnbindOnQuit()
        {
            foreach (var bindingsList in _bindings.Values)
            foreach (CommandBindingBase binding in bindingsList)
                if (binding.IsUnbindOnQuit)
                    _bindingsToUnbind.Add(binding);
            UnbindScheduled();
        }

        private void UnbindImpl(CommandBindingBase binding)
        {
            if (!_bindings.TryGetValue(binding.Event, out var bindings) || !bindings.Contains(binding))
                return;

            bindings.Remove(binding);
            binding.Dispose();

            if (bindings.Count == 0)
                UnbindAll(binding.Event);
        }

        private void UnbindOrScheduleIfOnce(CommandBindingBase binding)
        {
            if (_commandsExecutionIterationTokens > 0)
                _bindingsToUnbind.Add(binding);
            else
                Unbind(binding);
        }

        private void TryUnbindScheduled()
        {
            if (_commandsExecutionIterationTokens == 0)
                UnbindScheduled();
        }

        private void UnbindScheduled()
        {
            if (_bindingsToUnbind.Count == 0)
                return;
            foreach (var binding in _bindingsToUnbind)
                Unbind(binding);
            _bindingsToUnbind.Clear();
        }

        /*
         * Sequences.
         */

        // TODO: add pooling?
        
        public FlowBinding             Flow()             { return new FlowBinding(this, _commandsParamsPool); }
        public FlowBinding<T1>         Flow<T1>()         { return new FlowBinding<T1>(this, _commandsParamsPool); }
        public FlowBinding<T1, T2>     Flow<T1, T2>()     { return new FlowBinding<T1, T2>(this, _commandsParamsPool); }
        public FlowBinding<T1, T2, T3> Flow<T1, T2, T3>() { return new FlowBinding<T1, T2, T3>(this, _commandsParamsPool); }

        /*
         * Commands.
         */

        // TODO: add pooling?
        
        public SingleCommandBinding             Command<TCommand>() where TCommand : Command                         { throw new NotImplementedException(); }
        public SingleCommandBinding<T1>         Command<TCommand, T1>() where TCommand : Command<T1>                 { throw new NotImplementedException(); }
        public SingleCommandBinding<T1, T2>     Command<TCommand, T1, T2>() where TCommand : Command<T1, T2>         { throw new NotImplementedException(); }
        public SingleCommandBinding<T1, T2, T3> Command<TCommand, T1, T2, T3>() where TCommand : Command<T1, T2, T3> { throw new NotImplementedException(); }

        /*
         * Get.
         */

        public IList<CommandBinding> GetBindings(Event @event)
        {
            _bindings.TryGetValue(@event, out var bindings);
            return (List<CommandBinding>)bindings;
        }

        public IList<CommandBinding<T1>> GetBindings<T1>(Event<T1> @event)
        {
            _bindings.TryGetValue(@event, out var bindings);
            return (List<CommandBinding<T1>>)bindings;
        }

        public IList<CommandBinding<T1, T2>> GetBindings<T1, T2>(Event<T1, T2> @event)
        {
            _bindings.TryGetValue(@event, out var bindings);
            return (List<CommandBinding<T1, T2>>)bindings;
        }

        public IList<CommandBinding<T1, T2, T3>> GetBindings<T1, T2, T3>(Event<T1, T2, T3> @event)
        {
            _bindings.TryGetValue(@event, out var bindings);
            return (List<CommandBinding<T1, T2, T3>>)bindings;
        }

        /*
         * On Command Finish.
         */

        public void OnCommandFinish(CommandBase command)
        {
            if (command.IsExecuted)
                OnCommandFinishedImpl(command);
        }

        private void OnCommandFinishedImpl(CommandBase command)
        {
            var index = command.Index;
            var binding = command.Binding;
            binding.RegisterCommandRelease();

            if (command.IsBreak)
                binding.RegisterBreak();

            ReturnCommand(command);

            if (binding.IsSequence)
            {
                if (binding.IsBreak)
                    FinishBindingExecution(binding, command.Params);
                else
                    ProcessBindingCommand(binding, index + 1, command.Params);

                return;
            }

            if (binding.CheckAllReleased())
            {
                FinishBindingExecution(binding, command.Params);
                return;
            }

            if (!binding.CheckAllExecuted())
            {
                if (binding.IsBreak)
                    FinishBindingExecution(binding, default);
                else
                    ProcessBindingCommand(binding, index + 1, command.Params);
            }
        }

        /*
         * On Command Fail.
         */

        public void OnCommandFail(CommandBase command, Exception exception)
        {
            if (command.IsExecuted)
                OnCommandFailedImpl(command, exception);
        }

        private void OnCommandFailedImpl(CommandBase command, Exception exception)
        {
            var index = command.Index;
            var binding = command.Binding;
            binding.RegisterCommandException(exception);

            ReturnCommand(command);

            if (binding.IsSequence)
            {
                FinishBindingExecution(binding, command.Params);
                return;
            }

            if (binding.CheckAllReleased())
            {
                FinishBindingExecution(binding, command.Params);
                return;
            }

            if (!binding.CheckAllExecuted())
            {
                if (binding.IsBreak)
                    FinishBindingExecution(binding, command.Params);
                else
                    ProcessBindingCommand(binding, index + 1, command.Params);
            }
        }

        /*
         * Event Processing.
         */

        public void ProcessEvent(Event @event)
        {
            var bindings = GetBindings(@event);
            if (bindings == null)
                return;

            _commandsExecutionIterationTokens++;

            foreach (var binding in bindings)
            {
                if (!binding.CheckTriggerCondition())
                {
                    // TODO: check once behavior and schedule for unbinding
                    continue;
                }

                if (binding.IsExecuting)
                    throw new CommandBinderException(CommandBinderExceptionType.BindingAlreadyExecuting, binding.Event.Format());

                binding.StartExecution();
                ProcessBindingCommand(binding, 0, NoParams);
            }

            _commandsExecutionIterationTokens--;
            TryUnbindScheduled();
        }

        public void ProcessEvent<T1>(Event<T1> type, T1 param01)
        {
            var bindings = GetBindings(type);
            if (bindings == null)
                return;

            _commandsExecutionIterationTokens++;

            foreach (var binding in bindings)
            {
                if (!binding.CheckTriggerCondition(param01))
                {
                    // TODO: check once behavior and schedule for unbinding
                    continue;
                }

                if (binding.IsExecuting)
                    throw new CommandBinderException(CommandBinderExceptionType.BindingAlreadyExecuting, binding.Event.Format());

                var param = _commandsParamsPool.Take<CommandParams<T1>>();
                param.Param01 = param01;

                binding.StartExecution();
                ProcessBindingCommand(binding, 0, param);
            }

            _commandsExecutionIterationTokens--;
            TryUnbindScheduled();
        }

        public void ProcessEvent<T1, T2>(Event<T1, T2> type, T1 param01, T2 param02)
        {
            var bindings = GetBindings(type);
            if (bindings == null)
                return;

            _commandsExecutionIterationTokens++;

            foreach (var binding in bindings)
            {
                if (!binding.CheckTriggerCondition(param01, param02))
                {
                    // TODO: check once behavior and schedule for unbinding
                    continue;
                }

                if (binding.IsExecuting)
                    throw new CommandBinderException(CommandBinderExceptionType.BindingAlreadyExecuting, binding.Event.Format());

                var param = _commandsParamsPool.Take<CommandParams<T1, T2>>();
                param.Param01 = param01;
                param.Param02 = param02;

                binding.StartExecution();
                ProcessBindingCommand(binding, 0, param);
            }

            _commandsExecutionIterationTokens--;
            TryUnbindScheduled();
        }

        public void ProcessEvent<T1, T2, T3>(Event<T1, T2, T3> type, T1 param01, T2 param02, T3 param03)
        {
            var bindings = GetBindings(type);
            if (bindings == null)
                return;

            _commandsExecutionIterationTokens++;

            foreach (var binding in bindings)
            {
                if (!binding.CheckTriggerCondition(param01, param02, param03))
                {
                    // TODO: check once behavior and schedule for unbinding
                    continue;
                }

                if (binding.IsExecuting)
                    throw new CommandBinderException(CommandBinderExceptionType.BindingAlreadyExecuting, binding.Event.Format());

                var param = _commandsParamsPool.Take<CommandParams<T1, T2, T3>>();
                param.Param01 = param01;
                param.Param02 = param02;
                param.Param03 = param03;

                binding.StartExecution();
                ProcessBindingCommand(binding, 0, param);
            }

            _commandsExecutionIterationTokens--;
            TryUnbindScheduled();
        }

        /*
         * Sequences.
         */

        public void ProcessFlow(FlowBinding binding)
        {
            if (binding.IsExecuting)
                throw new CommandBinderException(CommandBinderExceptionType.FlowAlreadyExecuting);

            binding.StartExecution();
            ProcessBindingCommand(binding, 0, NoParams);
        }

        public void ProcessFlow<T1>(FlowBinding<T1> binding, T1 param01)
        {
            if (binding.IsExecuting)
                throw new CommandBinderException(CommandBinderExceptionType.FlowAlreadyExecuting);

            var param = _commandsParamsPool.Take<CommandParams<T1>>();
            param.Param01 = param01;

            binding.StartExecution();
            ProcessBindingCommand(binding, 0, param);
        }

        public void ProcessFlow<T1, T2>(FlowBinding<T1, T2> binding, T1 param01, T2 param02)
        {
            if (binding.IsExecuting)
                throw new CommandBinderException(CommandBinderExceptionType.FlowAlreadyExecuting);

            var param = _commandsParamsPool.Take<CommandParams<T1, T2>>();
            param.Param01 = param01;
            param.Param02 = param02;

            binding.StartExecution();
            ProcessBindingCommand(binding, 0, param);
        }

        public void ProcessFlow<T1, T2, T3>(FlowBinding<T1, T2, T3> binding, T1 param01, T2 param02, T3 param03)
        {
            if (binding.IsExecuting)
                throw new CommandBinderException(CommandBinderExceptionType.FlowAlreadyExecuting);

            var param = _commandsParamsPool.Take<CommandParams<T1, T2, T3>>();
            param.Param01 = param01;
            param.Param02 = param02;
            param.Param03 = param03;

            binding.StartExecution();
            ProcessBindingCommand(binding, 0, param);
        }

        /*
         * Breaking.
         */

        public void Break(ICommandBinding binding)
        {
            switch (binding)
            {
                case CommandBindingBase bindingBase:
                {
                    if (bindingBase.IsExecuting)
                        bindingBase.RegisterBreak();
                    break;
                }
                case ICommandBindingComposite bindingComposite:
                    bindingComposite.ForEachBinding(innerBinding =>
                    {
                        if (innerBinding.IsExecuting)
                            innerBinding.RegisterBreak();
                    });
                    break;
                default:
                    throw new CommandBinderException(CommandBinderExceptionType.UnknownBindingType, binding.GetType().FullName);
            }
        }

        public void BreakAll(EventBase @event)
        {
            if (!_bindings.TryGetValue(@event, out var bindings))
                return;

            foreach (CommandBindingBase binding in bindings)
            {
                if (binding.IsExecuting)
                    binding.RegisterBreak();
            }
        }

        /*
         * Command Processing.
         */

        private void ProcessBindingCommand(CommandBindingBase binding, int index, ICommandParams param)
        {
            if (binding.CheckAllReleased())
            {
                FinishBindingExecution(binding, param);
                return;
            }

            if (index >= binding.Commands.Count)
                return;

            var command = GetCommand(binding.Commands[index]);
            command.Setup(index, binding, param);

            try
            {
                if (binding.Params != null && binding.Params.TryGetValue(command.Index, out var commandParam))
                    command.InternalExecute(param, commandParam);
                else
                    command.InternalExecute(param, null);
            }
            catch (Exception exception)
            {
                OnCommandFailedImpl(command, exception);
                return;
            }

            command.PostExecute();
            binding.RegisterCommandExecute();

            if (!command.IsRetained)
            {
                if (command.IsFailed)
                    OnCommandFailedImpl(command, command.Exception);
                else
                    OnCommandFinishedImpl(command);
                return;
            }

            if (!binding.IsSequence)
                ProcessBindingCommand(binding, index + 1, param);
        }

        /*
         * Finishing.
         */

        private void FinishBindingExecution(CommandBindingBase binding, ICommandParams param)
        {
            if (binding.IsBreak)
            {
                binding.FinishExecution();

                if ((binding.OnceBehavior & OnceBehavior.UnbindOnBreak) == OnceBehavior.UnbindOnBreak)
                    UnbindOrScheduleIfOnce(binding);

                if (binding.BreakEvent != null)
                    param.DispatchParams(Dispatcher, binding.BreakEvent);
            }
            else if (binding.HasFails)
            {
                _commandsParamsPool.Return(param);

                HandleBindingFail(binding);
                return;
            }
            else
            {
                binding.FinishExecution();

                if ((binding.OnceBehavior & OnceBehavior.UnbindOnComplete) == OnceBehavior.UnbindOnComplete)
                    UnbindOrScheduleIfOnce(binding);

                if (binding.CompleteEvent != null)
                    param.DispatchParams(Dispatcher, binding.CompleteEvent);
            }

            _commandsParamsPool.Return(param);
        }

        private void HandleBindingFail(CommandBindingBase binding)
        {
            var exception = binding.CommandsFailed[0];

            binding.FinishExecution();

            if ((binding.OnceBehavior & OnceBehavior.UnbindOnFail) == OnceBehavior.UnbindOnFail)
                UnbindOrScheduleIfOnce(binding);

            switch (binding.FailEvent)
            {
                case Event<Exception> event1:
                    Dispatcher.Dispatch(event1, exception);
                    break;
                case Event event0:
                    Dispatcher.Dispatch(event0);
                    break;
                case null:
                    ExceptionDispatchInfo.Capture(exception).Throw();
                    break;
                default:
                    throw new CommandBinderException(CommandBinderExceptionType.IncompatibleEventType);
            }
        }

        /*
         * Command Generic.
         */

        private CommandBase GetCommand(Type commandType)
        {
            CommandBase command;
            bool isNewInstance;

            if (CheckCommandIsPoolable(commandType))
            {
                command = _commandsPool.Take(commandType, out isNewInstance);
            }
            else
            {
                command = _commandsPool.Instantiate(commandType);
                isNewInstance = true;
            }

            if (isNewInstance)
            {
                command.SetCommandBinder(this);
                InjectionBinder.Construct(command, true);
            }

            return command;
        }

        private void ReturnCommand(CommandBase command)
        {
            if (command.IsClean)
                return;

            command.Reset();

            if (CheckCommandIsPoolable(command.GetType()))
                _commandsPool.Return(command);
            else
                InjectionBinder.Destroy(command, true);
        }

        private bool CheckCommandIsPoolable(Type commandType)
        {
            if (!_commandsPoolableData.TryGetValue(commandType, out var poolable))
            {
                poolable = Attribute.IsDefined(commandType, typeof(PoolableAttribute));
                _commandsPoolableData.Add(commandType, poolable);
            }

            return poolable;
        }
    }
}