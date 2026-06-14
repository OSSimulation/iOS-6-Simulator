using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UKButtonMS : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Image image;

    [SerializeField] private List<ButtonState> states = new();

    private int currentState;
    private bool isPressed;

    public System.Action<int, ButtonState> onButtonStateChanged;
    public ButtonState CurrentState => states.Count > 0 ? states[currentState] : null;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (states.Count == 0) return;

        currentState = (currentState + 1) % states.Count;
        UpdateGraphic();
        onButtonStateChanged?.Invoke(currentState, CurrentState);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
        UpdateGraphic();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
        UpdateGraphic();
    }

    private void UpdateGraphic()
    {
        if (CurrentState == null) return;

        bool usePressedSprite = isPressed && CurrentState.pressedSprite != null;
        image.sprite = usePressedSprite ? CurrentState.pressedSprite : CurrentState.normalSprite;
    }

    public void SetButtonState(int stateIndex)
    {
        currentState = Mathf.Clamp(stateIndex, 0, states.Count - 1);
        UpdateGraphic();
        onButtonStateChanged?.Invoke(currentState, CurrentState);
    }
}

[System.Serializable]
public class ButtonState
{
    public Sprite normalSprite;
    public Sprite pressedSprite;
}
