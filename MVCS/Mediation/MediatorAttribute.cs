using System;

namespace Build1.PostMVC.Core.MVCS.Mediation
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class MediatorAttribute : Attribute
    {
        internal readonly Type mediatorType;
        internal readonly Type viewInterfaceType;
        
        public MediatorAttribute(Type mediatorType)
        {
            this.mediatorType = mediatorType;
        }

        public MediatorAttribute(Type mediatorType, Type viewInterfaceType)
        {
            this.mediatorType = mediatorType;
            this.viewInterfaceType = viewInterfaceType;
        }
    }
}