using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_QuestNav : MonoBehaviour
{
    [SerializeField]
    GameObject questNav;

    [SerializeField]
    TextMeshProUGUI questName;

    [SerializeField]
    TextMeshProUGUI questCount;

    QuestManager questmgr;

    private void Start()
    {
        questmgr = QuestManager.Instance;
        questNav.SetActive(false);
        if(questmgr.curQuestData != null)
        {
            questNav.SetActive(true);
            QuestNavUpdate();
        }
        questmgr.onQuestUpdate += QuestNavUpdate;
    }
    private void OnDisable()
    {
        questmgr.onQuestUpdate -= QuestNavUpdate;
    }
    void QuestNavUpdate()
    {
        if(questmgr.curQuestData == null)
        {
            questNav.SetActive(false);
        }
        else
        {
            questNav.SetActive(true);
            questName.text = questmgr.curQuestData.questName;
            questCount.text = "( " + questmgr.curQuestCount + " / " + questmgr.curQuestData.reqAmount + " )";
        }
        
    }

}
