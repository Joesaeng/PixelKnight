using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestNpc : NPC
{
    QuestManager questmgr;
    private void Start()
    {
        questmgr = QuestManager.Instance;
    }
    protected override void Interaction()
    {
        if(questmgr.curQuestData == null)
        {
            if (questmgr.curQuestIndex >= -1 && questmgr.curQuestIndex < questmgr.questDatas.Count - 1)
            {
                UI_MessageBox.instance.SetMessages(questmgr.GetMessages(questmgr.curQuestIndex + 1));
                questmgr.SetQuest(questmgr.curQuestIndex + 1);
            }  
        }
        if (questmgr.curQuestCount >= questmgr.curQuestData.reqAmount)
        {
            UI_MessageBox.instance.SetText("감사합니다 기사님!");
            questmgr.PlayerGetReward();
        }
        
    }
}
