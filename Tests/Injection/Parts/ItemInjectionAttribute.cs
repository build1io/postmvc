using Build1.PostMVC.Core.Extensions.MVCS.Injection;

namespace Build1.PostMVC.Core.Tests.Injection.Parts
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