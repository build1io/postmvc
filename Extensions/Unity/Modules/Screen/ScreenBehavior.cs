namespace Build1.PostMVC.Extensions.Unity.Modules.Screen
{
    public enum ScreenBehavior
    {
        /// Screen will replace an active screen if there is one.
        Default = 0,

        /// Screen will be open in background. Active screen will not be affected.
        OpenInBackground = 1
    }
}