using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CenterPopupText : MonoBehaviour
{
    public Text popupText;
    float enableTime = 3f;
    float curTime = 0f;
    private void Awake()
    {
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
