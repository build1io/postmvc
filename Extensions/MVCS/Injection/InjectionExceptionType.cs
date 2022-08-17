namespace Build1.PostMVC.Extensions.MVCS.Injection
{
    public enum InjectionExceptionType
    {
        InstanceIsOfPrimitiveType = 1,
        InstanceIsMissing         = 2,

        ValueNotProvided  = 10,
        ValueNotDestroyed = 11,

        InjectionTypeMismatch              = 20,
        ConstructingTypeCantBeInstantiated = 21,

        CircularDependency                     = 100,
        CircularDependencyIsCounterMissing     = 101,
        CircularDependencyCounterIsAlreadyZero = 102
    }
}