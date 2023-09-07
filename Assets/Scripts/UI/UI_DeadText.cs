using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_DeadText : MonoBehaviour
{
    public static UI_DeadText instance;
    float alphatime = 2f;
    float durationtime = 5f;
    [SerializeField]
    GameObject deadText;
    [SerializeField]
    Text text;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        text.color = new Color(197 / 255f, 14 / 255f, 14 / 255f, 0f);
        deadText.SetActive(false);
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            ActiveDeadText();
        }
    }
    public void ActiveDeadText()
    {
        deadText.SetActive(true);
        StartCoroutine(CoDeadText());
    }
    IEnumerator CoDeadText()
    {
        float curtime = 0f;
        yield return null;
        while (curtime < durationtime)
        {
            curtime += Time.deltaTime;
            if (curtime < alphatime)
                text.color = new Color(197 / 255f, 14 / 255f, 14 / 255f, curtime / alphatime);
            yield return null;
        }
        text.color = new Color(197 / 255f, 14 / 255f, 14 / 255f, 0f);
        deadText.SetActive(false);
        GameManager.Instance.DeadTextAfterLoading();
    }
}
