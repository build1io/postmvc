using UnityEngine;

namespace Build1.PostMVC.Extensions.Unity.Utils.Path
{
    public static class PathUtil
    {
        public static string GetPath(PathAttribute pathAttribute)
        {
            #if UNITY_ANDROID && !UNITY_EDITOR

            // Automatic => /storage/emulated/0/Android/data/[PACKAGE_NAME]/files: OK

            // Internal, Persistent, Absolute => /data/user/0/[PACKAGE_NAME]/files: OK
            // Internal, Persistent, Default => /data/user/0/[PACKAGE_NAME]/files: OK
            // Internal, Persistent, Canonical => /data/data/[PACKAGE_NAME]/files: OK

            // Internal, Cached, Absolute => /data/user/0/[PACKAGE_NAME]/cache: OK
            // Internal, Cached, Default => /data/user/0/[PACKAGE_NAME]/cache: OK
            // Internal, Cached, Canonical => /data/data/[PACKAGE_NAME]/cache: OK

            // External, Persistent, Absolute => /storage/emulated/0/Android/data/[PACKAGE_NAME]/files: OK
            // External, Persistent, Default => /storage/emulated/0/Android/data/[PACKAGE_NAME]/files: OK
            // External, Persistent, Canonical => /storage/emulated/0/Android/data/[PACKAGE_NAME]/files: OK

            // External, Cached, Absolute => /storage/emulated/0/Android/data/[PACKAGE_NAME]/cache: OK
            // External, Cached, Default => /storage/emulated/0/Android/data/[PACKAGE_NAME]/cache: OK
            // External, Cached, Canonical => /storage/emulated/0/Android/data/[PACKAGE_NAME]/cache: OK

            if (pathAttribute == PathAttribute.Automatic)
                return Application.persistentDataPath;
            
            var activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            var activity = activityClass.GetStatic<AndroidJavaObject>("currentActivity");

            var endMethod = string.Empty;
            if ((pathAttribute & PathAttribute.Absolute) == PathAttribute.Absolute)
                endMethod = "getAbsolutePath";
            else if ((pathAttribute & PathAttribute.Canonical) == PathAttribute.Canonical)
                endMethod = "getCanonicalPath";
            else if ((pathAttribute & PathAttribute.Default) == PathAttribute.Default)
                endMethod = "getPath";

            if ((pathAttribute & PathAttribute.Internal) == PathAttribute.Internal && (pathAttribute & PathAttribute.Persistent) == PathAttribute.Persistent)
                return activity.Call<AndroidJavaObject>("getFilesDir").Call<string>(endMethod);
            
            if ((pathAttribute & PathAttribute.Internal) == PathAttribute.Internal && (pathAttribute & PathAttribute.Cached) == PathAttribute.Cached)
                return activity.Call<AndroidJavaObject>("getCacheDir").Call<string>(endMethod);
            
            if ((pathAttribute & PathAttribute.External) == PathAttribute.External && (pathAttribute & PathAttribute.Persistent) == PathAttribute.Persistent)
                return activity.Call<AndroidJavaObject>("getExternalFilesDir", (object)null).Call<string>(endMethod);
            
            if ((pathAttribute & PathAttribute.External) == PathAttribute.External && (pathAttribute & PathAttribute.Cached) == PathAttribute.Cached)
                return activity.Call<AndroidJavaObject>("getExternalCacheDir").Call<string>(endMethod);
            
            throw new System.ArgumentOutOfRangeException(nameof(pathAttribute), pathAttribute, null);
            
            #else
            
            // PathAttributes are applicable only to Android platform at the moment.
            return Application.persistentDataPath;
            
            #endif
        }
    }
}