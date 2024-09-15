using UnityEngine;

public class Mask_Move : MonoBehaviour
{
    [SerializeField] private RectTransform mask;
    [SerializeField] private float moveSpeed;

    [SerializeField] private GameObject handle;
    [SerializeField] private CanvasGroup alpha;

    float startPosX = -240f;
    float endPosX = 240f;

    private void Start()
    {
        mask.anchoredPosition = new Vector2(startPosX, mask.anchoredPosition.y);
    }

    private void Update()
    {
        mask.anchoredPosition += Vector2.right * moveSpeed * Time.deltaTime;

        if (mask.anchoredPosition.x >= endPosX)
        {
            mask.anchoredPosition = new Vector2(startPosX, mask.anchoredPosition.y);
        }

        AlphaChanger();
    }

    void AlphaChanger()
    {
        float xPos = handle.transform.localPosition.x;
        float clampedX = Mathf.Clamp(xPos, -170f, -15f);
        float normalisedX = (-15f - clampedX) / 155f;

        alpha.alpha = normalisedX;
    }
}
