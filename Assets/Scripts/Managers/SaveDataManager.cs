using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class SaveData
{
    public string name;
    public int level;
    public int charId;
    public float curExp;
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
    public List<Item> inventoryItems = new();
    public List<Equip> inventoryEquips = new();
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

        string data = JsonUtility.ToJson(saveData);
        File.WriteAllText(path + nowSlot.ToString(), data);
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
        for(int i = 0; i< items.Count; ++i)
        {
            if(items[i] is Equip equip)
            {
                saveData.inventoryEquips.Add(equip);
            }
            else
            {
                saveData.inventoryItems.Add(items[i]);
            }
        }
    }
}
