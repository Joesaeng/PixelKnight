using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionUI : MenuUI
{
    bool firstOpen = false;
    public GameObject notSetKey;
    public GameObject exitCheck;
    public Text notSetKeyText;
    public Text descText;
    public override void KeyInputAtiveMenu(){}
    public override void ActiveMenu()
    {
        activeMenu = !activeMenu;
        menuPanel.SetActive(activeMenu);
        notSetKey.SetActive(false);
        exitCheck.SetActive(false);
        SetDescText("");
        if (menuPanel.activeSelf) Time.timeScale = 0;
        else Time.timeScale = 1;
        if (!firstOpen)
        {
            KeyManager.Instance.OnDescText += SetDescText;
            KeyManager.Instance.InitKeyChangeButtons(this);
            firstOpen = true;
        }
    }
    public void ExitOptionUI()
    {
        notSetKeyText.text = string.Empty;
        KeyManager.Instance.SetChangeKey(KeyAction.KeyCount);
        for(int i = 0; i < (int)KeyAction.KeyCount; ++i)
        {
            if (KeySetting.keys[(KeyAction)i] == KeyCode.None)
            {
                notSetKeyText.text += "[" + (KeyAction)i + "] ";
            }
        }
        if (notSetKeyText.text != string.Empty)
        {
            notSetKey.SetActive(true);
        }
        else ActiveMenu();
    }
    public void ExitNotSetKey()
    {
        notSetKey.SetActive(false);
    }
    void SetDescText(string descText)
    {
        this.descText.text = descText;
    }
    public void GameExitButton()
    {
        exitCheck.SetActive(true);
    }
    public void GameExit()
    {
        GameManager.Instance.GameExit();
    }
    public void SaveButton()
    {
        SaveDataManager.Instance.Save();
    }
}
