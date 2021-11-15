namespace Build1.PostMVC.Extensions.Unity.Modules.Assets
{
    public enum AssetsExceptionType
    {
        BundleAlreadyRegistered      = 1,
        BundleNotRegistered          = 2,
        BundleNotFound               = 3,
        BundleLoadingNetworkError    = 4,
        BundleLoadingHttpError       = 5,
        BundleLoadingProcessingError = 6,
        BundleNotLoaded              = 7,

        AssetNotFound = 10,

        AtlasNotFound        = 20,
        AtlasBundleNotLoaded = 21
    }
}