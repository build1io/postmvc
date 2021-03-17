using UnityEngine;

namespace Build1.PostMVC.Extensions.Unity.Utils
{
    public static class ScreenUtil
    {
        public static float GetAspectRatio()
        {
            return Mathf.Max((float)Screen.width, Screen.height) / Mathf.Min(Screen.width, Screen.height); 
        }
        
        public static float GetDiagonalInches()
        {
            var screenWidth = Screen.width / Screen.dpi;
            var screenHeight = Screen.height / Screen.dpi;
            return Mathf.Sqrt(Mathf.Pow(screenWidth, 2) + Mathf.Pow(screenHeight, 2));
        }
    }
}