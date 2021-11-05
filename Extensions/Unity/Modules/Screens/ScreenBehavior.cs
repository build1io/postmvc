namespace Build1.PostMVC.Extensions.Unity.Modules.Screens
{
    public enum ScreenBehavior
    {
        /// <summary>
        /// Used in cases when you need to hide a screen.
        /// </summary>
        None = 0,
        
        /// <summary>
        /// Screen will replace an active screen if there is one
        /// </summary>
        Replace = 1,
        
        /// <summary>
        /// Screen will be open in background. Active screen will not be affected.
        /// </summary>
        OpenInBackground = 2,
        
        /// <summary>
        /// Screen will be open on top of the current one and will not affect current screen.
        /// </summary>
        OpenOnTop = 3
    }
}