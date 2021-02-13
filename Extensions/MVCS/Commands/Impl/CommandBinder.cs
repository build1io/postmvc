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
        [Inject] public IInjectionBinder InjectionBinder { get; set; }

        private readonly Dictionary<EventBase, List<CommandBindingBase>> _bindings;
        private readonly List<CommandBindingBase>                        _bindingsToUnbind;

        private readonly ICommandPool                                 _commandPool;
        private readonly List<ICommandBase>                           _activeCommands;
        private readonly Dictionary<ICommandBase, CommandBindingBase> _activeSequences;

        public CommandBinder()
        {
            _bindings = new Dictionary<EventBase, List<CommandBindingBase>>();
            _bindingsToUnbind = new List<CommandBindingBase>(8);

            _commandPool = new CommandPool();
            _activeCommands = new List<ICommandBase>(16);
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

        public void UnbindAll(EventBase type)
        {
            _bindings.Remove(type);
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
            if (ReleaseCommandImpl(command, out var binding) && !NextCommand(binding, command.SequenceId + 1))
                UnbindIfOnce(binding);
        }

        public void ReleaseCommand<T1>(ICommand<T1> command)
        {
            if (ReleaseCommandImpl(command, out var binding) && !NextCommand(binding, command.SequenceId + 1, command.Param01))
                UnbindIfOnce(binding);
        }

        public void ReleaseCommand<T1, T2>(ICommand<T1, T2> command)
        {
            if (ReleaseCommandImpl(command, out var binding) && !NextCommand(binding, command.SequenceId + 1, command.Param01, command.Param02))
                UnbindIfOnce(binding);
        }

        public void ReleaseCommand<T1, T2, T3>(ICommand<T1, T2, T3> command)
        {
            if (ReleaseCommandImpl(command, out var binding) && !NextCommand(binding, command.SequenceId + 1, command.Param01, command.Param02, command.Param03))
                UnbindIfOnce(binding);
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

        public void StopCommand(ICommandBase command)
        {
            _activeSequences.Remove(command);
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
            ReleaseCommand(command);
            return true;
        }

        private bool NextCommand<T1>(CommandBindingBase binding, int depth, T1 param01)
        {
            var commands = binding.Commands;
            if (depth >= commands.Count)
                return false;
            var command = InvokeCommand(commands[depth], binding, depth, param01);
            ReleaseCommand(command);
            return true;
        }

        private bool NextCommand<T1, T2>(CommandBindingBase binding, int depth, T1 param01, T2 param02)
        {
            var commands = binding.Commands;
            if (depth >= commands.Count)
                return false;
            var command = InvokeCommand(commands[depth], binding, depth, param01, param02);
            ReleaseCommand(command);
            return true;
        }

        private bool NextCommand<T1, T2, T3>(CommandBindingBase binding, int depth, T1 param01, T2 param02, T3 param03)
        {
            var commands = binding.Commands;
            if (depth >= commands.Count)
                return false;
            var command = InvokeCommand(commands[depth], binding, depth, param01, param02, param03);
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
            command.SetSequenceId(sequenceId);
            command.Execute();
            return command;
        }

        private ICommand<T1> InvokeCommand<T1>(Type commandType, CommandBindingBase binding, int sequenceId, T1 param01)
        {
            var command = (ICommand<T1>)GetCommand(commandType);
            TrackCommand(command, binding);
            command.SetSequenceId(sequenceId);
            command.SetData(param01);
            command.Execute(param01);
            return command;
        }

        private ICommand<T1, T2> InvokeCommand<T1, T2>(Type commandType, CommandBindingBase binding, int sequenceId, T1 param01, T2 param02)
        {
            var command = (ICommand<T1, T2>)GetCommand(commandType);
            TrackCommand(command, binding);
            command.SetSequenceId(sequenceId);
            command.SetData(param01, param02);
            command.Execute(param01, param02);
            return command;
        }

        private ICommand<T1, T2, T3> InvokeCommand<T1, T2, T3>(Type commandType, CommandBindingBase binding, int sequenceId, T1 param01, T2 param02, T3 param03)
        {
            var command = (ICommand<T1, T2, T3>)GetCommand(commandType);
            TrackCommand(command, binding);
            command.SetSequenceId(sequenceId);
            command.SetData(param01, param02, param03);
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
                _activeCommands.Add(command);
        }
    }
}