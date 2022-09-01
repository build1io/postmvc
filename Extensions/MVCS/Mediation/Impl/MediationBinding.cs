using System;
using Build1.PostMVC.Core.Extensions.MVCS.Mediation.Api;

namespace Build1.PostMVC.Core.Extensions.MVCS.Mediation.Impl
{
    internal sealed class MediationBinding : IMediationBinding, IMediationBindingTo
    {
        public Type ViewType      { get; }
        public Type ViewInterface { get; }
        public Type MediatorType  { get; private set; }

        public MediationBinding(Type viewType)
        {
            ViewType = viewType;
        }

        public MediationBinding(Type viewType, Type viewInterface) : this(viewType)
        {
            ViewInterface = viewInterface;
        }

        public IMediationBinding To<T>() where T : class, IMediator, new()
        {
            MediatorType = typeof(T);
            return this;
        }
        
        public IMediationBinding To(Type mediatorType)
        {
            if (!mediatorType.IsClass)
                throw new MediationException(MediationExceptionType.MediatorTypeIsNotAClass, mediatorType.FullName);

            if (!typeof(IMediator).IsAssignableFrom(mediatorType))
                throw new MediationException(MediationExceptionType.MediatorTypeDoesntImplementRequiredInterface, $"{mediatorType.FullName} != {typeof(IMediator).FullName}");

            if (mediatorType.GetConstructor(Type.EmptyTypes) == null)
                throw new MediationException(MediationExceptionType.MediatorTypeDoesntHaveParameterlessConstructor, mediatorType.FullName);

            MediatorType = mediatorType;
            return this;
        }
    }
}