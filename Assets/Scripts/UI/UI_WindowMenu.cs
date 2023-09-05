using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UI_WindowMenu : MonoBehaviour
{
    // �κ��丮,���â �� On/Off �Ǵ� ������ �޴����� �θ����� �߻�Ŭ�����Դϴ�.
    public GameObject menuPanel;
    public bool activeMenu = false;

    private void Start()
    {
        menuPanel.SetActive(false);
    }
    public abstract void ActiveMenu();
    public abstract void KeyInputAtiveMenu();
}
