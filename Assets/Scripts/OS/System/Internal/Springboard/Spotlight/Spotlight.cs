using OS6.Kernel;
using TMPro;
using UnityEngine;

public class Spotlight : MonoBehaviour
{
    [SerializeField] private TMP_Text placeholderText;

    MobileGestalt mobileGestalt;

    private void Awake()
    {
        mobileGestalt = System_Services.GetService<MobileGestalt>();
    }

    private void Start()
    {
        placeholderText.text = "Search " + mobileGestalt.DeviceType;
    }
}
