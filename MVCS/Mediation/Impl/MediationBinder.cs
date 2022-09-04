using System;
using System.Collections.Generic;
using Build1.PostMVC.Core.MVCS.Injection;
using Build1.PostMVC.Core.MVCS.Mediation.Api;

namespace Build1.PostMVC.Core.MVCS.Mediation.Impl
{
    internal sealed class MediationBinder : IMediationBinder
    {
        private readonly Dictionary<object, IMediationBinding> _bindings;
        private readonly Dictionary<object, IInjectionBinding> _bindingsCache;
        private readonly Dictionary<IView, IMediator>          _mediators;

        private readonly MediationMode    _mode;
        private readonly IInjectionBinder _injectionBinder; // Must be implemented like this to escape link un injection.

        public MediationBinder(MediationMode mode, IInjectionBinder injectionBinder)
        {
            _mode = mode;
            _injectionBinder = injectionBinder;

            _bindings = new Dictionary<object, IMediationBinding>();
            _bindingsCache = new Dictionary<object, IInjectionBinding>();
            _mediators = new Dictionary<IView, IMediator>();
        }

        /*
         * Binding.
         */

        public IMediationBindingTo Bind<T>() where T : class, IView
        {
            return BindImpl(typeof(T));
        }

        public IMediationBindingTo Bind<T, I>() where T : class, I
                                                where I : class, IView
        {
            return BindImpl(typeof(T), typeof(I));
        }

        public IMediationBindingTo Bind(Type viewType)
        {
            if (!typeof(IView).IsAssignableFrom(viewType))
                throw new MediationException(MediationExceptionType.ViewTypeDoesntImplementRequiredInterface, $"{viewType.FullName} != {typeof(IView).FullName}");
            return BindImpl(viewType);
        }

        public IMediationBindingTo Bind(Type viewType, Type viewInterfaceType)
        {
            if (!viewInterfaceType.IsInterface)
                throw new MediationException(MediationExceptionType.ViewInterfaceIsNotAnInterface, viewInterfaceType.FullName);

            if (!typeof(IView).IsAssignableFrom(viewInterfaceType))
                throw new MediationException(MediationExceptionType.ViewInterfaceDoesntImplementRequiredInterface, $"{viewInterfaceType.FullName} != {typeof(IView).FullName}");

            if (!viewInterfaceType.IsAssignableFrom(viewType))
                throw new MediationException(MediationExceptionType.ViewTypeDoesntImplementSpecifiedInterface, $"{viewType.FullName} != {viewInterfaceType.FullName}");

            return BindImpl(viewType, viewInterfaceType);
        }

        private IMediationBindingTo BindImpl(Type viewType)
        {
            if (!viewType.IsClass)
                throw new MediationException(MediationExceptionType.ViewTypeIsNotAClass, viewType.FullName);

            if (_bindings.ContainsKey(viewType))
                throw new MediationException(MediationExceptionType.ViewTypeAlreadyRegistered, viewType.FullName);

            var binding = new MediationBinding(viewType);
            _bindings.Add(viewType, binding);
            return binding;
        }

        private IMediationBindingTo BindImpl(Type viewType, Type viewInterfaceType)
        {
            if (viewType.IsInterface)
                throw new MediationException(MediationExceptionType.ViewTypeIsNotAClass, viewType.FullName);

            if (_bindings.ContainsKey(viewType))
                throw new MediationException(MediationExceptionType.ViewTypeAlreadyRegistered, viewType.FullName);

            if (_bindings.ContainsKey(viewInterfaceType))
                throw new MediationException(MediationExceptionType.ViewInterfaceAlreadyRegistered, viewInterfaceType.FullName);

            var binding = new MediationBinding(viewType, viewInterfaceType);
            _bindings.Add(viewType, binding);
            return binding;
        }

        /*
         * Rebinding.
         */

        public IMediationBindingTo Rebind(Type viewType)
        {
            Unbind(viewType);
            return Bind(viewType);
        }

        public IMediationBindingTo Rebind(Type viewType, Type viewInterfaceType)
        {
            Unbind(viewType);
            return Bind(viewType, viewInterfaceType);
        }

        /*
         * Unbinding.
         */

        public void Unbind(Type viewType)
        {
            _bindings.Remove(viewType);
        }

        /*
         * View.
         */

        public void OnViewAdd(IView view)
        {
            if (_mediators.ContainsKey(view))
                throw new MediationException(MediationExceptionType.ViewInstanceAlreadyAdded, view.GetType().FullName);

            var type = view.GetType();

            if (!_bindings.TryGetValue(type, out var binding) &&
                !TryRegisterMediatorUsingMetadata(type, out binding) &&
                _mode == MediationMode.Strict)
            {
                throw new MediationException(MediationExceptionType.MediationBindingNotFound, view.GetType().FullName);
            }

            _injectionBinder.Construct(view, true);

            if (TryCreateMediator(binding, out var mediator))
            {
                view.SetMediator(mediator);
                InjectViewAndDependencies(binding, mediator, view, true);
            }

            _mediators.Add(view, mediator);
        }

        public void OnViewRemove(IView view)
        {
            if (!_mediators.TryGetValue(view, out var mediator))
                throw new MediationException(MediationExceptionType.ViewInstanceNotFoundForRemoval, view.GetType().FullName);

            if (mediator != null)
                _injectionBinder.Destroy(mediator, true);

            _injectionBinder.Destroy(view, true);

            _mediators.Remove(view);
        }

        public void UpdateViewInjections(IView view)
        {
            _injectionBinder.Construct(view, false);

            if (view.Mediator == null)
                return;

            if (!_bindings.TryGetValue(view.GetType(), out var binding) && _mode == MediationMode.Strict)
                throw new MediationException(MediationExceptionType.MediationBindingNotFound, view.GetType().FullName);

            InjectViewAndDependencies(binding, view.Mediator, view, false);
        }

        /*
         * Private.
         */

        private bool TryCreateMediator(IMediationBinding binding, out IMediator mediator)
        {
            if (binding != null && binding.MediatorType != null)
            {
                mediator = (IMediator)Activator.CreateInstance(binding.MediatorType);
                return true;
            }

            mediator = null;
            return false;
        }

        private bool TryRegisterMediatorUsingMetadata(Type viewType, out IMediationBinding binding)
        {
            var injections = viewType.GetCustomAttributes(typeof(MediatorAttribute), true);
            if (injections.Length == 0)
            {
                binding = null;
                return false;
            }

            var attribute = (MediatorAttribute)injections[0];

            binding = attribute.viewInterfaceType != null
                          ? Rebind(viewType, attribute.viewInterfaceType).To(attribute.mediatorType)
                          : Rebind(viewType).To(attribute.mediatorType);

            return true;
        }
        
        private void InjectViewAndDependencies(IMediationBinding binding, IMediator mediator, IView view, bool triggerPostConstructors)
        {
            var injectionType = binding.ViewInterface != null ? binding.ViewInterface : binding.ViewType;
            if (_bindingsCache.TryGetValue(injectionType, out var injectionBinding))
            {
                injectionBinding.SetValue(view);
                _injectionBinder.Bind(injectionBinding);
                _injectionBinder.Construct(mediator, triggerPostConstructors);
                _injectionBinder.Unbind(injectionBinding);
            }
            else
            {
                injectionBinding = (IInjectionBinding)_injectionBinder.Bind(injectionType).ToValue(view);
                _injectionBinder.Construct(mediator, triggerPostConstructors);
                _injectionBinder.Unbind(injectionType);
                _bindingsCache.Add(injectionType, injectionBinding);
            }
        }
    }
}