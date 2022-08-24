using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using Build1.PostMVC.Extensions.MVCS.Events;
using Build1.PostMVC.Extensions.MVCS.Events.Impl;
using Build1.PostMVC.Extensions.MVCS.Injection;
using Build1.PostMVC.Extensions.Unity.Modules.Logging;
using Build1.PostMVC.Utils.Pooling;
using Event = Build1.PostMVC.Extensions.MVCS.Events.Event;

namespace Build1.PostMVC.Extensions.MVCS.Commands.Impl
{
    public sealed class CommandBinder : ICommandBinder
    {
        [Log(LogLevel.Warning)] public ILog             Log             { get; set; }
        [Inject]                public IEventDispatcher Dispatcher      { get; set; }
        [Inject]                public IInjectionBinder InjectionBinder { get; set; }
        
        private readonly CommandParams NoParams = new();

        private readonly Dictionary<EventBase, IList> _bindings;
        private readonly List<CommandBindingBase>     _bindingsToUnbind;

        private readonly Pool<CommandBase>       _commandsPool;
        private readonly Pool<CommandParamsBase> _commandsParamsPool;
        private readonly Dictionary<Type, bool>  _commandsPoolableData;
        private          int                     _commandsExecutionIterationTokens;

        public CommandBinder()
        {
            _bindings = new Dictionary<EventBase, IList>();
            _bindingsToUnbind = new List<CommandBindingBase>(8);

            _commandsPool = new Pool<CommandBase>();
            _commandsParamsPool = new Pool<CommandParamsBase>();
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

        public void Unbind(CommandBindingBase binding)
        {
            if (!_bindings.TryGetValue(binding.Event, out var bindings) || !bindings.Contains(binding))
                return;

            bindings.Remove(binding);
            binding.Dispose();

            if (bindings.Count == 0)
                UnbindAll(binding.Event);
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
                if (binding.IsExecuting)
                    throw new CommandBinderException(CommandBinderExceptionType.BindingAlreadyExecuting);

                if (binding.TriggerPredicate != null && !binding.TriggerPredicate.Invoke())
                    continue;

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
                if (binding.IsExecuting)
                    throw new CommandBinderException(CommandBinderExceptionType.BindingAlreadyExecuting);

                if (binding.TriggerValuesSet && !EqualityComparer<T1>.Default.Equals(binding.TriggerValue01, param01))
                    continue;

                if (binding.TriggerPredicate != null && !binding.TriggerPredicate.Invoke(param01))
                    continue;

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
                if (binding.IsExecuting)
                    throw new CommandBinderException(CommandBinderExceptionType.BindingAlreadyExecuting);

                if (binding.TriggerValuesSet &&
                    (!EqualityComparer<T1>.Default.Equals(binding.TriggerValue01, param01) ||
                     !EqualityComparer<T2>.Default.Equals(binding.TriggerValue02, param02)))
                    continue;

                if (binding.TriggerPredicate != null && !binding.TriggerPredicate.Invoke(param01, param02))
                    continue;

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
                if (binding.IsExecuting)
                    throw new CommandBinderException(CommandBinderExceptionType.BindingAlreadyExecuting);

                if (binding.TriggerValuesSet &&
                    (!EqualityComparer<T1>.Default.Equals(binding.TriggerValue01, param01) ||
                     !EqualityComparer<T2>.Default.Equals(binding.TriggerValue02, param02) ||
                     !EqualityComparer<T3>.Default.Equals(binding.TriggerValue03, param03)))
                    continue;

                if (binding.TriggerPredicate != null && !binding.TriggerPredicate.Invoke(param01, param02, param03))
                    continue;

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
         * Breaking.
         */

        public void Break(CommandBindingBase binding)
        {
            if (binding.IsExecuting)
                binding.RegisterBreak();
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

        private void ProcessBindingCommand(CommandBindingBase binding, int index, CommandParamsBase param)
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

        private void FinishBindingExecution(CommandBindingBase binding, CommandParamsBase param)
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

            Log?.Error(log =>
            {
                try
                {
                    ExceptionDispatchInfo.Capture(exception).Throw();
                }
                catch (Exception exceptionRethrown)
                {
                    log.Error(exceptionRethrown);
                }
            });

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