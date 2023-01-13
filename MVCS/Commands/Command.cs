using Build1.PostMVC.Core.MVCS.Commands.Impl;

namespace Build1.PostMVC.Core.MVCS.Commands
{
    public abstract class Command : CommandBase
    {
        internal sealed override void Reset()
        {
            base.Reset();
        }

        internal sealed override void InternalExecute(ICommandParams param, ICommandParams paramAdditional)
        {
            Execute();
        }
        
        public abstract void Execute();
    }

    public abstract class Command<T1> : CommandBase
    {
        public T1 Param01 { get; private set; }
        
        internal sealed override void Reset()
        {
            base.Reset();

            Param01 = default;
        }

        internal sealed override void InternalExecute(ICommandParams param, ICommandParams paramAdditional)
        {
            if (param is ICommandParams<T1> @params)
                Param01 = @params.Param01;
            else if (paramAdditional is ICommandParams<T1> paramsOverride)
                Param01 = paramsOverride.Param01;
            else
                throw new CommandBinderException(CommandBinderExceptionType.IncompatibleCommand, $"{param}, {paramAdditional}");
            
            Execute(Param01);
        }
        
        public abstract void Execute(T1 param01);
    }

    public abstract class Command<T1, T2> : CommandBase
    {
        public T1 Param01 { get; private set; }
        public T2 Param02 { get; private set; }

        internal sealed override void Reset()
        {
            base.Reset();

            Param01 = default;
            Param02 = default;
        }
        
        internal sealed override void InternalExecute(ICommandParams param, ICommandParams paramAdditional)
        {
            if (paramAdditional != null)
            {
                if (param is ICommandParams<T1> params01 && paramAdditional is ICommandParams<T2> params02)
                {
                    Param01 = params01.Param01;
                    Param02 = params02.Param01;
                }
                else if (paramAdditional is ICommandParams<T1, T2> paramsOverride)
                {
                    Param01 = paramsOverride.Param01;
                    Param02 = paramsOverride.Param02;
                }
                else
                {
                    throw new CommandBinderException(CommandBinderExceptionType.IncompatibleCommand, $"{param}, {paramAdditional}");
                }
            }
            else
            {
                if (param is ICommandParams<T1, T2> @params)
                {
                    Param01 = @params.Param01;
                    Param02 = @params.Param02;
                }
                else
                {
                    throw new CommandBinderException(CommandBinderExceptionType.IncompatibleCommand, $"{param}");
                }
            }
            
            Execute(Param01, Param02);
        }

        public abstract void Execute(T1 param01, T2 param02);
    }

    public abstract class Command<T1, T2, T3> : CommandBase
    {
        public T1 Param01 { get; private set; }
        public T2 Param02 { get; private set; }
        public T3 Param03 { get; private set; }
        
        internal sealed override void Reset()
        {
            base.Reset();

            Param01 = default;
            Param02 = default;
            Param03 = default;
        }
        
        internal sealed override void InternalExecute(ICommandParams param, ICommandParams paramAdditional)
        {
            if (paramAdditional != null)
            {
                if (param is ICommandParams<T1> params01 && paramAdditional is ICommandParams<T2, T3> params02)
                {
                    Param01 = params01.Param01;
                    Param02 = params02.Param01;
                    Param03 = params02.Param02;
                }
                else if (param is ICommandParams<T1, T2> params03 && paramAdditional is ICommandParams<T3> params04)
                {
                    Param01 = params03.Param01;
                    Param02 = params03.Param02;
                    Param03 = params04.Param01;
                }
                else if (paramAdditional is ICommandParams<T1, T2, T3> paramsOverride)
                {
                    Param01 = paramsOverride.Param01;
                    Param02 = paramsOverride.Param02;
                    Param03 = paramsOverride.Param03;
                }
                else
                {
                    throw new CommandBinderException(CommandBinderExceptionType.IncompatibleCommand, $"{param}, {paramAdditional}");    
                }
            }
            else
            {
                if (param is ICommandParams<T1, T2, T3> @params)
                {
                    Param01 = @params.Param01;
                    Param02 = @params.Param02;
                    Param03 = @params.Param03;
                }
                else
                {
                    throw new CommandBinderException(CommandBinderExceptionType.IncompatibleCommand, $"{param}");
                }
            }
            
            Execute(Param01, Param02, Param03);
        }

        public abstract void Execute(T1 param01, T2 param02, T3 param03);
    }
}