using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class KeyChangeButton : MonoBehaviour, IPointerUpHandler
{
    public KeyAction keyAction;
    public KeyCode keyCode;
    public Text text;
    
    public KeyAction GetKeyAction()
    {
        return keyAction;
    }
    public void SetKeyCodeText(KeyCode keyCode)
    {
        this.keyCode = keyCode;
    }
    public void Init()
    {
        UpdateText();
        KeyManager.Instance.OnUpdateKeyChangeButtonText += UpdateText;
    }
    public void UpdateText()
    {
        text.text = "<< " + keyAction.ToString() + " >> == << " + KeySetting.keys[keyAction].ToString() + " >>";
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        KeyManager.Instance.SetChangeKey(keyAction);
    }
}
