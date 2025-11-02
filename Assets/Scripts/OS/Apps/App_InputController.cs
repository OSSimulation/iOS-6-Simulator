using UnityEngine;
using UnityEngine.EventSystems;

public class App_InputController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private TOSSP6 main;

    [SerializeField] private AppButtonType type;

    private bool isLongPressing;
    private float holdTime = 1f;
    private float holdTimer;

    private bool isWiggling;

    private void Awake()
    {
        GameObject OS = GameObject.FindGameObjectWithTag("TOSSP6");
        main = OS.GetComponent<TOSSP6>();
    }

    private void Update()
    {
        if (isLongPressing && !main.isWiggleMode || isLongPressing && !main.isSwitcherWiggleMode)
        {
            holdTimer += Time.deltaTime;

            if (holdTimer >= holdTime)
            {
                if (type == AppButtonType.NORMAL)
                {
                    main.isWiggleMode = true;
                }
                else if (type == AppButtonType.SWITCHER)
                {
                    main.isSwitcherWiggleMode = true;
                }
            }
        }

        StartWiggleMode();

        if (main.openApps.Count == 0)
        {
            main.isSwitcherWiggleMode = false;
        }

        if (main.isWiggleMode && GetComponent<App_Draggable>() != null)
        {
            GetComponent<App_Draggable>().enabled = true;
        }
        else if (!main.isWiggleMode && GetComponent<App_Draggable>() != null)
        {
            GetComponent<App_Draggable>().enabled = false;
        }

        if (main.isSwitcherWiggleMode && GetComponent<App_ForceQuit>() != null)
        {
            GetComponent<App_ForceQuit>().enabled = true;
        }
        else if (!main.isSwitcherWiggleMode && GetComponent<App_ForceQuit>() != null)
        {
            GetComponent<App_ForceQuit>().enabled = false;
        }

        if ((!main.isWiggleMode && type == AppButtonType.NORMAL) ||
            (!main.isSwitcherWiggleMode && type == AppButtonType.SWITCHER))
        {
            StopWiggleMode();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!main.isWiggleMode)
        {
            isLongPressing = true;
            holdTimer = 0;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isLongPressing = false;
        holdTimer = 0;
    }

    private void StartWiggleMode()
    {
        if (!isWiggling)
        {
            isWiggling = true;

            float initialOffset = Random.Range(-5f, 5f);
            transform.rotation = Quaternion.Euler(0f, 0f, initialOffset);

            WiggleLoop();
        }
    }

    private void WiggleLoop()
    {
        float wiggleSpeed = 0.1f;

        if (type == AppButtonType.NORMAL && main.isWiggleMode)
        {
            LeanTween.rotateZ(gameObject, Random.Range(-5f, -0.15f), wiggleSpeed).setEase(LeanTweenType.easeInOutSine).setOnComplete(() =>
            {
                LeanTween.rotateZ(gameObject, Random.Range(0.15f, 5f), wiggleSpeed).setEase(LeanTweenType.easeInOutSine).setOnComplete(WiggleLoop);
            });
        }
        else if (type == AppButtonType.SWITCHER && main.isSwitcherWiggleMode)
        {
            LeanTween.rotateZ(gameObject, Random.Range(-5f, 0.15f), wiggleSpeed).setOnComplete(() =>
            {
                LeanTween.rotateZ(gameObject, Random.Range(0.15f, 5f), wiggleSpeed).setOnComplete(WiggleLoop);
            });
        }
    }

    private void StopWiggleMode()
    {
        isWiggling = false;

        if (type == AppButtonType.NORMAL)
            LeanTween.cancel(gameObject);

        gameObject.transform.rotation = Quaternion.identity;
    }
}

public enum AppButtonType
{
    NORMAL,
    SWITCHER
}
