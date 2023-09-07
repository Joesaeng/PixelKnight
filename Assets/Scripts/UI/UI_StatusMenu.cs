using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_StatusMenu : UI_WindowMenu
{
    bool firstOpen = false;
    UI_StatusValueText statusValueUI;
    void Start()
    {
        menuPanel.SetActive(activeMenu);
        statusValueUI = menuPanel.transform.GetChild(1).transform.GetChild(2).GetComponent<UI_StatusValueText>();
        InputSystem.Instance.OnStatusMenu += KeyInputAtiveMenu;
    }
    private void OnDestroy()
    {
        InputSystem.Instance.OnStatusMenu -= KeyInputAtiveMenu;
    }
    public override void KeyInputAtiveMenu()
    {
        ActiveMenu();
    }
    public override void ActiveMenu()
    {
        activeMenu = !activeMenu;
        menuPanel.SetActive(activeMenu);
        if (!firstOpen)
        {
            statusValueUI.InitStatusUI();
            firstOpen = true;
        }
    }
    public void ResetStatus()
    {
        statusValueUI.GetPlayerStatus().ResetStatus();
    }
}
