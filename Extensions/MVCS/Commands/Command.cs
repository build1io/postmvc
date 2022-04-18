using Build1.PostMVC.Extensions.MVCS.Commands.Impl;

namespace Build1.PostMVC.Extensions.MVCS.Commands
{
    public abstract class Command : CommandBase
    {
        internal sealed override void Reset()
        {
            base.Reset();
        }

        internal sealed override void InternalExecute(CommandParamsBase param, CommandParamsBase paramAdditional)
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

        internal sealed override void InternalExecute(CommandParamsBase param, CommandParamsBase paramAdditional)
        {
            if (param is CommandParams<T1> @params)
                Param01 = @params.Param01;
            else if (paramAdditional is CommandParams<T1> paramsOverride)
                Param01 = paramsOverride.Param01;
            else
                throw new CommandBinderException(CommandBinderExceptionType.IncompatibleCommand);
            
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
        
        internal sealed override void InternalExecute(CommandParamsBase param, CommandParamsBase paramAdditional)
        {
            if (param is CommandParams<T1, T2> @params)
            {
                Param01 = @params.Param01;
                Param02 = @params.Param02;
            }
            else if (paramAdditional is CommandParams<T1, T2> paramsOverride)
            {
                Param01 = paramsOverride.Param01;
                Param02 = paramsOverride.Param02;
            }
            else if (param is CommandParams<T1> params01 && paramAdditional is CommandParams<T2> params02)
            {
                Param01 = params01.Param01;
                Param02 = params02.Param01;
            }
            else
            {
                throw new CommandBinderException(CommandBinderExceptionType.IncompatibleCommand);    
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
        
        internal sealed override void InternalExecute(CommandParamsBase param, CommandParamsBase paramAdditional)
        {
            if (param is CommandParams<T1, T2, T3> @params)
            {
                Param01 = @params.Param01;
                Param02 = @params.Param02;
                Param03 = @params.Param03;
            }
            else if (paramAdditional is CommandParams<T1, T2, T3> paramsOverride)
            {
                Param01 = paramsOverride.Param01;
                Param02 = paramsOverride.Param02;
                Param03 = paramsOverride.Param03;
            }
            else if (param is CommandParams<T1> params01 && paramAdditional is CommandParams<T2, T3> params02)
            {
                Param01 = params01.Param01;
                Param02 = params02.Param01;
                Param03 = params02.Param02;
            }
            else if (param is CommandParams<T1, T2> params03 && paramAdditional is CommandParams<T3> params04)
            {
                Param01 = params03.Param01;
                Param02 = params03.Param02;
                Param03 = params04.Param01;
            }
            else
            {
                throw new CommandBinderException(CommandBinderExceptionType.IncompatibleCommand);    
            }
            
            Execute(Param01, Param02, Param03);
        }

        public abstract void Execute(T1 param01, T2 param02, T3 param03);
    }
}