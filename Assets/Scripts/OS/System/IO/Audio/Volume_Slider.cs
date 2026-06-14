using OS6.Events;
using OS6.IO.Audio;
using OS6.Kernel;
using UnityEngine;
using UnityEngine.UI;

public class Volume_Slider : MonoBehaviour
{
    Audio_Manager audioManager;
    Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        audioManager = System_Services.GetService<Audio_Manager>();

        slider.onValueChanged.AddListener(audioManager.SetVolume);

        Refresh();
    }

    private void OnEnable()
    {
        GlobalEvents.Subscribe("VOLUME_CHANGED", Refresh);
    }

    private void OnDisable()
    {
        GlobalEvents.Unsubscribe("VOLUME_CHANGED", Refresh);
    }

    void Refresh()
    {
        slider.SetValueWithoutNotify(audioManager.GetGeneralVolume());
    }
}
