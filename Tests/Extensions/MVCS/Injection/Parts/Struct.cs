using Build1.PostMVC.Extensions.MVCS.Injection;

namespace Build1.PostMVC.Tests.Extensions.MVCS.Injection.Parts
{
    public struct Struct
    {
        public static int constructedTimes; 
        
        [PostConstruct]
        public void PostConstruct()
        {
            constructedTimes++;
        }
    }
}