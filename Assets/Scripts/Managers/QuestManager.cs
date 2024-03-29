using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class QuestManager : Singleton<QuestManager>
{
    public List<QuestData> questDatas;
    public QuestData curQuestData;
    public int curQuestIndex;
    public int curQuestCount;
    public Action onQuestUpdate;
    private void Start()
    {
        curQuestData = null;
        curQuestCount = 0;
        curQuestIndex = -1;
    }
    public void SaveQuest(ref int index, ref int count)
    {
        index = curQuestIndex;
        count = curQuestCount;
    }
    public void LoadQuest()
    {
        SaveData loadData = SaveDataManager.Instance.saveData;
        curQuestIndex = loadData.curQuestIndex;
        if (curQuestIndex < 0)
        {
            curQuestData = null;
            return;
        }
        curQuestData = questDatas[curQuestIndex];
        curQuestCount = loadData.curQuestCount;
        //onQuestUpdate.Invoke();
    }
    public List<string> GetMessages(int index)
    {
        return questDatas[index].conversation;
    }
    public void SetQuest(int index)
    {
        curQuestIndex = index;
        curQuestData = questDatas[index];
        onQuestUpdate.Invoke();
    }
    public void PlayerGetReward()
    {
        GameManager.Instance.ModifyGold(curQuestData.rewardGold, true);
        GameManager.Instance.playerStatus.ModifyExp(curQuestData.rewardExp, true);
        curQuestData = null;
        curQuestCount = 0;
    }
    public void KillEnemy(int enemyId)
    {
        if(curQuestData.questType == QuestType.Kill && curQuestData.targetId == enemyId)
        {
            curQuestCount++;
            onQuestUpdate.Invoke();
        }
    }
}
