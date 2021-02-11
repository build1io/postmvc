using Build1.PostMVC.Extensions.MVCS.Injection;
using Build1.PostMVC.Extensions.Unity.Modules.Logging;
using UnityEngine;
using Logger = Build1.PostMVC.Extensions.Unity.Modules.Logging.Logger;
using ILogger = Build1.PostMVC.Extensions.Unity.Modules.Logging.ILogger;

namespace Build1.PostMVC.Extensions.Unity.Modules.Display.Impl
{
    public sealed class DisplayController : IDisplayController
    {
        [Logger(LogLevel.Verbose)] public ILogger Logger { get; set; }
        
        public float AspectRatio    { get; private set; }
        public float DiagonalInches { get; private set; }
        
        [PostConstruct]
        public void PostConstruct()
        {
            DiagonalInches = CalculateDeviceDiagonalInches();
            AspectRatio = CalculateScreenAspectRatio();
            
            Logger.Debug(() => $"Diagonal: {DiagonalInches}\" Aspect Ratio: {AspectRatio}");
        }
        
        private float CalculateScreenAspectRatio()
        {
            return Mathf.Max((float)UnityEngine.Screen.width, UnityEngine.Screen.height) / Mathf.Min(UnityEngine.Screen.width, UnityEngine.Screen.height); 
        }

        private float CalculateDeviceDiagonalInches()
        {
            var screenWidth = UnityEngine.Screen.width / UnityEngine.Screen.dpi;
            var screenHeight = UnityEngine.Screen.height / UnityEngine.Screen.dpi;
            return Mathf.Sqrt(Mathf.Pow(screenWidth, 2) + Mathf.Pow(screenHeight, 2));
        }
    }
}