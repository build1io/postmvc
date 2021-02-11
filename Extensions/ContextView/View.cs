using System;
using Build1.PostMVC.Extensions.MVCS.Injection;

namespace Build1.PostMVC.Extensions.ContextView
{
    [AttributeUsage(AttributeTargets.Property)]
    public class View : Inject
    {
    }
}