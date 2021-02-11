namespace Build1.PostMVC.Extensions.Unity.Modules.Device
{
    public interface IDeviceDependentConfiguration
    {
        DevicePlatform DevicePlatform { get; }
        DeviceType     DeviceType     { get; }
    }
}