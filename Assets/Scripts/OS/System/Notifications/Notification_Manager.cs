using OS6.Events;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace OS6.Notifications
{
    public class Notification_Manager : MonoBehaviour
    {
        [SerializeField] private Notification_Alert alerts;

        private void OnEnable()
        {
            alerts.Initialise();
        }

        private void OnDisable()
        {
            alerts.Terminate();
        }

        public void CreateAlertSingle(string title, string body, string primaryText)
        {
            alerts.CreateAlertSingle(title, body, primaryText);
        }

        public void CreateAlertDouble(string title, string body, string primaryText, string secondaryText)
        {
            alerts.CreateAlertDouble(title, body, primaryText, secondaryText);
        }

        [System.Serializable]
        private class Notification_Alert
        {
            [SerializeField] private Transform alertBox;
            [SerializeField] private CanvasGroup spotlight;
            [SerializeField] private GameObject alertPrefab;

            private readonly Queue<System.Action> alertQueue = new();
            private Alert currentAlert;

            public void Initialise()
            {
                GlobalEvents.Subscribe("NOTIFICATION_ALERT_CLOSED", AlertClosed);
            }

            public void Terminate()
            {
                GlobalEvents.Unsubscribe("NOTIFICATION_ALERT_CLOSED", AlertClosed);
            }

            public void CreateAlertSingle(string title, string body, string primaryText)
            {
                LeanTween.alphaCanvas(spotlight, 1f, 0.2f);
                alertBox.GetComponent<Image>().raycastTarget = true;

                alertQueue.Enqueue(() =>
                {
                    GameObject newAlert = Instantiate(alertPrefab, alertBox);
                    Alert alert = newAlert.AddComponent<Alert>();
                    alert.CreateAlertSingle(title, body, primaryText);
                    currentAlert = alert;
                });

                ShowNextAlert();
            }

            public void CreateAlertDouble(string title, string body, string primaryText, string secondaryText)
            {
                LeanTween.alphaCanvas(spotlight, 1f, 0.2f);
                alertBox.GetComponent<Image>().raycastTarget = true;

                alertQueue.Enqueue(() =>
                {
                    GameObject newAlert = Instantiate(alertPrefab, alertBox);
                    Alert alert = newAlert.AddComponent<Alert>();
                    alert.CreateAlertDouble(title, body, primaryText, secondaryText);
                    currentAlert = alert;
                });

                ShowNextAlert();
            }

            private void AlertClosed()
            {
                LeanTween.alphaCanvas(spotlight, 0f, 0.2f);
                alertBox.GetComponent<Image>().raycastTarget = false;

                currentAlert = null;
                ShowNextAlert();
            }

            private void ShowNextAlert()
            {
                if (currentAlert != null) return;
                if (alertQueue.Count == 0) return;

                LeanTween.alphaCanvas(spotlight, 1f, 0.2f);
                alertQueue.Dequeue().Invoke();
            }
        }

        private class Alert : MonoBehaviour
        {
            private TMP_Text title;
            private TMP_Text body;

            [Space(10)]
            private Button primary;
            private TMP_Text primaryText;
            private Button secondary;
            private TMP_Text secondaryText;

            [Space(10)]
            private CanvasGroup alpha;

            private void Awake()
            {
                transform.localScale = Vector3.zero;

                title = transform.Find("Title").GetComponent<TMP_Text>();
                body = transform.Find("Body").GetComponent<TMP_Text>();

                Transform buttonHolder = transform.Find("Buttons").GetComponent<Transform>();

                primary = buttonHolder.Find("Primary").GetComponent<Button>();
                secondary = buttonHolder.Find("Secondary").GetComponent<Button>();

                primaryText = primary.transform.Find("Primary Text").GetComponent<TMP_Text>();
                secondaryText = secondary.transform.Find("Secondary Text").GetComponent<TMP_Text>();

                alpha = GetComponent<CanvasGroup>();
            }

            private void Start()
            {
                LeanTween.scale(gameObject, new Vector3(1f, 1f, 1f), 0.2f).setEase(LeanTweenType.easeOutBack);
            }

            public void CreateAlertSingle(string title, string body, string primaryText)
            {
                this.title.text = title;
                this.body.text = body;
                this.primaryText.text = primaryText;

                secondary.gameObject.SetActive(false);

                primary.onClick.RemoveAllListeners();
                primary.onClick.AddListener(DestroyAlert);
            }

            public void CreateAlertDouble(string title, string body, string primaryText, string secondaryText)
            {
                this.title.text = title;
                this.body.text = body;
                this.primaryText.text = primaryText;

                this.secondaryText.text = secondaryText;

                primary.onClick.RemoveAllListeners();
                secondary.onClick.RemoveAllListeners();

                primary.onClick.AddListener(DestroyAlert);
                secondary.onClick.AddListener(DestroyAlert);
            }

            private void DestroyAlert()
            {
                GlobalEvents.PublishEventMessage("NOTIFICATION_ALERT_CLOSED");
                LeanTween.alphaCanvas(alpha, 0f, 0.2f).setOnComplete(() =>
                {
                    Destroy(gameObject);
                });
            }
        }
    }
}