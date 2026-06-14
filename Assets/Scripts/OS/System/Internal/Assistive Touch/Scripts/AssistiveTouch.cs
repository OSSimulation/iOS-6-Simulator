using OS6.Events;
using OS6.IO.Audio;
using OS6.Kernel;
using UnityEngine;
using UnityEngine.UI;

public class AssistiveTouch : MonoBehaviour
{
    [SerializeField] private RectTransform nubbit;
    [SerializeField] private GameObject close;

    [SerializeField] private GameObject main;

    private CanvasGroup group;

    Vector3 mainWindowPos;
    Vector3 mainWindowScale;

    [SerializeField] Assistive_MenuManager manager;
    Audio_Manager audioManager;

    #region Options
    [Header("Device")]
    [SerializeField] private UKButtonMS muteToggle;
    [SerializeField] private Button lockScreen;
    #endregion

    private void Awake()
    {
        audioManager = System_Services.GetService<Audio_Manager>();
    }

    private void Start()
    {
        group = GetComponent<CanvasGroup>();
        group.alpha = 0.5f;

        mainWindowScale = main.transform.localScale;
        mainWindowPos = main.transform.position;

        LeanTween.scale(main, new Vector3(0, 0, 0), 0);
        LeanTween.move(main, nubbit.position, 0);

        ATInitialiseButtons();
        TOSSP6.LockDevice += StopAssistiveTouch;
    }

    private void ATInitialiseButtons()
    {
        // Device

        // More
        muteToggle.onButtonStateChanged += (index, state) => audioManager.SetSilentModeState(index == 1);
        muteToggle.SetButtonState(audioManager.GetSilentModeState() ? 1 : 0);

        lockScreen.onClick.AddListener(() => GlobalEvents.PublishEventMessage("BUTTON_POWER_PRESSED"));

        // Volume Up
        // Volume Dowm

        // Device More
    }

    public void StartAssistiveTouch()
    {
        close.SetActive(true);
        main.SetActive(true);
        nubbit.gameObject.SetActive(false);

        manager.OpenMenu("main");

        LeanTween.alphaCanvas(group, 1, 0.2f);

        LeanTween.scale(main, mainWindowScale, 0.2f);
        LeanTween.move(main, mainWindowPos, 0.2f);
    }

    public void StopAssistiveTouch()
    {
        close.SetActive(false);

        LeanTween.alphaCanvas(group, 0.5f, 0.2f);

        LeanTween.scale(main, new Vector3(0, 0, 0), 0.2f);
        LeanTween.move(main, transform.position, 0.2f).setOnComplete(() =>
        {
            nubbit.gameObject.SetActive(true);
        });
    }

    public void HideAssisstiveTouch()
    {
        close.SetActive(false);

        LeanTween.alphaCanvas(group, 0.5f, 0.2f);

        LeanTween.scale(main, new Vector3(0, 0, 0), 0.2f);
        LeanTween.move(main, transform.position, 0.2f).setOnComplete(() =>
        {
            nubbit.gameObject.SetActive(true);
            nubbit.gameObject.SetActive(false);
        });
    }
}
