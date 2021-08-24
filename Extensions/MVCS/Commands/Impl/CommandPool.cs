using System;
using System.Collections.Generic;
using Build1.PostMVC.Extensions.MVCS.Commands.Api;

namespace Build1.PostMVC.Extensions.MVCS.Commands.Impl
{
    public sealed class CommandPool : ICommandPool
    {
        private readonly Dictionary<Type, Stack<ICommandBase>>   _availableInstances;
        private readonly Dictionary<Type, HashSet<ICommandBase>> _usedInstances;
        
        public CommandPool()
        {
            _availableInstances = new Dictionary<Type, Stack<ICommandBase>>();
            _usedInstances = new Dictionary<Type, HashSet<ICommandBase>>();
        }
        
        /*
         * Counts.
         */

        public int GetAvailableInstancesCount<T>() where T : ICommandBase
        {
            return GetAvailableInstancesCount(typeof(T));
        }

        public int GetAvailableInstancesCount(Type commandType)
        {
            return GetCommandAvailableInstances(commandType, false)?.Count ?? 0;
        }

        public int GetUsedInstancesCount<T>() where T : ICommandBase
        {
            return GetUsedInstancesCount(typeof(T));
        }

        public int GetUsedInstancesCount(Type commandType)
        {
            return GetCommandUsedInstances(commandType, false)?.Count ?? 0;
        }
        
        /*
         * Take.
         */

        public T TakeCommand<T>() where T : ICommandBase
        {
            return (T)TakeCommand(typeof(T));
        }

        public T TakeCommand<T>(out bool isNewInstance) where T : ICommandBase
        {
            return (T)TakeCommand(typeof(T), out isNewInstance);
        }

        public ICommandBase TakeCommand(Type commandType)
        {
            return TakeCommand(commandType, out var isNewInstance);
        }

        public ICommandBase TakeCommand(Type commandType, out bool isNewInstance)
        {
            ICommandBase command;

            var usedInstances = GetCommandUsedInstances(commandType, true);
            var availableInstances = GetCommandAvailableInstances(commandType, true);
            if (availableInstances.Count > 0)
            {
                isNewInstance = false;
                command = availableInstances.Pop();
                usedInstances.Add(command);
                return command;
            }

            isNewInstance = true;
            command = InstantiateCommand(commandType);
            usedInstances.Add(command);
            return command;
        }
        
        /*
         * Return.
         */

        public void ReturnCommand(ICommandBase command)
        {
            if (command == null)
                return;

            var commandType = command.GetType();
            var usedInstances = GetCommandUsedInstances(commandType, false);
            if (usedInstances == null || !usedInstances.Remove(command))
                return;
            
            GetCommandAvailableInstances(commandType, false).Push(command);
        }
        
        /*
         * Instantiate.
         */

        public ICommandBase InstantiateCommand(Type commandType)
        {
            return (ICommandBase)Activator.CreateInstance(commandType);
        }

        /*
         * Private.
         */

        private Stack<ICommandBase> GetCommandAvailableInstances(Type commandType, bool create)
        {
            if (_availableInstances.TryGetValue(commandType, out var availableInstances) || !create)
                return availableInstances;
            availableInstances = new Stack<ICommandBase>();
            _availableInstances.Add(commandType, availableInstances);
            return availableInstances;
        }
        
        private HashSet<ICommandBase> GetCommandUsedInstances(Type commandType, bool create)
        {
            if (_usedInstances.TryGetValue(commandType, out var usedInstances) || !create) 
                return usedInstances;
            usedInstances = new HashSet<ICommandBase>();
            _usedInstances.Add(commandType, usedInstances);
            return usedInstances;
        }
    }
}