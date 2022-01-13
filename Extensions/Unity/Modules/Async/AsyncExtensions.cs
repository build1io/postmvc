using System;
using System.Threading.Tasks;

namespace Build1.PostMVC.Extensions.Unity.Modules.Async
{
    public static class AsyncExtensions
    {
        public static void Resolve(this Task task, Action<Task> continuation)
        {
            AsyncResolverStaticAdapter.AsyncResolver.ResolveTask(task, continuation);
        }

        public static void Resolve<T>(this Task<T> task, Action<Task<T>> continuation)
        {
            AsyncResolverStaticAdapter.AsyncResolver.ResolveTask(task, continuation);
        }
    }
}