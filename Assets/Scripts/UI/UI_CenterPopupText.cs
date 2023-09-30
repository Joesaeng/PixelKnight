using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_CenterPopupText : MonoBehaviour
{
    // 경고, 알림 등을 화면 정중앙에 출력하는 스크립트 입니다.
    public static UI_CenterPopupText instance;
    public Text popupText;
    public float enableTime = 3f; // 텍스트가 생성 후 존재하는 시간입니다.
    public float curTime = 0f;
    private void Awake()
    {
        instance = this;
        popupText.enabled = false;
    }
    public void SetPopupText(string text)
    {
        if (popupText.enabled == true && curTime < 1f) return;
        popupText.text = text;
        popupText.enabled = true;
        curTime = 0f;
    }
    private void Update()
    {
        if (popupText.enabled)
        {
            popupText.color = new Color(popupText.color.r, popupText.color.g, popupText.color.b, 1f - (curTime / enableTime));
            curTime += Time.deltaTime;
            if (curTime >= enableTime)
            {
                popupText.enabled = false;
                curTime = 0f;
            }
        }
    }
}
