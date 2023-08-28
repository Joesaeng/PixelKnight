using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class SaveData
{
    public string name;
    public int level;
    public float curExp;
    public int remainingPoint;
    public int addedPoint;
    public int curGold;
    public Vector2 playerCurPos = new();

    public int initVit;
    public int initEnd;
    public int initStr;
    public int initDex;
    public int initLuk;
    public int AddedVit;
    public int AddedEnd;
    public int AddedStr;
    public int AddedDex;
    public int AddedLuk;

    public float curHp;
    public List<SkillName> skills = new();
    public List<Equip> curEquips = new();
    public List<Item> inventoryItems = new();
}
public class SaveDataManager : Singleton<SaveDataManager>
{
    public SaveData saveData = new();
    public string path;
    public int nowSlot;

    public List<SkillName> skills = new();
    public List<Equip> curEquips = new();
    public List<Item> inventoryItems = new();
    private void Awake()
    {
        path = Application.persistentDataPath + "/save";
    }
    public void Save()
    {
        if(GameManager.Instance.player != null)
        {
            Player savePlayer = GameManager.Instance.player;
            saveData = savePlayer.playerStatus.GetSaveStatus();
            saveData.curGold = GameManager.Instance.curGold;
            saveData.playerCurPos = GameManager.Instance.player.transform.position;
            saveData.skills = savePlayer.skills.GetEnableSkills();
            saveData.curEquips.AddRange(savePlayer.playerStatus.equipment.GetCurEquip());
            saveData.inventoryItems = Inventory.Instance.GetItems();
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
}
