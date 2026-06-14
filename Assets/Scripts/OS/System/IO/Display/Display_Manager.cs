using OS6.Events;
using OS6.Kernel;
using OS6.Power;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace OS6.IO.Displays
{
    public partial class Display_Manager : MonoBehaviour
    {
        int currentDisplay = 0;
        int previousDisplay = -1;

        float aspectRatio = 640f / 1136f;

        List<DisplayInfo> displayList = new();

        [SerializeField] private Button brightnessTap;
        [SerializeField] private CanvasGroup brightnessAlpha;

        private float currentBrightness;

        Power_Manager powerManager;

        private void OnEnable()
        {
            GlobalEvents.Subscribe("SYSTEM_DISPLAY_LOCKED", DisplayPowerOff);
            GlobalEvents.Subscribe("SYSTEM_DISPLAY_UNLOCKED", DisplayPowerOn);

            brightnessTap.onClick.AddListener(OnBrightnessTap);
        }

        private void OnDisable()
        {
            GlobalEvents.Unsubscribe("SYSTEM_DISPLAY_LOCKED", DisplayPowerOff);
            GlobalEvents.Unsubscribe("SYSTEM_DISPLAY_UNLOCKED", DisplayPowerOn);

            brightnessTap.onClick.RemoveListener(OnBrightnessTap);
        }

        private void OnBrightnessTap()
        {
            GlobalEvents.PublishEventMessage("BUTTON_POWER_PRESSED");
        }

        private void Awake()
        {
            powerManager = System_Services.GetService<Power_Manager>();
        }

        private void Start()
        {
            currentBrightness = 1f - brightnessAlpha.alpha;

#if UNITY_STANDALONE_WIN && !UNITY_EDITOR
        OverrideDPISettings();
#endif

            UpdateDisplayDetails();
            SetResolution(currentDisplay);

            DisplayInfo displayInfo = displayList[currentDisplay];
            SetWindowPosition(displayInfo);
        }

        private void Update()
        {
            if (UpdateDisplayDetails() || CheckWindowSize(currentDisplay))
                SetResolution(currentDisplay);
        }
    }
}
