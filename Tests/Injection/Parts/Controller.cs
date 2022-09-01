using Build1.PostMVC.Core.Extensions.MVCS.Injection;

namespace Build1.PostMVC.Core.Tests.Injection.Parts
{
    public sealed class Controller : IController
    {
        public static int constructedTimes;
        public static int destroyedTimes;
        
        [PostConstruct]
        public void PostConstruct()
        {
            constructedTimes++;
        }

        [PreDestroy]
        public void PreDestroy()
        {
            destroyedTimes++;
        }
    }
}