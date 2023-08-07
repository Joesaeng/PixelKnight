using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum KeyAction
{
    Up, Down, Left, Right,
    MeleeAttack,
    Jump,
    Skill_1,
    Skill_2,
    Skill_3,
    Skill_4,
    Inventory,
    Status,
    Skill,
    KeyCount
}

public static class KeySetting 
{ 
    public static Dictionary<KeyAction, KeyCode> keys = new Dictionary<KeyAction, KeyCode>(); 
}
public class KeyManager : Singleton<KeyManager>
{
    KeyCode[] defaultKeys = new KeyCode[]
    {
        KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.LeftArrow,KeyCode.RightArrow,
        KeyCode.Z,KeyCode.C,
        KeyCode.Q,KeyCode.W,KeyCode.E,KeyCode.R,
        KeyCode.I,KeyCode.P,KeyCode.K
    };
    private void Awake()
    {
        for(int i = 0; i < (int)KeyAction.KeyCount; ++i)
        {
            KeySetting.keys.Add((KeyAction)i, defaultKeys[i]);
        }
        
    }
}
