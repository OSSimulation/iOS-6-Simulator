using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] private TOSSP6 main;

    [Space(10)]
    [Header("Sound")]
    [SerializeField] private Slider[] volumeSliders;

    [Space(10)]
    [Header("Brightnesss")]
    [SerializeField] private Slider brightnessSlider;

    private void Awake()
    {
        LoadSettingsFromPrefs();
    }

    private void Start()
    {
        foreach (Slider slider in volumeSliders)
        {
            slider.onValueChanged.AddListener(delegate { SetVolume(slider); });
        }
    }

    private void LoadSettingsFromPrefs()
    {
        main.isSilentMode = PlayerPrefs.GetInt("System_Ringer_Silent") != 0;
    }

    public void SetVolume(Slider changedSlider)
    {
        float volume = changedSlider.value;
        PlayerPrefs.SetFloat("System_Volume_SFX", volume);
        PlayerPrefs.Save();

        foreach (Slider slider in volumeSliders)
        {
            slider.value = PlayerPrefs.GetFloat("System_Volume_SFX");
        }
    }

    public void SetRinger(bool silent)
    {
        PlayerPrefs.SetInt("System_Ringer_Silent", (silent ? 1 : 0));
        PlayerPrefs.Save();
    }
}
