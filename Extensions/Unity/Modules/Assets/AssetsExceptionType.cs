namespace Build1.PostMVC.Extensions.Unity.Modules.Assets
{
    public enum AssetsExceptionType
    {
        AssetNotFound                = 1,
        AtlasNotFound                = 2,
        AtlasBundleNotLoaded         = 3,
        BundleNotFound               = 4,
        BundleLoadingNetworkError    = 5,
        BundleLoadingHttpError       = 6,
        BundleLoadingProcessingError = 7,
        BundleNotLoaded              = 8
    }
}