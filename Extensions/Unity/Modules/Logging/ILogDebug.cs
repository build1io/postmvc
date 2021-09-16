using System;

namespace Build1.PostMVC.Extensions.Unity.Modules.Logging
{
    public interface ILogDebug
    {
        void Debug(string message);
        void Debug(Exception exception);
    }
}