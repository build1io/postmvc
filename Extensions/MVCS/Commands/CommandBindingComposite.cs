using System;
using System.Collections.Generic;
using Build1.PostMVC.Extensions.MVCS.Events;

namespace Build1.PostMVC.Extensions.MVCS.Commands
{
    public sealed class CommandBindingComposite : ICommandBinding
    {
        private readonly List<CommandBinding> _bindings;

        internal CommandBindingComposite(CommandBinding binding)
        {
            _bindings = new List<CommandBinding> { binding };
        }

        public ICommandBinding And(Event @event)
        {
            _bindings.Add(_bindings[0].CommandBinder.Bind(@event));
            return this;
        }

        public ICommandBinding Bind(Event @event)
        {
            _bindings.Add(_bindings[0].CommandBinder.Bind(@event));
            return this;
        }
        
        /*
         * Params 0.
         */

        public ICommandBinding To<TCommand>() where TCommand : Command, new()
        {
            foreach (var binding in _bindings)
                binding.To<TCommand>();
            return this;
        }

        public ICommandBinding To0<TCommand>() where TCommand : Command, new()
        {
            foreach (var binding in _bindings)
                binding.To0<TCommand>();
            return this;
        }
        
        /*
         * Params 1.
         */

        public ICommandBinding To1<TCommand>(int param) where TCommand : Command<int>, new()
        {
            foreach (var binding in _bindings)
                binding.To1<TCommand>(param);
            return this;
        }
        
        public ICommandBinding To1<TCommand>(float param) where TCommand : Command<float>, new()
        {
            foreach (var binding in _bindings)
                binding.To1<TCommand>(param);
            return this;
        }

        public ICommandBinding To1<TCommand>(bool param) where TCommand : Command<bool>, new()
        {
            foreach (var binding in _bindings)
                binding.To1<TCommand>(param);
            return this;
        }

        public ICommandBinding To1<TCommand, TCP1>(TCP1 param01) where TCommand : Command<TCP1>, new()
        {
            foreach (var binding in _bindings)
                binding.To1<TCommand, TCP1>(param01);
            return this;
        }
        
        /*
         * Params 2.
         */

        public ICommandBinding To2<TCommand, TCP1, TCP2>(TCP1 param01, TCP2 param02) where TCommand : Command<TCP1, TCP2>, new()
        {
            foreach (var binding in _bindings)
                binding.To2<TCommand, TCP1, TCP2>(param01, param02);
            return this;
        }

        /*
         * Params 3.
         */

        public ICommandBinding To3<TCommand, TCP1, TCP2, TCP3>(TCP1 param01, TCP2 param02, TCP3 param03) where TCommand : Command<TCP1, TCP2, TCP3>, new()
        {
            foreach (var binding in _bindings)
                binding.To3<TCommand, TCP1, TCP2, TCP3>(param01, param02, param03);
            return this;
        }

        /*
         * Events.
         */
        
        public ICommandBinding OnComplete(Event @event)
        {
            foreach (var binding in _bindings)
                binding.OnComplete(@event);
            return this;
        }

        public ICommandBinding OnBreak(Event @event)
        {
            foreach (var binding in _bindings)
                binding.OnBreak(@event);
            return this;
        }

        public ICommandBindingBase OnFail(Event<Exception> @event)
        {
            foreach (var binding in _bindings)
                binding.OnFail(@event);
            return this;
        }

        public ICommandBindingBase OnFail(Event @event)
        {
            foreach (var binding in _bindings)
                binding.OnFail(@event);
            return this;
        }
        
        /*
         * Other.
         */

        public ICommandBindingBase InParallel()
        {
            foreach (var binding in _bindings)
                binding.InParallel();
            return this;
        }
        
        public ICommandBindingBase InSequence()
        {
            foreach (var binding in _bindings)
                binding.InSequence();
            return this;
        }

        public ICommandBindingBase Once()
        {
            foreach (var binding in _bindings)
                binding.Once();
            return this;
        }
        
        public ICommandBindingBase Once(OnceBehavior behavior)
        {
            foreach (var binding in _bindings)
                binding.Once(behavior);
            return this;
        }
        
        public ICommandBindingBase UnbindOnQuit()
        {
            foreach (var binding in _bindings)
                binding.UnbindOnQuit();
            return this;
        }
    }

    public sealed class CommandBindingComposite<T1> : ICommandBinding<T1>
    {
        private readonly List<CommandBinding<T1>> _bindings;

        internal CommandBindingComposite(CommandBinding<T1> binding)
        {
            _bindings = new List<CommandBinding<T1>> { binding };
        }

        public ICommandBinding<T1> And(Event<T1> @event)
        {
            _bindings.Add(_bindings[0].CommandBinder.Bind(@event));
            return this;
        }

        public ICommandBinding<T1> Bind(Event<T1> @event)
        {
            _bindings.Add(_bindings[0].CommandBinder.Bind(@event));
            return this;
        }
        
        /*
         * Params 0.
         */
        
        public ICommandBinding<T1> To0<TCommand>() where TCommand : Command, new()
        {
            foreach (var binding in _bindings)
                binding.To0<TCommand>();
            return this;
        }

        public ICommandBinding<T1> To<TCommand>() where TCommand : Command<T1>, new()
        {
            foreach (var binding in _bindings)
                binding.To<TCommand>();
            return this;
        }
        
        /*
         * Params 1.
         */
        
        public ICommandBinding<T1> To<TCommand>(int param) where TCommand : Command<int>, new()
        {
            foreach (var binding in _bindings)
                binding.To1<TCommand>(param);
            return this;
        }
        
        public ICommandBinding<T1> To<TCommand>(float param) where TCommand : Command<float>, new()
        {
            foreach (var binding in _bindings)
                binding.To1<TCommand>(param);
            return this;
        }

        public ICommandBinding<T1> To<TCommand>(bool param) where TCommand : Command<bool>, new()
        {
            foreach (var binding in _bindings)
                binding.To1<TCommand>(param);
            return this;
        }

        public ICommandBinding<T1> To<TCommand, TCP1>(TCP1 param01) where TCommand : Command<TCP1>, new()
        {
            foreach (var binding in _bindings)
                binding.To<TCommand, TCP1>(param01);
            return this;
        }

        public ICommandBinding<T1> To1<TCommand>() where TCommand : Command<T1>, new()
        {
            foreach (var binding in _bindings)
                binding.To1<TCommand>();
            return this;
        }
        
        public ICommandBinding<T1> To1<TCommand>(int param) where TCommand : Command<int>, new()
        {
            foreach (var binding in _bindings)
                binding.To1<TCommand>(param);
            return this;
        }
        
        public ICommandBinding<T1> To1<TCommand>(float param) where TCommand : Command<float>, new()
        {
            foreach (var binding in _bindings)
                binding.To1<TCommand>(param);
            return this;
        }

        public ICommandBinding<T1> To1<TCommand>(bool param) where TCommand : Command<bool>, new()
        {
            foreach (var binding in _bindings)
                binding.To1<TCommand>(param);
            return this;
        }

        public ICommandBinding<T1> To1<TCommand, TCP1>(TCP1 param01) where TCommand : Command<TCP1>, new()
        {
            foreach (var binding in _bindings)
                binding.To1<TCommand, TCP1>(param01);
            return this;
        }
        
        /*
         * Params 2.
         */

        public ICommandBinding<T1> To2<TCommand, TCP1, TCP2>(TCP1 param01, TCP2 param02) where TCommand : Command<TCP1, TCP2>, new()
        {
            foreach (var binding in _bindings)
                binding.To2<TCommand, TCP1, TCP2>(param01, param02);
            return this;
        }

        /*
         * Params 3.
         */

        public ICommandBinding<T1> To3<TCommand, TCP1, TCP2, TCP3>(TCP1 param01, TCP2 param02, TCP3 param03) where TCommand : Command<TCP1, TCP2, TCP3>, new()
        {
            foreach (var binding in _bindings)
                binding.To3<TCommand, TCP1, TCP2, TCP3>(param01, param02, param03);
            return this;
        }

        /*
         * Events.
         */
        
        public ICommandBinding<T1> OnComplete(Event<T1> @event)
        {
            foreach (var binding in _bindings)
                binding.OnComplete(@event);
            return this;
        }

        public ICommandBinding<T1> OnComplete(Event @event)
        {
            foreach (var binding in _bindings)
                binding.OnComplete(@event);
            return this;
        }

        public ICommandBinding<T1> OnBreak(Event<T1> @event)
        {
            foreach (var binding in _bindings)
                binding.OnBreak(@event);
            return this;
        }

        public ICommandBinding<T1> OnBreak(Event @event)
        {
            foreach (var binding in _bindings)
                binding.OnBreak(@event);
            return this;
        }
        
        public ICommandBindingBase OnFail(Event<Exception> @event)
        {
            foreach (var binding in _bindings)
                binding.OnFail(@event);
            return this;
        }

        public ICommandBindingBase OnFail(Event @event)
        {
            foreach (var binding in _bindings)
                binding.OnFail(@event);
            return this;
        }
        
        /*
         * Events.
         */
        
        public ICommandBindingBase InParallel()
        {
            foreach (var binding in _bindings)
                binding.InParallel();
            return this;
        }
        
        public ICommandBindingBase InSequence()
        {
            foreach (var binding in _bindings)
                binding.InSequence();
            return this;
        }
        
        public ICommandBindingBase Once()
        {
            foreach (var binding in _bindings)
                binding.Once();
            return this;
        }
        
        public ICommandBindingBase Once(OnceBehavior behavior)
        {
            foreach (var binding in _bindings)
                binding.Once(behavior);
            return this;
        }
        
        public ICommandBindingBase UnbindOnQuit()
        {
            foreach (var binding in _bindings)
                binding.UnbindOnQuit();
            return this;
        }
    }

    public sealed class CommandBindingComposite<T1, T2> : ICommandBinding<T1, T2>
    {
        private readonly List<CommandBinding<T1, T2>> _bindings;

        internal CommandBindingComposite(CommandBinding<T1, T2> binding)
        {
            _bindings = new List<CommandBinding<T1, T2>> { binding };
        }

        public ICommandBinding<T1, T2> And(Event<T1, T2> @event)
        {
            _bindings.Add(_bindings[0].CommandBinder.Bind(@event));
            return this;
        }

        public ICommandBinding<T1, T2> Bind(Event<T1, T2> @event)
        {
            _bindings.Add(_bindings[0].CommandBinder.Bind(@event));
            return this;
        }
        
        /*
         * Params 0.
         */
        
        public ICommandBinding<T1, T2> To0<TCommand>() where TCommand : Command, new()
        {
            foreach (var binding in _bindings)
                binding.To0<TCommand>();
            return this;
        }
        
        /*
         * Params 1.
         */
        
        public ICommandBinding<T1, T2> To1<TCommand>() where TCommand : Command<T1>, new()
        {
            foreach (var binding in _bindings)
                binding.To1<TCommand>();
            return this;
        }
        
        public ICommandBinding<T1, T2> To1<TCommand>(int param) where TCommand : Command<int>, new()
        {
            foreach (var binding in _bindings)
                binding.To1<TCommand>(param);
            return this;
        }
        
        public ICommandBinding<T1, T2> To1<TCommand>(float param) where TCommand : Command<float>, new()
        {
            foreach (var binding in _bindings)
                binding.To1<TCommand>(param);
            return this;
        }

        public ICommandBinding<T1, T2> To1<TCommand>(bool param) where TCommand : Command<bool>, new()
        {
            foreach (var binding in _bindings)
                binding.To1<TCommand>(param);
            return this;
        }

        public ICommandBinding<T1, T2> To1<TCommand, TCP1>(TCP1 param01) where TCommand : Command<TCP1>, new()
        {
            foreach (var binding in _bindings)
                binding.To1<TCommand, TCP1>(param01);
            return this;
        }
        
        /*
         * Params 2.
         */

        public ICommandBinding<T1, T2> To<TCommand>() where TCommand : Command<T1, T2>, new()
        {
            foreach (var binding in _bindings)
                binding.To<TCommand>();
            return this;
        }

        public ICommandBinding<T1, T2> To2<TCommand>() where TCommand : Command<T1, T2>, new()
        {
            foreach (var binding in _bindings)
                binding.To2<TCommand>();
            return this;
        }

        public ICommandBinding<T1, T2> To2<TCommand, TCP1, TCP2>(TCP1 param01, TCP2 param02) where TCommand : Command<TCP1, TCP2>, new()
        {
            foreach (var binding in _bindings)
                binding.To2<TCommand, TCP1, TCP2>(param01, param02);
            return this;
        }
        
        /*
         * Params 3.
         */

        public ICommandBinding<T1, T2> To3<TCommand, TCP1, TCP2, TCP3>(TCP1 param01, TCP2 param02, TCP3 param03) where TCommand : Command<TCP1, TCP2, TCP3>, new()
        {
            foreach (var binding in _bindings)
                binding.To3<TCommand, TCP1, TCP2, TCP3>(param01, param02, param03);
            return this;
        }
        
        /*
         * Events.
         */

        public ICommandBinding<T1, T2> OnComplete(Event<T1, T2> @event)
        {
            foreach (var binding in _bindings)
                binding.OnComplete(@event);
            return this;
        }

        public ICommandBinding<T1, T2> OnComplete(Event<T1> @event)
        {
            foreach (var binding in _bindings)
                binding.OnComplete(@event);
            return this;
        }

        public ICommandBinding<T1, T2> OnComplete(Event @event)
        {
            foreach (var binding in _bindings)
                binding.OnComplete(@event);
            return this;
        }

        public ICommandBinding<T1, T2> OnBreak(Event<T1, T2> @event)
        {
            foreach (var binding in _bindings)
                binding.OnBreak(@event);
            return this;
        }

        public ICommandBinding<T1, T2> OnBreak(Event<T1> @event)
        {
            foreach (var binding in _bindings)
                binding.OnBreak(@event);
            return this;
        }

        public ICommandBinding<T1, T2> OnBreak(Event @event)
        {
            foreach (var binding in _bindings)
                binding.OnBreak(@event);
            return this;
        }
        
        public ICommandBindingBase OnFail(Event<Exception> @event)
        {
            foreach (var binding in _bindings)
                binding.OnFail(@event);
            return this;
        }

        public ICommandBindingBase OnFail(Event @event)
        {
            foreach (var binding in _bindings)
                binding.OnFail(@event);
            return this;
        }
        
        /*
         * Other.
         */
        
        public ICommandBindingBase InParallel()
        {
            foreach (var binding in _bindings)
                binding.InParallel();
            return this;
        }
        
        public ICommandBindingBase InSequence()
        {
            foreach (var binding in _bindings)
                binding.InSequence();
            return this;
        }
        
        public ICommandBindingBase Once()
        {
            foreach (var binding in _bindings)
                binding.Once();
            return this;
        }
        
        public ICommandBindingBase Once(OnceBehavior behavior)
        {
            foreach (var binding in _bindings)
                binding.Once(behavior);
            return this;
        }
        
        public ICommandBindingBase UnbindOnQuit()
        {
            foreach (var binding in _bindings)
                binding.UnbindOnQuit();
            return this;
        }
    }

    public sealed class CommandBindingComposite<T1, T2, T3> : ICommandBinding<T1, T2, T3>
    {
        private readonly List<CommandBinding<T1, T2, T3>> _bindings;
        
        internal CommandBindingComposite(CommandBinding<T1, T2, T3> binding)
        {
            _bindings = new List<CommandBinding<T1, T2, T3>> { binding };
        }
        
        public ICommandBinding<T1, T2, T3> And(Event<T1, T2, T3> @event)
        {
            _bindings.Add(_bindings[0].CommandBinder.Bind(@event));
            return this;
        }

        public ICommandBinding<T1, T2, T3> Bind(Event<T1, T2, T3> @event)
        {
            _bindings.Add(_bindings[0].CommandBinder.Bind(@event));
            return this;
        }
        
        /*
         * Params 0.
         */

        public ICommandBinding<T1, T2, T3> To0<TCommand>() where TCommand : Command, new()
        {
            foreach (var binding in _bindings)
                binding.To0<TCommand>();
            return this;
        }
        
        /*
         * Params 1.
         */

        public ICommandBinding<T1, T2, T3> To1<TCommand>() where TCommand : Command<T1>, new()
        {
            foreach (var binding in _bindings)
                binding.To1<TCommand>();
            return this;
        }
        
        public ICommandBinding<T1, T2, T3> To1<TCommand>(int param) where TCommand : Command<int>, new()
        {
            foreach (var binding in _bindings)
                binding.To1<TCommand>(param);
            return this;
        }
        
        public ICommandBinding<T1, T2, T3> To1<TCommand>(float param) where TCommand : Command<float>, new()
        {
            foreach (var binding in _bindings)
                binding.To1<TCommand>(param);
            return this;
        }

        public ICommandBinding<T1, T2, T3> To1<TCommand>(bool param) where TCommand : Command<bool>, new()
        {
            foreach (var binding in _bindings)
                binding.To1<TCommand>(param);
            return this;
        }

        public ICommandBinding<T1, T2, T3> To1<TCommand, TCP1>(TCP1 param01) where TCommand : Command<TCP1>, new()
        {
            foreach (var binding in _bindings)
                binding.To1<TCommand, TCP1>(param01);
            return this;
        }
        
        /*
         * Params 2.
         */
        
        public ICommandBinding<T1, T2, T3> To<TCommand>() where TCommand : Command<T1, T2, T3>, new()
        {
            foreach (var binding in _bindings)
                binding.To<TCommand>();
            return this;
        }

        public ICommandBinding<T1, T2, T3> To2<TCommand>() where TCommand : Command<T1, T2>, new()
        {
            foreach (var binding in _bindings)
                binding.To2<TCommand>();
            return this;
        }

        public ICommandBinding<T1, T2, T3> To2<TCommand, TCP1, TCP2>(TCP1 param01, TCP2 param02) where TCommand : Command<TCP1, TCP2>, new()
        {
            foreach (var binding in _bindings)
                binding.To2<TCommand, TCP1, TCP2>(param01, param02);
            return this;
        }
        
        /*
         * Params 3.
         */

        public ICommandBinding<T1, T2, T3> To3<TCommand>() where TCommand : Command<T1, T2, T3>, new()
        {
            foreach (var binding in _bindings)
                binding.To3<TCommand>();
            return this;
        }

        public ICommandBinding<T1, T2, T3> To3<TCommand, TCP1, TCP2, TCP3>(TCP1 param01, TCP2 param02, TCP3 param03) where TCommand : Command<TCP1, TCP2, TCP3>, new()
        {
            foreach (var binding in _bindings)
                binding.To3<TCommand, TCP1, TCP2, TCP3>(param01, param02, param03);
            return this;
        }
        
        /*
         * Events.
         */

        public ICommandBinding<T1, T2, T3> OnComplete(Event<T1, T2, T3> @event)
        {
            foreach (var binding in _bindings)
                binding.OnComplete(@event);
            return this;
        }

        public ICommandBinding<T1, T2, T3> OnComplete(Event<T1, T2> @event)
        {
            foreach (var binding in _bindings)
                binding.OnComplete(@event);
            return this;
        }

        public ICommandBinding<T1, T2, T3> OnComplete(Event<T1> @event)
        {
            foreach (var binding in _bindings)
                binding.OnComplete(@event);
            return this;
        }

        public ICommandBinding<T1, T2, T3> OnComplete(Event @event)
        {
            foreach (var binding in _bindings)
                binding.OnComplete(@event);
            return this;
        }

        public ICommandBinding<T1, T2, T3> OnBreak(Event<T1, T2, T3> @event)
        {
            foreach (var binding in _bindings)
                binding.OnBreak(@event);
            return this;
        }

        public ICommandBinding<T1, T2, T3> OnBreak(Event<T1, T2> @event)
        {
            foreach (var binding in _bindings)
                binding.OnBreak(@event);
            return this;
        }

        public ICommandBinding<T1, T2, T3> OnBreak(Event<T1> @event)
        {
            foreach (var binding in _bindings)
                binding.OnBreak(@event);
            return this;
        }

        public ICommandBinding<T1, T2, T3> OnBreak(Event @event)
        {
            foreach (var binding in _bindings)
                binding.OnBreak(@event);
            return this;
        }
        
        public ICommandBindingBase OnFail(Event<Exception> @event)
        {
            foreach (var binding in _bindings)
                binding.OnFail(@event);
            return this;
        }

        public ICommandBindingBase OnFail(Event @event)
        {
            foreach (var binding in _bindings)
                binding.OnFail(@event);
            return this;
        }
        
        /*
         * Other.
         */
        
        public ICommandBindingBase InParallel()
        {
            foreach (var binding in _bindings)
                binding.InParallel();
            return this;
        }
        
        public ICommandBindingBase InSequence()
        {
            foreach (var binding in _bindings)
                binding.InSequence();
            return this;
        }
        
        public ICommandBindingBase Once()
        {
            foreach (var binding in _bindings)
                binding.Once();
            return this;
        }
        
        public ICommandBindingBase Once(OnceBehavior behavior)
        {
            foreach (var binding in _bindings)
                binding.Once(behavior);
            return this;
        }
        
        public ICommandBindingBase UnbindOnQuit()
        {
            foreach (var binding in _bindings)
                binding.UnbindOnQuit();
            return this;
        }
    }
}