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

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        messageBox.SetActive(false);
    }
    public void SetText(string text)
    {
        messageBox.SetActive(true);
        boxText.text = text;
    }
    public void Update()
    {
        if(messageBox.activeSelf)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                boxText.text = string.Empty;
                messageBox.SetActive(false);
            }
        }
    }
}
