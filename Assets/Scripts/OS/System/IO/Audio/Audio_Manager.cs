using Newtonsoft.Json;
using OS6.Events;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.Audio;

namespace OS6.IO.Audio
{
    public class Audio_Manager : MonoBehaviour
    {
        [SerializeField] private AudioMixer mixer;

        private const float MIN_DB = -80f;
        [SerializeField] private float step = 0.0625f;
        [SerializeField] private float repeatDelay = 0.25f;
        [SerializeField] private float repeateRate = 0.1f;
        bool isSilent, holdingUp, holdingDown;
        float repeatTime;

        [SerializeField] private TextAsset defaultConfig;
        private string audioFileName = "com.os6.audio.json";
        private string audioFilePath => System_Locations.SYS_PREFERENCES + audioFileName;
        private AudioData data = new();

        float autoSaveTimer;
        bool shouldSave;

        // bool buttonsChangeRinger = false;

        private void Start()
        {
            LoadVolume();

            // Read "buttons-change-ringer" from
            // com.os6.preferences.sounds.json file
        }

        private void Update()
        {
            HandleVolumeKey(KeyCode.Equals, ref holdingUp, step);
            HandleVolumeKey(KeyCode.Minus, ref holdingDown, -step);

            if (holdingUp || holdingDown)
            {
                if (Time.time >= repeatTime)
                {
                    float volume = holdingUp ? step : -step;
                    AddVolume(volume);
                    repeatTime = Time.time + repeateRate;
                }
            }

            if (Input.GetKeyDown(KeyCode.BackQuote))
                ToggleSilentModeState();


            if (shouldSave)
            {
                autoSaveTimer += Time.deltaTime;

                if (autoSaveTimer > 5f)
                {
                    SaveVolume();
                    shouldSave = false;
                    autoSaveTimer = 0f;
                }
            }
        }

        void HandleVolumeKey(KeyCode key, ref bool flag, float volume)
        {
            if (Input.GetKeyDown(key))
            {
                flag = true;
                AddVolume(volume);
                repeatTime = Time.time + repeatDelay;
            }

            if (Input.GetKeyUp(key))
                flag = false;
        }

        public float GetGeneralVolume()
        {
            mixer.GetFloat("General", out float db);
            return Mathf.Pow(10f, db / 20f);
        }

        private float GetVolumeForMix()
        {
            mixer.GetFloat("General", out float db);
            return Mathf.Pow(10f, db / 20f);
        }

        public void AddVolume(float volume)
        {
            float vol = Mathf.Clamp01(GetVolumeForMix() + volume);
            SetVolume(vol);
        }

        public void SetVolume(float normalised)
        {
            float db = normalised <= 0.0001f
                ? MIN_DB
                : Mathf.Log10(normalised) * 20f;

            mixer.SetFloat("General", db);

            GlobalEvents.PublishEventMessage("VOLUME_CHANGED");

            shouldSave = true;
        }

        public bool GetSilentModeState()
        {
            return isSilent;
        }

        private void ApplySilentModeState()
        {
            float masterVolume = isSilent ? MIN_DB : 0f;

            mixer.SetFloat("Master", masterVolume);
            GlobalEvents.PublishEventMessage("VOLUME_SILENT_CHANGED");
        }

        public void SetSilentModeState(bool state)
        {
            isSilent = state;

            ApplySilentModeState();
            shouldSave = true;
        }

        public void ToggleSilentModeState()
        {
            SetSilentModeState(!isSilent);
        }

        private void SaveVolume()
        {
            // Assign silent bool eventually
            data.global.isSilentMode = isSilent;

            data.output.General.volume = GetGeneralVolume();
            mixer.GetFloat("Ringer", out data.output.Ringer.volume);
            mixer.GetFloat("Assistant", out data.output.Assistant.volume);

            string json = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(audioFilePath, json);
        }

        private void LoadVolume()
        {
            if (!File.Exists(audioFilePath))
            {
                File.WriteAllText(audioFilePath, defaultConfig.text);
                Debug.Log("Creating Audio Config file...");
            }

            string json = File.ReadAllText(audioFilePath);
            data = JsonConvert.DeserializeObject<AudioData>(json);

            SetSilentModeState(data.global.isSilentMode);

            SetVolume(data.output.General.volume);
        }

        [Serializable]
        private class AudioData
        {
            public GlobalSettings global = new();
            public OutputSettings output = new();
        }

        [Serializable]
        private class GlobalSettings
        {
            public bool isSilentMode;
        }

        [Serializable]
        private class OutputSettings
        {
            public VolumeData General = new();
            public VolumeData Ringer = new();
            public VolumeData Assistant = new();
        }

        [Serializable]
        private class VolumeData
        {
            public float volume;
        }
    }
}
