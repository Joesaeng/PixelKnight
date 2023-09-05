using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_OffAllWindowMenu : MonoBehaviour
{
    // ESC Ű�� �Է����� �� �����ִ� �ɼ�â�� ������ ��� ������ â�� �ݴ� ��ũ��Ʈ�Դϴ�.
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
