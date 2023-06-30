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

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            activeStatusUI = !activeStatusUI;
            statusPanel.SetActive(activeStatusUI);
            if(!firstOpen)
            {
                statusValueUI.InitStatusUI();
                firstOpen = true;
            }
        }
    }
}
