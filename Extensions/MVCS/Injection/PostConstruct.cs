using System;

namespace Build1.PostMVC.Core.Extensions.MVCS.Injection
{
    [AttributeUsage(AttributeTargets.Method)]
    public class PostConstruct: Attribute
    {
        public readonly int priority; 
        
        public PostConstruct()
        {
        }
        
        public PostConstruct(int priority)
        {
            this.priority = priority;
        }
    }
}