namespace Build1.PostMVC.Core.Extensions.MVCS.Mediation.Impl
{
    internal enum MediationExceptionType
    {
        ViewTypeAlreadyRegistered                 = 1,
        ViewTypeIsNotAClass                       = 2,
        ViewTypeDoesntImplementRequiredInterface  = 3,
        ViewTypeDoesntImplementSpecifiedInterface = 4,

        ViewInterfaceAlreadyRegistered                = 10,
        ViewInterfaceIsNotAnInterface                 = 11,
        ViewInterfaceDoesntImplementRequiredInterface = 12,

        MediatorTypeIsNotAClass                        = 20,
        MediatorTypeDoesntImplementRequiredInterface   = 21,
        MediatorTypeDoesntHaveParameterlessConstructor = 22,
        
        MediationBindingNotFound = 30,

        ViewInstanceAlreadyAdded       = 40,
        ViewInstanceNotFoundForRemoval = 41
    }
}