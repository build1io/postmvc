namespace Build1.PostMVC.Contexts.Impl
{
    internal enum ContextExceptionType
    {
        ContextAlreadyStarted = 1,
        ContextNotStarted     = 2,

        ExtensionInstanceNotFound                     = 10,
        ExtensionInstanceAlreadyExists                = 11,
        ExtensionInstallationAttemptAfterContextStart = 12,

        ModuleAlreadyAdded   = 20
    }
}