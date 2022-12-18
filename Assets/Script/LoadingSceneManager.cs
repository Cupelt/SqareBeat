using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneManager : MonoBehaviour
{
    public static string nextScene;
    [SerializeField] Slider progressBar;
    [SerializeField] Text progressText;
    [SerializeField] Image fadeInOut;

    private void Start()
    {
        StartCoroutine(LoadScene());
    }

    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("LoadingScene");
    }

    IEnumerator LoadScene()
    {
        yield return null;
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;
        float timer = 0.0f;
        while (!op.isDone)
        {
            yield return null;
            timer += Time.deltaTime;
            if (op.progress < 0.9f)
            {
                progressBar.value = Mathf.Lerp(progressBar.value, op.progress, timer);
                progressText.text = Mathf.RoundToInt(progressBar.value * 100f).ToString() + "%";
                if (progressBar.value >= op.progress)
                {
                    timer = 0f;
                }
            }
            else
            {
                progressBar.value = Mathf.Lerp(progressBar.value, 1f, timer);
                progressText.text = Mathf.RoundToInt(progressBar.value * 100f).ToString() + "%";
                if (progressBar.value == 1.0f)
                {
                    float alpha = 0;
                    while (alpha < 1)
                    {
                        yield return null;
                        alpha += Time.deltaTime * 2f;
                        fadeInOut.color = new Color(0, 0, 0, alpha);
                    }

                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }
}
