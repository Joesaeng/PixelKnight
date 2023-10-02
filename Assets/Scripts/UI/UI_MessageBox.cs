using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_MessageBox : MonoBehaviour
{
    public static UI_MessageBox instance;
    [SerializeField]
    GameObject messageBox;
    [SerializeField]
    Text boxText;
    [SerializeField]
    Text pressInteractionKeyText;
    public List<string> messages;

    int messageIndex;

    float typingSpeed = 0.05f;
    private string currentText = "";
    private string targetText = "";
    private bool isTyping = false;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        messageBox.SetActive(false);
    }
    public void SetMessages(List<string> _messages)
    {
        messages = _messages;
        messageIndex = 0;
        SetText(messages[messageIndex]);
    }
    public void SetText(string text)
    {
        messageBox.SetActive(true);
        pressInteractionKeyText.text = "PREES " + KeySetting.keys[KeyAction.Interaction].ToString() + " KEY";
        if (isTyping) return;
        targetText = text;
        StartCoroutine(CoTypeText());
    }
    private IEnumerator CoTypeText()
    {
        isTyping = true;
        int length = targetText.Length;
        yield return new WaitForEndOfFrame();

        for (int i = 0; i < length; i++)
        {
            if(Input.GetKeyDown(KeySetting.keys[KeyAction.Interaction]))
            {
                boxText.text = targetText;
                break;
            }
            currentText = targetText.Substring(0, i + 1);
            boxText.text = currentText;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }
    public void Update()
    {
        if(messageBox.activeSelf)
        {
            if(isTyping == false && Input.GetKeyDown(KeySetting.keys[KeyAction.Interaction]))
            {
                if(messages != null && messageIndex < messages.Count -1)
                {
                    SetText(messages[++messageIndex]);
                }
                else
                {
                    boxText.text = string.Empty;
                    messageBox.SetActive(false);
                    messageIndex = 0;
                    messages = null;
                }
                
            }
        }
    }
}
