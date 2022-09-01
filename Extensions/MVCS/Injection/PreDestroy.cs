using System;

namespace Build1.PostMVC.Core.Extensions.MVCS.Injection
{
    [AttributeUsage(AttributeTargets.Method)]
    public class PreDestroy : Attribute
    {
        public readonly int priority; 
        
        public PreDestroy()
        {
        }
        
        public PreDestroy(int priority)
        {
            this.priority = priority;
        }
    }
}