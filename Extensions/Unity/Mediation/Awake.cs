using System;
using Build1.PostMVC.Extensions.MVCS.Injection;

namespace Build1.PostMVC.Extensions.Unity.Mediation
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class Awake : PostConstruct
    {
    }
}