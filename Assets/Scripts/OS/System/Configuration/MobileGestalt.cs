using UnityEngine;

public class MobileGestalt : MonoBehaviour
{
    [SerializeField, DisableInPlayMode] private IDeviceType deviceType;
    public IDeviceType DeviceType => deviceType;

    public enum IDeviceType
    {
        iPhone,
        iPad,
        iPod
    }

    private void SaveMobileGestalt()
    {

    }

    private void LoadMobileGestalt()
    {

    }
}
