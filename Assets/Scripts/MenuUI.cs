using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MenuUI : MonoBehaviour
{
    public GameObject menuPanel;
    public bool activeMenu = false;

    public abstract void ActiveMenu();
}
