using UnityEngine;

public class App_Anim : MonoBehaviour
{
    [SerializeField] private GameObject safeZone;

    private void Awake()
    {
        safeZone.SetActive(false);
    }

    private void Start()
    {
        LeanTween.scale(gameObject.GetComponent<RectTransform>(), new Vector2(0, 0), 0f).setOnComplete(() =>
        {
            LeanTween.scale(gameObject.GetComponent<RectTransform>(), new Vector2(1, 1), 0.5f).setEase(LeanTweenType.easeInQuint).setOnComplete(() =>
            {
                LeanTween.cancel(gameObject);
            });
        });

        safeZone.SetActive(true);
    }

    public void Cancel()
    {
        LeanTween.cancel(gameObject);
    }

    public void AppZoomIn()
    {
        LeanTween.scale(gameObject.GetComponent<RectTransform>(), new Vector2(0, 0), 0f).setOnComplete(() =>
        {
            LeanTween.scale(gameObject.GetComponent<RectTransform>(), new Vector2(1, 1), 0.5f).setEase(LeanTweenType.easeInQuint).setOnComplete(() =>
            {
                LeanTween.cancel(gameObject);
            });
        });

        safeZone.SetActive(true);
    }

    public void AppZoomOut()
    {
        LeanTween.scale(gameObject.GetComponent<RectTransform>(), new Vector2(0, 0), 0.5f).setEase(LeanTweenType.easeOutQuint).setOnComplete(() =>
        {
            LeanTween.cancel(gameObject);
            safeZone.SetActive(false);
        });
    }

    public void AppSwitchIn()
    {
        LeanTween.scale(gameObject, new Vector3(0.25f, 0.25f, 0.25f), 0f)
            .setOnComplete(() =>
            {
                LeanTween.scale(gameObject, new Vector3(0.75f, 0.75f, 0.75f), 0.25f)
                    .setOnComplete(() =>
                    {
                        LeanTween.scale(gameObject, new Vector3(1f, 1f, 1f), 0.166f);
                    });
            });

        LeanTween.moveLocal(gameObject, new Vector3(0.25f, 0.25f, 0.25f), 0f).setOnComplete(() =>
        {
            LeanTween.moveLocalX(gameObject, 250f, 0.25f)
            .setOnComplete(() =>
            {
                LeanTween.moveLocalX(gameObject, 0f, 0.25f);
            });
        });
    }

    public void AppSwitchOut()
    {
        LeanTween.scale(gameObject, new Vector3(1f, 1f, 1f), 0f)
            .setOnComplete(() =>
            {
                LeanTween.scale(gameObject, new Vector3(0.75f, 0.75f, 0.75f), 0.25f)
                    .setOnComplete(() =>
                    {
                        LeanTween.scale(gameObject, new Vector3(0.25f, 0.25f, 0.25f), 0.166f);
                    });
            });

        LeanTween.moveLocalX(gameObject, -250f, 0.25f)
            .setOnComplete(() =>
            {
                LeanTween.moveLocalX(gameObject, 0f, 0.25f);
            });
    }
}
