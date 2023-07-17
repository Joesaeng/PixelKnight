using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusUI : MonoBehaviour
{
    bool activeStatusUI = false;
    bool firstOpen = false;
    public GameObject statusPanel;
    StatusValueUI statusValueUI;
    void Start()
    {
        statusPanel.SetActive(activeStatusUI);
        statusValueUI = statusPanel.transform.GetChild(1).transform.GetChild(2).GetComponent<StatusValueUI>();
    }
    public void ActiveStatusUI()
    {
        activeStatusUI = !activeStatusUI;
        statusPanel.SetActive(activeStatusUI);
        if (!firstOpen)
        {
            statusValueUI.InitStatusUI();
            firstOpen = true;
        }
    }
}
