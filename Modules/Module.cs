using Build1.PostMVC.Contexts;

namespace Build1.PostMVC.Modules
{
    public abstract class Module : IModule
    {
        private IContext _context;
        
        /*
         * Public.
         */

        public void SetContext(IContext context)
        {
            _context = context;
        }

        public virtual void Configure() { }

        /*
         * Protected.
         */
        
        protected void AddModule<T>() where T : IModule, new()
        {
            _context.AddModule<T>();
        }
    }
}