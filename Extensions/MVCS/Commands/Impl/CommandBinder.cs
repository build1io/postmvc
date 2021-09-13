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

        public CommandBinder()
        {
            _bindings = new Dictionary<EventBase, List<CommandBindingBase>>();
            _bindingsToUnbind = new List<CommandBindingBase>(8);

            _commandPool = new CommandPool();
            _commandsPoolableData = new Dictionary<Type, bool>(64);
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
         * On Command Finished.
         */
        
        public void OnCommandFinished(ICommand command)
        {
            if (command.IsExecuted)
                OnCommandFinishedImpl(command);
        }
        public void OnCommandFinished<T1>(ICommand<T1> command)
        {
            if (command.IsExecuted)
                OnCommandFinishedImpl(command);
        }
        public void OnCommandFinished<T1, T2>(ICommand<T1, T2> command)
        {
            if (command.IsExecuted)
                OnCommandFinishedImpl(command);
        }
        public void OnCommandFinished<T1, T2, T3>(ICommand<T1, T2, T3> command)
        {
            if (command.IsExecuted)
                OnCommandFinishedImpl(command);
        }
        
        private void OnCommandFinishedImpl(ICommand command)
        {
            var index = command.Index;
            var binding = command.Binding;
            
            binding.RegisterCommandRelease();
            
            ReturnCommand(command);

            if (binding.IsSequence)
            {
                ProcessBindingCommand(binding, index + 1);
                return;
            }
            
            if (binding.CheckAllReleased())
            {
                FinishBindingExecution(binding);
                return;
            }
            
            ProcessBindingCommand(binding, index + 1);
        }

        private void OnCommandFinishedImpl<T1>(ICommand<T1> command)
        {
            var index = command.Index;
            var param01 = command.Param01;
            var binding = command.Binding;
            
            binding.RegisterCommandRelease();
            
            ReturnCommand(command);

            if (binding.IsSequence)
            {
                ProcessBindingCommand(binding, index + 1, param01);
                return;
            }
            
            if (binding.CheckAllReleased())
            {
                FinishBindingExecution(binding, param01);
                return;
            }
            
            ProcessBindingCommand(binding, index + 1, param01);
        }
        
        private void OnCommandFinishedImpl<T1, T2>(ICommand<T1, T2> command)
        {
            var index = command.Index;
            var param01 = command.Param01;
            var param02 = command.Param02;
            var binding = command.Binding;
            
            binding.RegisterCommandRelease();
            
            ReturnCommand(command);

            if (binding.IsSequence)
            {
                ProcessBindingCommand(binding, index + 1, param01, param02);
                return;
            }
            
            if (binding.CheckAllReleased())
            {
                FinishBindingExecution(binding, param01, param02);
                return;
            }
            
            ProcessBindingCommand(binding, index + 1, param01, param02);
        }
        
        private void OnCommandFinishedImpl<T1, T2, T3>(ICommand<T1, T2, T3> command)
        {
            var index = command.Index;
            var param01 = command.Param01;
            var param02 = command.Param02;
            var param03 = command.Param03;
            var binding = command.Binding;
            
            binding.RegisterCommandRelease();
            
            ReturnCommand(command);

            if (binding.IsSequence)
            {
                ProcessBindingCommand(binding, index + 1, param01, param02, param03);
                return;
            }
            
            if (binding.CheckAllReleased())
            {
                FinishBindingExecution(binding, param01, param02, param03);
                return;
            }
            
            ProcessBindingCommand(binding, index + 1, param01, param02, param03);
        }
        
        /*
         * On Command Failed.
         */
        
        public void OnCommandFailed(ICommand command, Exception exception)
        {
            if (command.IsExecuted)
                OnCommandFailedImpl(command, exception);
        }
        public void OnCommandFailed<T1>(ICommand<T1> command, Exception exception)
        {
            if (command.IsExecuted)
                OnCommandFailedImpl(command, exception);
        }
        public void OnCommandFailed<T1, T2>(ICommand<T1, T2> command, Exception exception)
        {
            if (command.IsExecuted)
                OnCommandFailedImpl(command, exception);
        }
        public void OnCommandFailed<T1, T2, T3>(ICommand<T1, T2, T3> command, Exception exception)
        {
            if (command.IsExecuted)
                OnCommandFailedImpl(command, exception);
        }
        
        private void OnCommandFailedImpl(ICommand command, Exception exception)
        {
            var index = command.Index;
            var binding = command.Binding;
            
            binding.RegisterCommandException(exception);
            
            ReturnCommand(command);

            if (binding.IsSequence)
            {
                FinishBindingExecution(binding);
                return;
            }
            
            if (binding.CheckAllReleased())
            {
                FinishBindingExecution(binding);
                return;
            }

            ProcessBindingCommand(binding, index + 1);
        }
        
        private void OnCommandFailedImpl<T1>(ICommand<T1> command, Exception exception)
        {
            var index = command.Index;
            var param01 = command.Param01;
            var binding = command.Binding;
            
            binding.RegisterCommandException(exception);
            
            ReturnCommand(command);

            if (binding.IsSequence)
            {
                FinishBindingExecution(binding, default);
                return;
            }
            
            if (binding.CheckAllReleased())
            {
                FinishBindingExecution(binding, default);
                return;
            }

            ProcessBindingCommand(binding, index + 1, param01);
        }
        
        private void OnCommandFailedImpl<T1, T2>(ICommand<T1, T2> command, Exception exception)
        {
            var index = command.Index;
            var param01 = command.Param01;
            var param02 = command.Param02;
            var binding = command.Binding;
            
            binding.RegisterCommandException(exception);
            
            ReturnCommand(command);

            if (binding.IsSequence)
            {
                FinishBindingExecution(binding, default, default);
                return;
            }
            
            if (binding.CheckAllReleased())
            {
                FinishBindingExecution(binding, default, default);
                return;
            }

            ProcessBindingCommand(binding, index + 1, param01, param02);
        }
        
        private void OnCommandFailedImpl<T1, T2, T3>(ICommand<T1, T2, T3> command, Exception exception)
        {
            var index = command.Index;
            var param01 = command.Param01;
            var param02 = command.Param02;
            var param03 = command.Param03;
            var binding = command.Binding;
            
            binding.RegisterCommandException(exception);
            
            ReturnCommand(command);

            if (binding.IsSequence)
            {
                FinishBindingExecution(binding, default, default, default);
                return;
            }
            
            if (binding.CheckAllReleased())
            {
                FinishBindingExecution(binding, default, default, default);
                return;
            }

            ProcessBindingCommand(binding, index + 1, param01, param02, param03);
        }
        
        /*
         * Event Processing.
         */

        public void ProcessEvent(Event type)
        {
            var bindings = GetBindings(type);
            if (bindings == null)
                return;
            
            _commandsExecutionIterationTokens++;

            foreach (var binding in bindings)
            {
                if (binding.IsExecuting)
                    throw new CommandBinderException(CommandBinderExceptionType.BindingAlreadyExecuting);
                
                binding.StartExecution();
                ProcessBindingCommand((CommandBinding)binding, 0);
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
                
                binding.StartExecution();
                ProcessBindingCommand((CommandBinding<T1>)binding, 0, param01);
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
                
                binding.StartExecution();
                ProcessBindingCommand((CommandBinding<T1, T2>)binding, 0, param01, param02);
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
                
                binding.StartExecution();
                ProcessBindingCommand((CommandBinding<T1, T2, T3>)binding, 0, param01, param02, param03);
            }

            _commandsExecutionIterationTokens--;
            TryUnbindScheduled();
        }
        
        /*
         * Command Processing.
         */
        
        private void ProcessBindingCommand(CommandBinding binding, int index)
        {
            if (binding.CheckAllReleased())
            {
                if (binding.CheckAllReleased())
                    FinishBindingExecution(binding);
                return;
            }
            
            if (index >= binding.Commands.Count)
                return;
            
            var command = (ICommand)GetCommand(binding.Commands[index]);
            command.Setup(index);
            command.PreExecute(binding);

            try
            {
                command.Execute();
            }
            catch (Exception exception)
            {
                OnCommandFailedImpl(command, exception);
                return;
            }
            
            command.PostExecute();

            if (!command.IsRetained)
            {
                if (command.IsFailed)
                    OnCommandFailedImpl(command, command.Exception);    
                else
                    OnCommandFinishedImpl(command);
                return;
            }
                
            if (!binding.IsSequence)
                ProcessBindingCommand(binding, index + 1);
        }

        private void ProcessBindingCommand<T1>(CommandBinding<T1> binding, int index, T1 param01)
        {
            if (binding.CheckAllReleased())
            {
                if (binding.CheckAllReleased())
                    FinishBindingExecution(binding, param01);
                return;
            }
            
            if (index >= binding.Commands.Count)
                return;
            
            var command = (ICommand<T1>)GetCommand(binding.Commands[index]);
            command.Setup(index);
            command.PreExecute(binding, param01);

            try
            {
                command.Execute(param01);
            }
            catch (Exception exception)
            {
                OnCommandFailedImpl(command, exception);
                return;
            }
            
            command.PostExecute();

            if (!command.IsRetained)
            {
                if (command.IsFailed)
                    OnCommandFailedImpl(command, command.Exception);    
                else
                    OnCommandFinishedImpl(command);
                return;
            }
                
            if (!binding.IsSequence)
                ProcessBindingCommand(binding, index + 1, param01);
        }
        
        private void ProcessBindingCommand<T1, T2>(CommandBinding<T1, T2> binding, int index, T1 param01, T2 param02)
        {
            if (binding.CheckAllReleased())
            {
                if (binding.CheckAllReleased())
                    FinishBindingExecution(binding, param01, param02);
                return;
            }
            
            if (index >= binding.Commands.Count)
                return;
            
            var command = (ICommand<T1, T2>)GetCommand(binding.Commands[index]);
            command.Setup(index);
            command.PreExecute(binding, param01, param02);

            try
            {
                command.Execute(param01, param02);
            }
            catch (Exception exception)
            {
                OnCommandFailedImpl(command, exception);
                return;
            }
            
            command.PostExecute();

            if (!command.IsRetained)
            {
                if (command.IsFailed)
                    OnCommandFailedImpl(command, command.Exception);    
                else
                    OnCommandFinishedImpl(command);
                return;
            }
                
            if (!binding.IsSequence)
                ProcessBindingCommand(binding, index + 1, param01, param02);
        }

        private void ProcessBindingCommand<T1, T2, T3>(CommandBinding<T1, T2, T3> binding, int index, T1 param01, T2 param02, T3 param03)
        {
            if (binding.CheckAllReleased())
            {
                if (binding.CheckAllReleased())
                    FinishBindingExecution(binding, param01, param02, param03);
                return;
            }
            
            if (index >= binding.Commands.Count)
                return;
            
            var command = (ICommand<T1, T2, T3>)GetCommand(binding.Commands[index]);
            command.Setup(index);
            command.PreExecute(binding, param01, param02, param03);

            try
            {
                command.Execute(param01, param02, param03);
            }
            catch (Exception exception)
            {
                OnCommandFailedImpl(command, exception);
                return;
            }
            
            command.PostExecute();

            if (!command.IsRetained)
            {
                if (command.IsFailed)
                    OnCommandFailedImpl(command, command.Exception);    
                else
                    OnCommandFinishedImpl(command);
                return;
            }
                
            if (!binding.IsSequence)
                ProcessBindingCommand(binding, index + 1, param01, param02, param03);
        }
        
        /*
         * Finishing.
         */
        
        private void FinishBindingExecution(CommandBinding binding)
        {
            if (TryHandleBindingFail(binding))
                return;
            
            binding.FinishExecution();
            UnbindOrScheduleIfOnce(binding);
            
            var @event = binding.CompleteEvent;
            if (@event != null)
                Dispatcher.Dispatch(@event);
        }

        private void FinishBindingExecution<T1>(CommandBinding<T1> binding, T1 param01)
        {
            if (TryHandleBindingFail(binding))
                return;
            
            binding.FinishExecution();
            UnbindOrScheduleIfOnce(binding);
            
            var @event = binding.CompleteEvent;
            if (@event != null)
                Dispatcher.Dispatch(@event, param01);
        }
        
        private void FinishBindingExecution<T1, T2>(CommandBinding<T1, T2> binding, T1 param01, T2 param02)
        {
            if (TryHandleBindingFail(binding))
                return;
            
            binding.FinishExecution();
            UnbindOrScheduleIfOnce(binding);
            
            var @event = binding.CompleteEvent;
            if (@event != null)
                Dispatcher.Dispatch(@event, param01, param02);
        }
        
        private void FinishBindingExecution<T1, T2, T3>(CommandBinding<T1, T2, T3> binding, T1 param01, T2 param02, T3 param03)
        {
            if (TryHandleBindingFail(binding))
                return;
            
            binding.FinishExecution();
            UnbindOrScheduleIfOnce(binding);
            
            var @event = binding.CompleteEvent;
            if (@event != null)
                Dispatcher.Dispatch(@event, param01, param02, param03);
        }

        private bool TryHandleBindingFail(CommandBindingBase binding)
        {
            if (!binding.HasFails) 
                return false;
            
            var exception = binding.CommandsFailed[0];
                
            binding.FinishExecution();
                
            var @event = binding.FailEvent;
            if (@event == null)
                throw exception;
                
            UnbindOrScheduleIfOnce(binding);
                
            Dispatcher.Dispatch(@event, exception);
            return true;
        }
        
        /*
         * Command Generic.
         */

        private ICommandBase GetCommand(Type commandType)
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