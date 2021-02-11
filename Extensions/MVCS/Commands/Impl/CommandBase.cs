using Build1.PostMVC.Extensions.MVCS.Commands.Api;

namespace Build1.PostMVC.Extensions.MVCS.Commands.Impl
{
    public abstract class CommandBase : ICommandBase
    {
        public int  SequenceId     { get; private set; }
        public bool IsRetained     { get; protected set; }
        public bool IsClean        { get; private set; }
        public bool ClearOnRelease { get; protected set; }
        
        protected ICommandBinder CommandBinder { get; private set; }

        protected CommandBase()
        {
            IsClean = true;
        }

        public void SetCommandBinder(ICommandBinder commandBinder)
        {
            CommandBinder = commandBinder;
        }

        public void SetSequenceId(int sequenceId)
        {
            SequenceId = sequenceId;
        }

        protected void Retain()
        {
            IsRetained = true;
        }

        protected abstract void Release();

        protected void Fail()
        {
            CommandBinder.StopCommand(this);
        }
        
        public void Clear()
        {
            OnClear();
            IsClean = true;
        }

        protected virtual void OnClear() { }
    }
}