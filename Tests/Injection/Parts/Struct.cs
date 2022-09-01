using Build1.PostMVC.Core.MVCS.Injection;

namespace Build1.PostMVC.Core.Tests.Injection.Parts
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