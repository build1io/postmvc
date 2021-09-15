namespace Build1.PostMVC.Utils.Pooling
{
    internal interface IPoolable
    {
        void OnTake();
        void OnReturn();
    }
}