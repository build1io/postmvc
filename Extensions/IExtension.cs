using Build1.PostMVC.Contexts;

namespace Build1.PostMVC.Extensions
{
    public interface IExtension
    {
        void SetContext(IContext context, IContext rootContext);
        void Initialize();
        void Setup();
        void Dispose();
    }
}