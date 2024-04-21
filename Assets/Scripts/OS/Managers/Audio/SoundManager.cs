using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private TOSSP6 main;
    [SerializeField] private GameObject systemSFXPrefab;

    AudioSource source;

    private void Start()
    {
        source = systemSFXPrefab.GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip clip, float volume, SoundSources soundSource)
    {
        if (!main.isSilentMode)
        {
            if (soundSource == SoundSources.SYSTEM_SFX)
            {
                source.clip = clip;
                source.volume = main.GetSFXVolume();
                Instantiate(systemSFXPrefab);
            }

            if (soundSource == SoundSources.SYSTEM_NORMAL)
            {
                Debug.Log("Play Normal Sound");
            }
        }
    }
}

public enum SoundSources
{
    SYSTEM_SFX,
    SYSTEM_NORMAL
}
