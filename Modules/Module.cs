using Build1.PostMVC.Core.Contexts;

namespace Build1.PostMVC.Core.Modules
{
    public abstract class Module
    {
        private IContext _context;
        
        /*
         * Public.
         */

        internal void SetContext(IContext context)
        {
            _context = context;
        }

        /*
         * Protected.
         */
        
        protected void AddModule<T>() where T : Module, new()
        {
            _context.AddModule<T>();
        }
    }
}