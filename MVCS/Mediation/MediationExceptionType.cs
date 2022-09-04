namespace Build1.PostMVC.Core.MVCS.Mediation
{
    public enum MediationExceptionType
    {
        ViewTypeAlreadyRegistered                 = 1,
        ViewTypeIsNotAClass                       = 2,
        ViewTypeDoesntImplementRequiredInterface  = 3,
        ViewTypeDoesntImplementSpecifiedInterface = 4,

        ViewInterfaceAlreadyRegistered                = 10,
        ViewInterfaceIsNotAnInterface                 = 11,
        ViewInterfaceDoesntImplementRequiredInterface = 12,

        MediatorTypeIsNotAClass                        = 20,
        MediatorTypeCantBeNull                         = 21,
        MediatorTypeDoesntImplementRequiredInterface   = 22,
        MediatorTypeDoesntHaveParameterlessConstructor = 23,

        MediationBindingNotFound = 30,

        ViewInstanceAlreadyAdded       = 40,
        ViewInstanceNotFoundForRemoval = 41
    }
}