namespace Build1.PostMVC.Extensions.Unity.Modules.Assets
{
    public enum AssetsExceptionType
    {
        AgentAlreadyInitialised = 1,

        UnknownBundleType = 10,
        UnknownBundle     = 11,

        BundleNotFound               = 20,
        BundleLoadingNetworkError    = 21,
        BundleLoadingHttpError       = 22,
        BundleLoadingProcessingError = 23,
        BundleLoadingStorageError    = 24,
        BundleNotLoaded              = 25,
        BundleLoadingAborted         = 26,

        AssetNotFound = 30,

        AtlasNotFound        = 40,
        AtlasBundleNotLoaded = 41,
        
        BundleInfoUpdateError = 50
    }
}