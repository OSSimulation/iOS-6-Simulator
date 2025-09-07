using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Boot : MonoBehaviour
{
    [SerializeField] private Slider bar;

    private void Start()
    {
        StartCoroutine(LoadMainScene());
    }

    IEnumerator LoadMainScene()
    {
        bar.value = 0;

        AsyncOperation operation = SceneManager.LoadSceneAsync(1);
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
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
