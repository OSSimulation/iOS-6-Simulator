using OS6.Events;
using UnityEngine;
using UnityEngine.UI;

namespace OS6.IO.Displays
{
    public partial class Display_Manager
    {
        public float GetBrightness()
        {
            return currentBrightness;
        }

        public void AddBrightness(float brightness)
        {
            float alpha = Mathf.Clamp01(currentBrightness + brightness);
            SetBrightness(alpha);
        }

        public void SetBrightness(float brightness)
        {
            float clampedAlpha = Mathf.Clamp01(brightness);

            brightnessAlpha.alpha = 1f - clampedAlpha;
            currentBrightness = 1f - brightnessAlpha.alpha;

            GlobalEvents.PublishEventMessage("BRIGHTNESS_CHANGED");
        }

        private void DisplayPowerOff()
        {
            brightnessAlpha.alpha = 1f;
            brightnessAlpha.blocksRaycasts = true;
            brightnessAlpha.GetComponent<Image>().raycastTarget = true;
        }

        private void DisplayPowerOn()
        {
            brightnessAlpha.alpha = 1f - currentBrightness;
            brightnessAlpha.blocksRaycasts = false;
            brightnessAlpha.GetComponent<Image>().raycastTarget = false;
        }
    }
}
