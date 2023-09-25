using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FakeLoading : MonoBehaviour
{
    public static FakeLoading instance;
    [SerializeField]
    GameObject fakeLoadingUI;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        fakeLoadingUI.SetActive(false);
    }
    public void StartFakeLoding(float time, string nextSceneName,bool playScene)
    {
        SoundManager.Instance.bgm.Stop();
        fakeLoadingUI.SetActive(true);
        if (PoolManager.Instance != null)
            PoolManager.Instance.ReturnAllObj();
        StartCoroutine(CoFakeLoading(time, nextSceneName,playScene));
    }
    IEnumerator CoFakeLoading(float time, string nextSceneName,bool playScene)
    {
        float curtime = 0f;
        yield return null;

        while(curtime < time)
        {
            curtime += Time.deltaTime;
            yield return null;
        }
        LoadingSceneController.LoadScene(nextSceneName, playScene);
    }
}
