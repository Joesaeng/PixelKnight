using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_OffAllWindowMenu : MonoBehaviour
{
    // ESC 키를 입력했을 때 열려있는 옵션창을 제외한 모든 윈도우 창을 닫는 스크립트입니다.
    UI_WindowMenu[] menuUIList;
    UI_WindowMenu optionMenu;
    private bool isOnMenu;
    private void Awake()
    {
        menuUIList = GetComponents<UI_WindowMenu>();
        isOnMenu = false;
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            for(int i = 0; i < menuUIList.Length;++i)
            {
                if (menuUIList[i].menuPanel.CompareTag("OptionUI"))
                {
                    optionMenu = menuUIList[i];
                    continue;
                }
                if (menuUIList[i].activeMenu)
                {
                    menuUIList[i].ActiveMenu();
                    isOnMenu = true;
                }
            }
            if(!isOnMenu)
            {
                if (!optionMenu.activeMenu)
                    optionMenu.ActiveMenu();
            }
            isOnMenu = false;
        }
    }
}
