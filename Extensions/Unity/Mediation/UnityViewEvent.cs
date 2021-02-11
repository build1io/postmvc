using Build1.PostMVC.Extensions.MVCS.Events;

namespace Build1.PostMVC.Extensions.Unity.Mediation
{
    public abstract class UnityViewEvent
    {
        public static readonly Event Enabled  = new Event();
        public static readonly Event Disabled = new Event();
    }
}