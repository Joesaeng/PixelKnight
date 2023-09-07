using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class SaveData
{
    public string name;
    public string curSceneName;
    public int level;
    public int charId;
    public int hour = 0;
    public int minute = 0;
    public float second = 0f;
    public float curExp;
    public float expReq = 50f;
    public int remainingPoint;
    public int addedPoint;
    public int curGold;
    public Vector2 playerCurPos = new();

    public int AddedVit;
    public int AddedEnd;
    public int AddedStr;
    public int AddedDex;
    public int AddedLuk;

    public float curHp;
    public List<SkillName> skills = new();
    public List<Equip> curEquips = new();
    public int inventorySlotCount;
    public List<Consumable> inventoryConsumables = new();
    public List<Equip> inventoryEquips = new();
    public List<SaveCountItem> countItem = new();
}
[System.Serializable]
public class SaveCountItem
{
    public Item item;
    public int count;
    public SaveCountItem(Item item, int count)
    {
        this.item = item;
        this.count = count;
    }

}
public class SaveDataManager : Singleton<SaveDataManager>
{
    public SaveData saveData = new();
    public SaveData tempSaveData = new();
    public string path;
    public int nowSlot;

    private void Awake()
    {
        path = Application.persistentDataPath + "/save";
    }
    public void Save()
    {
        saveData = tempSaveData;
    }
    public void SaveTempData()
    {
        if (GameManager.Instance.player != null)
        {
            Player savePlayer = GameManager.Instance.player;
            tempSaveData = savePlayer.playerStatus.SaveStatus();
            tempSaveData.curGold = GameManager.Instance.curGold;
            tempSaveData.playerCurPos = GameManager.Instance.player.transform.position;
            tempSaveData.skills = savePlayer.skills.GetEnableSkills();
            tempSaveData.curEquips.AddRange(savePlayer.playerStatus.equipment.GetCurEquip());
            List<Item> items = Inventory.Instance.GetItems();
            SaveInventoryItems(items);

            tempSaveData.inventorySlotCount = Inventory.Instance.SlotCnt;
        }
        tempSaveData.hour = GameManager.Instance.GetPlayTime().hour;
        tempSaveData.minute = GameManager.Instance.GetPlayTime().minute;
        tempSaveData.second = GameManager.Instance.GetPlayTime().second;
        tempSaveData.curSceneName = GameManager.Instance.curScene.ToString();
    }
    public void SaveToJson()
    {
        File.WriteAllText(path + nowSlot.ToString(), JsonUtility.ToJson(saveData));
        FindAnyObjectByType<UI_CenterPopupText>()?.SetPopupText
                ("저장이 완료되었습니다.");
    }
    public void LoadFromJson()
    {
        string jsonData = File.ReadAllText(path + nowSlot.ToString());
        saveData = JsonUtility.FromJson<SaveData>(jsonData);
        tempSaveData = saveData;

    }

    public void DataClear()
    {
        nowSlot = -1;
        saveData = new SaveData();
    }
    void SaveInventoryItems(List<Item> items)
    {
        for(int i = 0; i< items.Count; ++i) // 저장 할 때, items의 0번부터 돌면서 저장한다.
        {
            if(items[i] is Equip equip)
            {
                tempSaveData.inventoryEquips.Add(equip);
            }
            else if(items[i] is Consumable consumable)
            {
                SaveCountItem count = new SaveCountItem(items[i],Inventory.Instance.CountItemSave(items[i]));
                tempSaveData.countItem.Add(count);                  // 고로 countItem의 인덱스 번호와
                tempSaveData.inventoryConsumables.Add(consumable);  // inventoryConsumables의 인덱스가
            }                                                   // 가지고있는데 Item은 같다.
        }
    }
}
