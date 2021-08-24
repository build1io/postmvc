using System;

namespace Build1.PostMVC.Extensions.MVCS.Commands.Api
{
    public interface ICommandPool
    {
        int GetAvailableInstancesCount<T>() where T : ICommandBase;
        int GetAvailableInstancesCount(Type commandType);

        int GetUsedInstancesCount<T>() where T : ICommandBase;
        int GetUsedInstancesCount(Type commandType);

        T TakeCommand<T>() where T : ICommandBase;
        T TakeCommand<T>(out bool isNewInstance) where T : ICommandBase;

        ICommandBase TakeCommand(Type commandType);
        ICommandBase TakeCommand(Type commandType, out bool isNewInstance);
        
        void ReturnCommand(ICommandBase command);
        
        ICommandBase InstantiateCommand(Type commandType);
    }
}