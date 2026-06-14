using OS6.Events;
using OS6.IO.Displays;
using OS6.Kernel;
using UnityEngine;
using UnityEngine.UI;

public class Brightness_Slider : MonoBehaviour
{
    Display_Manager displayManager;
    Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        displayManager = System_Services.GetService<Display_Manager>();

        slider.onValueChanged.AddListener(displayManager.SetBrightness);
    }

    private void OnEnable()
    {
        GlobalEvents.Subscribe("BRIGHTNESS_CHANGED", Refresh);
        Refresh();
    }

    private void OnDisable()
    {
        GlobalEvents.Unsubscribe("BRIGHTNESS_CHANGED", Refresh);
    }

    void Refresh()
    {
        slider.value = displayManager.GetBrightness();
    }
}
