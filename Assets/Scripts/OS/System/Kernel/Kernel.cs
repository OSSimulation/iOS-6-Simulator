using OS6.IO.Audio;
using OS6.IO.Displays;
using OS6.Notifications;
using OS6.Power;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace OS6.Kernel
{
    public class Kernel : MonoBehaviour
    {
        private static Kernel instance;

        [SerializeField] private Slider bar;
        Coroutine loadSpringboard;
        AsyncOperation operation;

        private void Awake()
        {
            Debug.Log("[OS6/BOOT]: Starting iOS 6 Simulator...");

            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(gameObject);

            Application.targetFrameRate = 60;

            Intitialise();
        }

        private void Start()
        {
            Boot();
        }

        private void Intitialise()
        {
            Debug.Log("[OS6/BOOT]: Registering System Services...");

            System_Services.RegisterService(GetComponent<Audio_Manager>());
            System_Services.RegisterService(GetComponent<Display_Manager>());
            System_Services.RegisterService(GetComponent<Notification_Manager>());
            System_Services.RegisterService(GetComponent<Power_Manager>());

            System_Services.RegisterService(GetComponent<MobileGestalt>());

            Debug.Log("[OS6/BOOT]: System Service Registration Complete");
        }

        private void Boot()
        {
            Debug.Log("[OS6/BOOT]: Starting SpringBoard...");

            loadSpringboard = StartCoroutine(LoadSpringBoard());
        }

        private IEnumerator LoadSpringBoard()
        {
            bar.value = 0;

            operation = SceneManager.LoadSceneAsync("Springboard");
            operation.allowSceneActivation = false;

            float displayProgress = 0;

            yield return new WaitForSeconds(1f);

            while (!operation.isDone)
            {
                float targetProgress = Mathf.Clamp01(operation.progress / 0.9f);
                displayProgress = Mathf.MoveTowards(displayProgress, targetProgress, Time.deltaTime * 0.25f);
                bar.value = displayProgress;

                if (displayProgress >= 1f && operation.progress >= 0.9f)
                {
                    Debug.Log("[OS6/BOOT]: SpringBoard Started Successfully");
                    operation.allowSceneActivation = true;
                }

                yield return null;
            }
        }
    }
}
