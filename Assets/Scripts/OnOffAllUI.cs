using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnOffAllUI : MonoBehaviour
{
    MenuUI[] menuUIList;
    
    private void Awake()
    {
        menuUIList = GetComponents<MenuUI>();
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            for(int i = 0; i < menuUIList.Length;++i)
            {
                if (menuUIList[i].activeMenu)
                    menuUIList[i].ActiveMenu();
            }
        }
    }
}
