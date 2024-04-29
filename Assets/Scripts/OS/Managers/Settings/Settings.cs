using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] private TOSSP6 main;

    [Space(10)]
    [Header("Sound")]
    [SerializeField] private Slider volumeSlider;

    [Space(10)]
    [Header("Brightnesss")]
    [SerializeField] private Slider brightnessSlider;

    private void Awake()
    {
        LoadSettingsFromPrefs();
    }

    private void LoadSettingsFromPrefs()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("System_Volume_SFX");
        main.isSilentMode = PlayerPrefs.GetInt("System_Ringer_Silent") != 0;
    }

    public void SetVolume()
    {
        float volume = volumeSlider.value;
        PlayerPrefs.SetFloat("System_Volume_SFX", volume);
        PlayerPrefs.Save();
    }

    public void SetRinger(bool silent)
    {
        PlayerPrefs.SetInt("System_Ringer_Silent", (silent ? 1 : 0));
        PlayerPrefs.Save();
    }


}
