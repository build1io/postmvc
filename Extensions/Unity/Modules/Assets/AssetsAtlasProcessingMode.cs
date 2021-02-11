namespace Build1.PostMVC.Extensions.Unity.Modules.Assets
{
    public enum AssetsAtlasProcessingMode
    {
        // An exception will be thrown if asset bundle is not registered.
        // An exception will be thrown if asset bundle is not loaded.
        // An exception will be thrown if SpriteAtlas is not bound in the bundle.
        Strict = 1,
        
        // No exceptions will be thrown.
        Soft = 2,
    }
}