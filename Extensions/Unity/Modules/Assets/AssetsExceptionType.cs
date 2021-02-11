namespace Build1.PostMVC.Extensions.Unity.Modules.Assets
{
    public enum AssetsExceptionType
    {
        AssetNotFound             = 1,
        AtlasNotFound             = 2,
        BundleNotRegistered       = 3,
        BundleAlreadyRegistered   = 4,
        BundleNotFound            = 5,
        BundleLoadingNetworkError = 6,
        BundleLoadingHttpError    = 7,
        BundleNotLoaded           = 8
    }
}