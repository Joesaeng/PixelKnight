using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingSceneController : MonoBehaviour
{
    static string nextScene;
    static bool playScene;
    [SerializeField]
    Image progressBar;
    public static void LoadScene(string sceneName, bool _playScene)
    {
        nextScene = sceneName;
        playScene = _playScene;
        SceneManager.LoadScene("LoadingScene");
    }
    private void Start()
    {
        StartCoroutine(LoadSceneProcess());
    }
    IEnumerator LoadSceneProcess()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;
        
        
        float time = 0f;
        while(!op.isDone)
        {
            yield return null;

            if(op.progress >= 0.5f && op.progress < 0.9f)
            {
                progressBar.fillAmount = op.progress;
            }
            else
            {
                time += Time.unscaledDeltaTime;
                progressBar.fillAmount = Mathf.Lerp(0.9f, 1f, time);
                if(progressBar.fillAmount >= 1f)
                {
                    op.allowSceneActivation = true;
                    if (playScene)
                        SceneManager.LoadScene("UI_SceneAdditiveOnly", LoadSceneMode.Additive);
                    yield break;
                }
            }
        }
    }
}
