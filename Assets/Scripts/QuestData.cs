using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum QuestType
{
    Kill,
    Collect,
}

[CreateAssetMenu(fileName = "Quest", menuName = "QuestData")]
public class QuestData : ScriptableObject
{
    public int questId;
    public string questName;
    public QuestType questType;
    public int targetId;
    public int reqAmount;
    public float rewardExp;
    public int rewardGold;
    public List<string> conversation;
}
