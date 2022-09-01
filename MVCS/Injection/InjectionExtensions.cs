namespace Build1.PostMVC.Core.MVCS.Injection
{
    public static class InjectionExtensions
    {
        public static T Destroy<T>(this T instance, IInjectionBinder injectionBinder, bool triggerPreDestroys) where T : class
        {
            injectionBinder.Destroy(instance, triggerPreDestroys);
            return null;
        }
    }
}