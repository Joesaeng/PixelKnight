using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusUI : MenuUI
{
    bool firstOpen = false;
    StatusValueUI statusValueUI;
    void Start()
    {
        menuPanel.SetActive(activeMenu);
        statusValueUI = menuPanel.transform.GetChild(1).transform.GetChild(2).GetComponent<StatusValueUI>();
        InputSystem.Instance.OnStatusMenu += KeyInputAtiveMenu;
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
}
