using UnityEngine;
using UnityEngine.UI;

public class Slider_Increment : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private float incrementAmount = 6.25f;
    [SerializeField] private float repeatRate;

    private float currentValue;
    private bool isHoldingKey = false;
    private bool isIncreasing = false;
    private bool isDecreasing = false;
    private float lastKeyPressTime;

    void Start()
    {
        currentValue = slider.value;
        slider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    void Update()
    {
        HandleKeyInput(KeyCode.Equals, ref isIncreasing);
        HandleKeyInput(KeyCode.Minus, ref isDecreasing, false);

        if (isHoldingKey)
        {
            if (Time.time - lastKeyPressTime > repeatRate)
            {
                if (isIncreasing)
                    IncrementSlider();
                else if (isDecreasing)
                    DecrementSlider();

                lastKeyPressTime = Time.time;
            }
        }
    }

    void HandleKeyInput(KeyCode keyCode, ref bool actionFlag, bool increase = true)
    {
        if (Input.GetKeyDown(keyCode))
        {
            actionFlag = true;
            lastKeyPressTime = Time.time;
            isHoldingKey = true;
            if (increase)
                IncrementSlider();
            else
                DecrementSlider();
        }
        else if (Input.GetKeyUp(keyCode))
        {
            actionFlag = false;
            isHoldingKey = false;
        }
    }

    void IncrementSlider()
    {
        currentValue += incrementAmount;
        UpdateSliderValue();
    }

    void DecrementSlider()
    {
        currentValue -= incrementAmount;
        UpdateSliderValue();
    }

    void OnSliderValueChanged(float newValue)
    {
        currentValue = newValue;
    }

    void UpdateSliderValue()
    {
        float roundedValue = Mathf.Round(currentValue / incrementAmount) * incrementAmount;
        slider.value = roundedValue;
    }
}
