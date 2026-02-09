using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class LoadingController : MonoBehaviour
{
   // ui loading
   [SerializeField] private Slider progressBar;
    [SerializeField] private TMP_Text progressText;

    // cấu hình
    [SerializeField] private float minLoadingTime = 1.5f;

    // dùng chung cho toàn game;
    private static string targetScene;

    public static void LoadScene(string sceneName)
    {
        targetScene = sceneName;
        SceneManager.LoadScene("Loading");
    }
    void Start()
    {
        if(string.IsNullOrEmpty(targetScene))
        {
            targetScene = "Startgame";
        }

        StartCoroutine(LoadTargetScene());
    }

    private IEnumerator LoadTargetScene()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(targetScene);
        operation.allowSceneActivation = false;

        float timer = 0f;
        while(operation.progress < 0.9f || timer < minLoadingTime)
        {
            timer += Time.deltaTime;
            float realProgress = Mathf.Clamp01(operation.progress / 0.9f);
            float fakeProgress = Mathf.Clamp01(timer / minLoadingTime);
            float displayedProgress = Mathf.Min(realProgress, fakeProgress);   

            if(progressBar != null)
            {
                progressBar.value = displayedProgress;
            }
            if(progressText != null)
            {
                progressText.text =$"{Mathf.RoundToInt(displayedProgress * 100)}%";
            }

            yield return null;
        }
        if(progressBar != null)
        {
            progressBar.value = 1f;
        }

        if(progressText != null)
        {
            progressText.text = "100%";
        }

        yield return new WaitForSeconds(0.2f);
        operation.allowSceneActivation = true;
    }   
}
