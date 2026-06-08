using TMPro;
using UnityEngine;

public class Home_Spotlight : MonoBehaviour
{
    [SerializeField] private TOSSP6 main;
    [SerializeField] private TMP_Text placeholderText;

    void Start()
    {
        placeholderText.text = "Search " + main.GetDeviceName();
    }

    void Update()
    {
        placeholderText.text = "Search " + main.GetDeviceName();
    }
}
