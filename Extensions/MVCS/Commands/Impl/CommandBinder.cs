using System;
using System.Collections.Generic;
using Build1.PostMVC.Extensions.MVCS.Commands.Api;
using Build1.PostMVC.Extensions.MVCS.Events;
using Build1.PostMVC.Extensions.MVCS.Events.Impl;
using Build1.PostMVC.Extensions.MVCS.Injection;
using Event = Build1.PostMVC.Extensions.MVCS.Events.Event;

namespace Build1.PostMVC.Extensions.MVCS.Commands.Impl
{
    public sealed class CommandBinder : ICommandBinder
    {
        [Inject] public IEventDispatcher Dispatcher      { get; set; }
        [Inject] public IInjectionBinder InjectionBinder { get; set; }

        private readonly Dictionary<EventBase, List<CommandBindingBase>> _bindings;
        private readonly List<CommandBindingBase>                        _bindingsToUnbind;

        private readonly ICommandPool                                 _commandPool;
        private readonly Dictionary<Type, bool>                       _commandsPoolableData;
        private          int                                          _commandsExecutionIterationTokens;
        private readonly Dictionary<ICommandBase, CommandBindingBase> _activeCommands;
        private readonly Dictionary<ICommandBase, CommandBindingBase> _activeSequences;

        public CommandBinder()
        {
            _bindings = new Dictionary<EventBase, List<CommandBindingBase>>();
            _bindingsToUnbind = new List<CommandBindingBase>(8);

            _commandPool = new CommandPool();
            _commandsPoolableData = new Dictionary<Type, bool>(64);
            _activeCommands = new Dictionary<ICommandBase, CommandBindingBase>(16);
            _activeSequences = new Dictionary<ICommandBase, CommandBindingBase>(8);
        }

        /*
         * Binding.
         */

        public CommandBinding Bind(Event type)
        {
            var binding = new CommandBinding(type);
            AddBinding(type, binding);
            return binding;
        }

        public CommandBinding<T1> Bind<T1>(Event<T1> type)
        {
            var binding = new CommandBinding<T1>(type);
            AddBinding(type, binding);
            return binding;
        }

        public CommandBinding<T1, T2> Bind<T1, T2>(Event<T1, T2> type)
        {
            var binding = new CommandBinding<T1, T2>(type);
            AddBinding(type, binding);
            return binding;
        }

        public CommandBinding<T1, T2, T3> Bind<T1, T2, T3>(Event<T1, T2, T3> type)
        {
            var binding = new CommandBinding<T1, T2, T3>(type);
            AddBinding(type, binding);
            return binding;
        }

        private void AddBinding(EventBase key, CommandBindingBase binding)
        {
            if (!_bindings.ContainsKey(key))
                _bindings[key] = new List<CommandBindingBase> { binding };
            else
                _bindings[key].Add(binding);
        }

        /*
         * Unbinding.
         */

        public void Unbind(CommandBindingBase binding)
        {
            if (!_bindings.TryGetValue(binding.Event, out var bindings))
                return;
            if (bindings.Remove(binding) && bindings.Count == 0)
                UnbindAll(binding.Event);
        }

        public void UnbindAll(EventBase type) { _bindings.Remove(type); }
        public void UnbindAll()               { _bindings.Clear(); }

        public void UnbindOnQuit()
        {
            foreach (var bindingsList in _bindings.Values)
            foreach (var binding in bindingsList)
                if (binding.IsUnbindOnQuit)
                    _bindingsToUnbind.Add(binding);
            UnbindScheduled();
        }

        private void UnbindOrScheduleIfOnce(CommandBindingBase binding)
        {
            if (!binding.IsOnce)
                return;
            
            if (_commandsExecutionIterationTokens > 0)
                _bindingsToUnbind.Add(binding);
            else
                Unbind(binding);
        }
        
        private void ScheduleUnbindIfOnce(CommandBindingBase binding)
        {
            if (binding.IsOnce)
                _bindingsToUnbind.Add(binding);
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

        public IList<CommandBindingBase> GetBindings(EventBase type)
        {
            _bindings.TryGetValue(type, out var bindings);
            return bindings;
        }

        /*
         * Releasing.
         */

        public void ReleaseCommand(ICommand command)
        {
            var index = command.Index + 1;

            if (!TryReleaseCommand(command, out var binding) || TryExecuteBindingCommand(binding, index))
                return;

            UnbindOrScheduleIfOnce(binding);
            ProcessOnCompleteEvent((CommandBinding)binding);
        }

        public void ReleaseCommand<T1>(ICommand<T1> command)
        {
            var index = command.Index + 1;
            var param01 = command.Param01;

            if (!TryReleaseCommand(command, out var binding) || TryExecuteBindingCommand(binding, index, param01))
                return;

            UnbindOrScheduleIfOnce(binding);
            ProcessOnCompleteEvent((CommandBinding<T1>)binding, param01);
        }

        public void ReleaseCommand<T1, T2>(ICommand<T1, T2> command)
        {
            var index = command.Index + 1;
            var param01 = command.Param01;
            var param02 = command.Param02;

            if (!TryReleaseCommand(command, out var binding) || TryExecuteBindingCommand(binding, index, param01, param02))
                return;

            UnbindOrScheduleIfOnce(binding);
            ProcessOnCompleteEvent((CommandBinding<T1, T2>)binding, param01, param02);
        }

        public void ReleaseCommand<T1, T2, T3>(ICommand<T1, T2, T3> command)
        {
            var index = command.Index + 1;
            var param01 = command.Param01;
            var param02 = command.Param02;
            var param03 = command.Param03;

            if (!TryReleaseCommand(command, out var binding) || TryExecuteBindingCommand(binding, index, param01, param02, param03))
                return;

            UnbindOrScheduleIfOnce(binding);
            ProcessOnCompleteEvent((CommandBinding<T1, T2, T3>)binding, param01, param02, param03);
        }

        private void ProcessOnCompleteEvent(CommandBinding binding)
        {
            var @event = binding.CompleteEvent;
            if (@event != null)
                Dispatcher.Dispatch(@event);
        }

        private void ProcessOnCompleteEvent<T1>(CommandBinding<T1> binding, T1 param01)
        {
            var @event = binding.CompleteEvent;
            if (@event != null)
                Dispatcher.Dispatch(@event, param01);
        }

        private void ProcessOnCompleteEvent<T1, T2>(CommandBinding<T1, T2> binding, T1 param01, T2 param02)
        {
            var @event = binding.CompleteEvent;
            if (@event != null)
                Dispatcher.Dispatch(@event, param01, param02);
        }

        private void ProcessOnCompleteEvent<T1, T2, T3>(CommandBinding<T1, T2, T3> binding, T1 param01, T2 param02, T3 param03)
        {
            var @event = binding.CompleteEvent;
            if (@event != null)
                Dispatcher.Dispatch(@event, param01, param02, param03);
        }

        private bool TryReleaseCommand(ICommandBase command, out CommandBindingBase binding)
        {
            if (command.IsRetained)
            {
                binding = null;
                return false;
            }

            ReturnCommand(command);

            if (_activeCommands.Remove(command))
            {
                binding = null;
                return false;
            }

            if (!_activeSequences.TryGetValue(command, out binding))
                return false;

            _activeSequences.Remove(command);
            return true;
        }

        /*
         * Stopping.
         */

        public void FailCommand(ICommand command, Exception exception)
        {
            if (TryReleaseFailedCommand(command, out var binding))
                ProcessOnFailEvent(binding, exception);
        }

        public void FailCommand<T1>(ICommand<T1> command, Exception exception)
        {
            if (TryReleaseFailedCommand(command, out var binding))
                ProcessOnFailEvent(binding, exception);
        }

        public void FailCommand<T1, T2>(ICommand<T1, T2> command, Exception exception)
        {
            if (TryReleaseFailedCommand(command, out var binding))
                ProcessOnFailEvent(binding, exception);
        }

        public void FailCommand<T1, T2, T3>(ICommand<T1, T2, T3> command, Exception exception)
        {
            if (TryReleaseFailedCommand(command, out var binding))
                ProcessOnFailEvent(binding, exception);
        }

        private void ProcessOnFailEvent(CommandBindingBase binding, Exception exception)
        {
            var @event = binding.FailEvent;
            if (@event != null)
                Dispatcher.Dispatch(@event, exception);
        }

        /*
         * Releasing Implementation.
         */

        private bool TryReleaseFailedCommand(ICommandBase command, out CommandBindingBase binding)
        {
            if (command.IsRetained)
            {
                binding = null;
                return false;
            }

            ReturnCommand(command);

            if (_activeCommands.Remove(command))
            {
                binding = null;
                return false;
            }

            if (!_activeSequences.TryGetValue(command, out binding))
                return false;

            _activeSequences.Remove(command);
            return true;
        }

        /*
         * Event Processing.
         */

        public void ProcessEvent(Event type)
        {
            var bindings = GetBindings(type);
            if (bindings == null)
                return;

            // TODO: if there will be an exception then we're screwed
            
            _commandsExecutionIterationTokens++;
            
            foreach (var binding in bindings)
            {
                if (binding.IsSequence)
                {
                    if (!TryExecuteBindingCommand(binding, 0))
                        ScheduleUnbindIfOnce(binding);
                    continue;
                }
                
                var length = binding.Commands.Count + 1;
                for (var i = 0; i < length; i++)
                {
                    if (!TryExecuteBindingCommand(binding, i))
                        ScheduleUnbindIfOnce(binding);
                }
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
                if (binding.IsSequence)
                {
                    if (!TryExecuteBindingCommand(binding, 0, param01))
                        ScheduleUnbindIfOnce(binding);
                    continue;
                }
                
                var length = binding.Commands.Count + 1;
                for (var i = 0; i < length; i++)
                {
                    if (!TryExecuteBindingCommand(binding, i, param01))
                        ScheduleUnbindIfOnce(binding);
                }
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
                if (binding.IsSequence)
                {
                    if (!TryExecuteBindingCommand(binding, 0, param01, param02))
                        ScheduleUnbindIfOnce(binding);
                    continue;
                }
                
                var length = binding.Commands.Count + 1;
                for (var i = 0; i < length; i++)
                {
                    if (!TryExecuteBindingCommand(binding, i, param01, param02))
                        ScheduleUnbindIfOnce(binding);
                }
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
                if (binding.IsSequence)
                {
                    if (!TryExecuteBindingCommand(binding, 0, param01, param02, param03))
                        ScheduleUnbindIfOnce(binding);
                    continue;
                }
                
                var length = binding.Commands.Count + 1;
                for (var i = 0; i < length; i++)
                {
                    if (!TryExecuteBindingCommand(binding, i, param01, param02, param03))
                        ScheduleUnbindIfOnce(binding);
                }
            }
            
            _commandsExecutionIterationTokens--;

            TryUnbindScheduled();
        }

        /*
         * Next Command.
         */

        private bool TryExecuteBindingCommand(CommandBindingBase binding, int index)
        {
            if (index >= binding.Commands.Count)
                return false;
            
            var command = (ICommand)GetCommand(binding.Commands[index], binding);
            command.Setup(index);
            command.Execute();
            
            if (!command.IsRetained && !command.IsFailed)
                ReleaseCommand(command);
            
            return true;
        }

        private bool TryExecuteBindingCommand<T1>(CommandBindingBase binding, int index, T1 param01)
        {
            var commands = binding.Commands;
            if (index >= commands.Count)
                return false;
            
            var command = (ICommand<T1>)GetCommand(commands[index], binding);
            command.Setup(index, param01);
            command.Execute(param01);

            if (!command.IsRetained && !command.IsFailed)
                ReleaseCommand(command);
            
            return true;
        }

        private bool TryExecuteBindingCommand<T1, T2>(CommandBindingBase binding, int index, T1 param01, T2 param02)
        {
            var commands = binding.Commands;
            if (index >= commands.Count)
                return false;
            
            var command = (ICommand<T1, T2>)GetCommand(commands[index], binding);
            command.Setup(index, param01, param02);
            command.Execute(param01, param02);
            
            if (!command.IsRetained && !command.IsFailed)
                ReleaseCommand(command);
            
            return true;
        }

        private bool TryExecuteBindingCommand<T1, T2, T3>(CommandBindingBase binding, int index, T1 param01, T2 param02, T3 param03)
        {
            var commands = binding.Commands;
            if (index >= commands.Count)
                return false;
            
            var command = (ICommand<T1, T2, T3>)GetCommand(commands[index], binding);
            command.Setup(index, param01, param02, param03);
            command.Execute(param01, param02, param03);
            
            if (!command.IsRetained && !command.IsFailed)
                ReleaseCommand(command);
            
            return true;
        }

        /*
         * Command Generic.
         */

        private ICommandBase GetCommand(Type commandType, CommandBindingBase binding)
        {
            ICommandBase command;
            bool isNewInstance;

            if (CheckCommandIsPoolable(commandType))
            {
                command = _commandPool.TakeCommand(commandType, out isNewInstance);
            }
            else
            {
                command = _commandPool.InstantiateCommand(commandType);
                isNewInstance = true;
            }

            if (isNewInstance)
            {
                command.SetCommandBinder(this);
                InjectionBinder.Construct(command, true);
            }

            if (binding.IsSequence)
                _activeSequences.Add(command, binding);
            else
                _activeCommands.Add(command, binding);
            
            return command;
        }

        private void ReturnCommand(ICommandBase command)
        {
            if (command.IsClean)
                return;
            
            command.Reset();
            
            if (CheckCommandIsPoolable(command.GetType()))
                _commandPool.ReturnCommand(command);
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