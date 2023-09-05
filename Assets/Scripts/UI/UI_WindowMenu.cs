using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UI_WindowMenu : MonoBehaviour
{
    // 인벤토리,장비창 등 On/Off 되는 윈도우 메뉴들의 부모이자 추상클래스입니다.
    public GameObject menuPanel;
    public bool activeMenu = false;

    private void Start()
    {
        menuPanel.SetActive(false);
    }
    public abstract void ActiveMenu();
    public abstract void KeyInputAtiveMenu();
}
