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
        private readonly Dictionary<ICommandBase, CommandBindingBase> _activeCommands;
        private readonly Dictionary<ICommandBase, CommandBindingBase> _activeSequences;

        public CommandBinder()
        {
            _bindings = new Dictionary<EventBase, List<CommandBindingBase>>();
            _bindingsToUnbind = new List<CommandBindingBase>(8);

            _commandPool = new CommandPool();
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
            UnbindImpl();
        }

        private void UnbindIfOnce(CommandBindingBase binding)
        {
            if (binding.IsOnce)
                Unbind(binding);
        }

        private void UnbindLaterIfOnce(CommandBindingBase binding)
        {
            if (binding.IsOnce)
                _bindingsToUnbind.Add(binding);
        }

        private void UnbindImpl()
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
            var sequenceId = command.SequenceId + 1;
            
            if (!ReleaseCommandImpl(command, out var binding) || NextCommand(binding, sequenceId)) 
                return;
            
            UnbindIfOnce(binding);
            ProcessOnCompleteEvent((CommandBinding)binding);
        }

        public void ReleaseCommand<T1>(ICommand<T1> command)
        {
            var sequenceId = command.SequenceId + 1;
            var param01 = command.Param01;
            
            if (!ReleaseCommandImpl(command, out var binding) || NextCommand(binding, sequenceId, param01)) 
                return;
            
            UnbindIfOnce(binding);
            ProcessOnCompleteEvent((CommandBinding<T1>)binding, param01);
        }
        
        public void ReleaseCommand<T1, T2>(ICommand<T1, T2> command)
        {
            var sequenceId = command.SequenceId + 1;
            var param01 = command.Param01;
            var param02 = command.Param02;
            
            if (!ReleaseCommandImpl(command, out var binding) || NextCommand(binding, sequenceId, param01, param02)) 
                return;
            
            UnbindIfOnce(binding);
            ProcessOnCompleteEvent((CommandBinding<T1, T2>)binding, param01, param02);
        }

        public void ReleaseCommand<T1, T2, T3>(ICommand<T1, T2, T3> command)
        {
            var sequenceId = command.SequenceId + 1;
            var param01 = command.Param01;
            var param02 = command.Param02;
            var param03 = command.Param03;
            
            if (!ReleaseCommandImpl(command, out var binding) || NextCommand(binding, sequenceId, param01, param02, param03)) 
                return;
            
            UnbindIfOnce(binding);
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
        
        private bool ReleaseCommandImpl(ICommandBase command, out CommandBindingBase binding)
        {
            if (command.IsRetained)
            {
                binding = null;
                return false;
            }
            
            _commandPool.ReturnCommand(command);
            
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
            if (FailCommandImpl(command, out var binding))
                ProcessOnFailEvent(binding, exception);
        }

        public void FailCommand<T1>(ICommand<T1> command, Exception exception)
        {
            if (FailCommandImpl(command, out var binding))
                ProcessOnFailEvent(binding, exception);
        }

        public void FailCommand<T1, T2>(ICommand<T1, T2> command, Exception exception)
        {
            if (FailCommandImpl(command, out var binding))
                ProcessOnFailEvent(binding, exception);
        }

        public void FailCommand<T1, T2, T3>(ICommand<T1, T2, T3> command, Exception exception)
        {
            if (FailCommandImpl(command, out var binding))
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

        private bool FailCommandImpl(ICommandBase command, out CommandBindingBase binding)
        {
            if (command.IsRetained)
            {
                binding = null;
                return false;
            }
            
            _commandPool.ReturnCommand(command);

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

            foreach (var binding in bindings)
            {
                if (binding.IsSequence)
                {
                    if (!NextCommand(binding, 0))
                        UnbindLaterIfOnce(binding);
                    return;
                }

                var length = binding.Commands.Count + 1;
                for (var i = 0; i < length; i++)
                {
                    if (!NextCommand(binding, i))
                        UnbindLaterIfOnce(binding);
                }
            }

            UnbindImpl();
        }

        public void ProcessEvent<T1>(Event<T1> type, T1 param01)
        {
            var bindings = GetBindings(type);
            if (bindings == null)
                return;

            foreach (var binding in bindings)
            {
                if (binding.IsSequence)
                {
                    if (!NextCommand(binding, 0, param01))
                        UnbindLaterIfOnce(binding);
                    return;
                }

                var length = binding.Commands.Count + 1;
                for (var i = 0; i < length; i++)
                {
                    if (!NextCommand(binding, i, param01))
                        UnbindLaterIfOnce(binding);
                }
            }

            UnbindImpl();
        }

        public void ProcessEvent<T1, T2>(Event<T1, T2> type, T1 param01, T2 param02)
        {
            var bindings = GetBindings(type);
            if (bindings == null)
                return;

            foreach (var binding in bindings)
            {
                if (binding.IsSequence)
                {
                    if (!NextCommand(binding, 0, param01, param02))
                        UnbindLaterIfOnce(binding);
                    return;
                }

                var length = binding.Commands.Count + 1;
                for (var i = 0; i < length; i++)
                {
                    if (!NextCommand(binding, i, param01, param02))
                        UnbindLaterIfOnce(binding);
                }
            }

            UnbindImpl();
        }

        public void ProcessEvent<T1, T2, T3>(Event<T1, T2, T3> type, T1 param01, T2 param02, T3 param03)
        {
            var bindings = GetBindings(type);
            if (bindings == null)
                return;

            foreach (var binding in bindings)
            {
                if (binding.IsSequence)
                {
                    if (!NextCommand(binding, 0, param01, param02, param03))
                        UnbindLaterIfOnce(binding);
                    return;
                }

                var length = binding.Commands.Count + 1;
                for (var i = 0; i < length; i++)
                {
                    if (!NextCommand(binding, i, param01, param02, param03))
                        UnbindLaterIfOnce(binding);
                }
            }

            UnbindImpl();
        }

        /*
         * Next Command.
         */

        private bool NextCommand(CommandBindingBase binding, int depth)
        {
            var commands = binding.Commands;
            if (depth >= commands.Count)
                return false;
            var command = InvokeCommand(commands[depth], binding, depth);
            if (!command.IsRetained && !command.IsFailed)
                ReleaseCommand(command);
            return true;
        }

        private bool NextCommand<T1>(CommandBindingBase binding, int depth, T1 param01)
        {
            var commands = binding.Commands;
            if (depth >= commands.Count)
                return false;
            var command = InvokeCommand(commands[depth], binding, depth, param01);
            if (!command.IsRetained && !command.IsFailed)
                ReleaseCommand(command);
            return true;
        }

        private bool NextCommand<T1, T2>(CommandBindingBase binding, int depth, T1 param01, T2 param02)
        {
            var commands = binding.Commands;
            if (depth >= commands.Count)
                return false;
            var command = InvokeCommand(commands[depth], binding, depth, param01, param02);
            if (!command.IsRetained && !command.IsFailed)
                ReleaseCommand(command);
            return true;
        }

        private bool NextCommand<T1, T2, T3>(CommandBindingBase binding, int depth, T1 param01, T2 param02, T3 param03)
        {
            var commands = binding.Commands;
            if (depth >= commands.Count)
                return false;
            var command = InvokeCommand(commands[depth], binding, depth, param01, param02, param03);
            if (!command.IsRetained && !command.IsFailed)
                ReleaseCommand(command);
            return true;
        }

        /*
         * Invocation.
         */

        private ICommand InvokeCommand(Type commandType, CommandBindingBase binding, int sequenceId)
        {
            var command = (ICommand)GetCommand(commandType);
            TrackCommand(command, binding);
            command.Setup(sequenceId);
            command.Execute();
            return command;
        }

        private ICommand<T1> InvokeCommand<T1>(Type commandType, CommandBindingBase binding, int sequenceId, T1 param01)
        {
            var command = (ICommand<T1>)GetCommand(commandType);
            TrackCommand(command, binding);
            command.Setup(sequenceId, param01);
            command.Execute(param01);
            return command;
        }

        private ICommand<T1, T2> InvokeCommand<T1, T2>(Type commandType, CommandBindingBase binding, int sequenceId, T1 param01, T2 param02)
        {
            var command = (ICommand<T1, T2>)GetCommand(commandType);
            TrackCommand(command, binding);
            command.Setup(sequenceId, param01, param02);
            command.Execute(param01, param02);
            return command;
        }

        private ICommand<T1, T2, T3> InvokeCommand<T1, T2, T3>(Type commandType, CommandBindingBase binding, int sequenceId, T1 param01, T2 param02, T3 param03)
        {
            var command = (ICommand<T1, T2, T3>)GetCommand(commandType);
            TrackCommand(command, binding);
            command.Setup(sequenceId, param01, param02, param03);
            command.Execute(param01, param02, param03);
            return command;
        }

        /*
         * Command Generic.
         */

        private ICommandBase GetCommand(Type commandType)
        {
            var command = _commandPool.TakeCommand(commandType, out var isNewInstance);
            if (isNewInstance)
                command.SetCommandBinder(this);
            if (isNewInstance || command.IsClean)
                InjectionBinder.Construct(command, true);
            return command;
        }

        private void TrackCommand(ICommandBase command, CommandBindingBase binding)
        {
            if (binding.IsSequence)
                _activeSequences.Add(command, binding);
            else
                _activeCommands.Add(command, binding);
        }
    }
}