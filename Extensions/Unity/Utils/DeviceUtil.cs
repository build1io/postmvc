using Build1.PostMVC.Extensions.Unity.Modules.Device;

namespace Build1.PostMVC.Extensions.Unity.Utils
{
    public static class DeviceUtil
    {
        public static DeviceType GetDeviceType(DevicePlatform platform)
        {
            if ((DevicePlatform.iOS & platform) == DevicePlatform.iOS || (DevicePlatform.Android & platform) == DevicePlatform.Android)
                return ScreenUtil.GetDiagonalInches() >= 7f && ScreenUtil.GetAspectRatio() < 2f ? DeviceType.Tablet : DeviceType.Phone;
            return DeviceType.Desktop;
        }
    }
}