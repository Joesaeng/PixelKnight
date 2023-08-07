using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InputSystem : Singleton<InputSystem>
{
    private float horizontalInput;
    private float verticalInput;

    public Action OnInventoryMenu;
    public Action OnStatusMenu;
    public Action OnSkillMenu;
    private void Update()
    {
        SetHorizontalInput();
        SetVerticalInput();
        GetSkillKeyDown();
        GetMenuKeyDown();
    }
    private void SetHorizontalInput()
    {
        float leftInput = Input.GetKey(KeySetting.keys[KeyAction.Left]) ? -1f : 0f;
        float rightInput = Input.GetKey(KeySetting.keys[KeyAction.Right]) ? 1f : 0f;
        horizontalInput = leftInput + rightInput;

        horizontalInput = Mathf.Clamp(horizontalInput, -1f, 1f);
    }
    private void SetVerticalInput()
    {
        float downInput = Input.GetKey(KeySetting.keys[KeyAction.Down]) ? -1f : 0f;
        float upInput = Input.GetKey(KeySetting.keys[KeyAction.Up]) ? 1f : 0f;
        verticalInput = downInput + upInput;

        verticalInput = Mathf.Clamp(verticalInput, -1f, 1f);
    }
    public float GetHorizontalInput()
    {
        return horizontalInput;
    }
    public float GetVerticalInput()
    {
        return verticalInput;
    }
    public KeyAction GetSkillKeyDown()
    {
        KeyAction key = KeyAction.KeyCount;
        if (Input.GetKeyDown(KeySetting.keys[KeyAction.Skill_1])) key = KeyAction.Skill_1;
        else if (Input.GetKeyDown(KeySetting.keys[KeyAction.Skill_2])) key = KeyAction.Skill_2;
        else if (Input.GetKeyDown(KeySetting.keys[KeyAction.Skill_3])) key = KeyAction.Skill_3;
        else if (Input.GetKeyDown(KeySetting.keys[KeyAction.Skill_4])) key = KeyAction.Skill_4;


        return key;
    }
    private void GetMenuKeyDown()
    {
        if (Input.GetKeyDown(KeySetting.keys[KeyAction.Inventory])) OnInventoryMenu?.Invoke();
        else if (Input.GetKeyDown(KeySetting.keys[KeyAction.Status])) OnStatusMenu?.Invoke();
        else if (Input.GetKeyDown(KeySetting.keys[KeyAction.Skill])) OnSkillMenu?.Invoke();
    }
}
