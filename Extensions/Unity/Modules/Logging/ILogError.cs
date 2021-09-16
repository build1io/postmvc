using System;

namespace Build1.PostMVC.Extensions.Unity.Modules.Logging
{
    public interface ILogError
    {
        void Error(string message);
        void Error(Exception exception);
    }
}