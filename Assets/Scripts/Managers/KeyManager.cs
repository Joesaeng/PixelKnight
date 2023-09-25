using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum KeyAction
{
    Up, Down, Left, Right,
    MeleeAttack,
    Jump,
    UseHpPotion,
    Interaction,
    Skill_1,
    Skill_2,
    Skill_3,
    Skill_4,
    InventoryMenu,
    StatusMenu,
    SkillMenu,
    KeyCount
}

public static class KeySetting 
{ 
    public static Dictionary<KeyAction, KeyCode> keys = new Dictionary<KeyAction, KeyCode>();
}
public class KeyManager : Singleton<KeyManager>
{
    UI_KeyChangeButton[] keyChangeButtons;
    public Action OnUpdateKeyChangeButtonText;
    public Action<string> OnDescText;
    KeyAction changeKey = KeyAction.KeyCount;
    KeyCode[] defaultKeys = new KeyCode[]
    {
        KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.LeftArrow,KeyCode.RightArrow,
        KeyCode.Z,KeyCode.C,KeyCode.R,KeyCode.G,
        KeyCode.Q,KeyCode.W,KeyCode.E,KeyCode.R,
        KeyCode.I,KeyCode.P,KeyCode.K
    };
    private void Start()
    {
        for(int i = 0; i < (int)KeyAction.KeyCount; ++i)
        {
            KeySetting.keys.Add((KeyAction)i, defaultKeys[i]);
        }
    }
    public void InitKeyChangeButtons(UI_Option ui)
    {
        keyChangeButtons = ui.GetComponentsInChildren<UI_KeyChangeButton>();
        KeyAction i = 0;
        foreach (UI_KeyChangeButton t in keyChangeButtons)
        {
            t.SetKeyAction(i);
            t.SetKeyCodeText(KeySetting.keys[t.GetKeyAction()]);
            t.Init();
            i++;
        }
    }
    private void OnGUI()
    {
        Event keyEvent = Event.current;
        if(keyEvent.isKey && changeKey != KeyAction.KeyCount)
        {
            KeyChange(changeKey, keyEvent.keyCode);
            changeKey = KeyAction.KeyCount;
        }
    }
    public void SetChangeKey(KeyAction keyAction)
    {
        if(keyAction != KeyAction.KeyCount)
            OnDescText?.Invoke("< " + keyAction.ToString() + " > \n변경할 키를 입력해 주세요");
        changeKey = keyAction;
    }
    public void KeyChange(KeyAction keyAction,KeyCode keyCode)
    {
        if(keyCode == KeyCode.Escape)
        {
            KeySetting.keys[keyAction] = KeyCode.None;
            OnUpdateKeyChangeButtonText?.Invoke();
            OnDescText?.Invoke("");
            return;
        }
        for(int i = 0; i < KeySetting.keys.Count; ++i)
        {
            if (KeySetting.keys[(KeyAction)i] == keyCode)
            {
                OnDescText?.Invoke("다른 키에 설정되어 있는 키를 \n입력하셨습니다.");
                return;
            }
        }
        KeySetting.keys[keyAction] = keyCode;
        OnDescText?.Invoke("");
        OnUpdateKeyChangeButtonText?.Invoke();
    }
}
