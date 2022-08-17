using Build1.PostMVC.Extensions.MVCS.Injection;

namespace Build1.PostMVC.Tests.Extensions.MVCS.Injection.Parts
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