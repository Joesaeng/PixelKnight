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
    public float curStamina;
    public float curPoise;
    public List<bool> activatedSkills = new();
    public List<int> curUsedSkills = new();
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

    private void Start()
    {
        path = Application.persistentDataPath + "/save";
    }
    public void Save()
    {
        if (GameManager.Instance.player != null)
        {
            Player savePlayer = GameManager.Instance.player;
            saveData = savePlayer.playerStatus.SaveStatus();
            saveData.curGold = GameManager.Instance.curGold;
            saveData.playerCurPos = GameManager.Instance.player.transform.position;
            SkillManager.Instance.SaveSkillData(ref saveData.activatedSkills,ref saveData.curUsedSkills);
            saveData.curEquips.AddRange(savePlayer.playerStatus.equipment.GetCurEquip());
            List<Item> items = Inventory.Instance.GetItems();
            SaveInventoryItems(items);

            saveData.inventorySlotCount = Inventory.Instance.SlotCnt;
        }
        saveData.hour = GameManager.Instance.GetPlayTime().hour;
        saveData.minute = GameManager.Instance.GetPlayTime().minute;
        saveData.second = GameManager.Instance.GetPlayTime().second;
        saveData.curSceneName = GameManager.Instance.curScene.ToString();
    }
    public void SaveToJson()
    {
        File.WriteAllText(path + nowSlot.ToString(), JsonUtility.ToJson(saveData));
        UI_CenterPopupText.instance.SetPopupText
                ("저장이 완료되었습니다.");
    }
    public void LoadFromJson()
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
            else if(items[i] is Consumable consumable)  // i 번째 아이템이 소모품(개수가 있는 아이템) 일때
            {
                SaveCountItem count = new SaveCountItem(items[i],Inventory.Instance.CountItemSave(items[i]));
                saveData.countItem.Add(count);                  // SaveCountItem 클래스에 현재 아이템과, 개수를 저장한다
                saveData.inventoryConsumables.Add(consumable);  // items[i]와 consumable은 동일 객체
            }                                                   // items[i]의 정보와 개수를 알고있으니 로드가가능하다.
        }
    }
}
