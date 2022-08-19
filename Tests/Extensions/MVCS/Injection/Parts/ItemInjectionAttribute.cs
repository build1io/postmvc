using Build1.PostMVC.Extensions.MVCS.Injection;

namespace Build1.PostMVC.Tests.Extensions.MVCS.Injection.Parts
{
    public class ItemInjectionAttribute : Inject
    {
        public readonly int param;

        public ItemInjectionAttribute(int param)
        {
            this.param = param;
        }
    }
}