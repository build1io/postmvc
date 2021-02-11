using Build1.PostMVC.Contexts;

namespace Build1.PostMVC.Modules
{
    public interface IModule
    {
        void SetContext(IContext context);
        void Configure();
    }
}