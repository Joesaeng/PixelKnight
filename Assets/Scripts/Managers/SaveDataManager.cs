using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class SaveData
{
    public string name;
    public int level;
    public int charId;
    public int hour = 0;
    public int minute = 0;
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
    public string path;
    public int nowSlot;

    private void Awake()
    {
        path = Application.persistentDataPath + "/save";
    }
    public void Save()
    {
        if(GameManager.Instance.player != null)
        {
            Player savePlayer = GameManager.Instance.player;
            saveData = savePlayer.playerStatus.SaveStatus();
            saveData.curGold = GameManager.Instance.curGold;
            saveData.playerCurPos = GameManager.Instance.player.transform.position;
            saveData.skills = savePlayer.skills.GetEnableSkills();
            saveData.curEquips.AddRange(savePlayer.playerStatus.equipment.GetCurEquip());
            List<Item> items = Inventory.Instance.GetItems();
            SaveInventoryItems(items);

            saveData.inventorySlotCount = Inventory.Instance.SlotCnt;
        }
        saveData.hour = GameManager.Instance.GetPlayTime().hour;
        saveData.minute = GameManager.Instance.GetPlayTime().minute;
        
        string data = JsonUtility.ToJson(saveData);
        File.WriteAllText(path + nowSlot.ToString(), data);

        FindAnyObjectByType<UI_CenterPopupText>()?.SetPopupText
                ("저장이 완료되었습니다.");
    }

    public void Load()
    {
        string jsonData = File.ReadAllText(path + nowSlot.ToString());
        saveData = JsonUtility.FromJson<SaveData>(jsonData);

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
                saveData.inventoryEquips.Add(equip);
            }
            else if(items[i] is Consumable consumable)
            {
                SaveCountItem count = new SaveCountItem(items[i],Inventory.Instance.CountItemSave(items[i]));
                saveData.countItem.Add(count);                  // 고로 countItem의 인덱스 번호와
                saveData.inventoryConsumables.Add(consumable);  // inventoryConsumables의 인덱스가
            }                                                   // 가지고있는데 Item은 같다.
        }
    }
}
