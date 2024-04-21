using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Button_MultiImage : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public string buttonName;
    [SerializeField] private Sprite[] buttonImages;

    Toggle toggle;
    Image image;

    private void Start()
    {
        toggle = GetComponent<Toggle>();
        image = GetComponent<Image>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (toggle.isOn)
        {
            image.sprite = buttonImages[1];
        }
        else
        {
            image.sprite = buttonImages[3];
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (toggle.isOn)
        {
            image.sprite = buttonImages[0];
        }
        else
        {
            image.sprite = buttonImages[2];
        }
    }
}