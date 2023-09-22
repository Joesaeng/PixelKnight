using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingSceneController : MonoBehaviour
{
    static string nextScene;
    static bool playScene;
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
        while (!op.isDone)
        {
            yield return null;

            if (playScene)
            {
                SceneManager.LoadScene("UI_SceneAdditiveOnly", LoadSceneMode.Additive);
            }
            op.allowSceneActivation = true;
            yield break;
        }
    }
}
