using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    // 플레이어의 인벤토리 클래스입니다.
    #region SINGLETON
    private static Inventory instance;
    public static Inventory Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            return instance;
        }
    }
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    #endregion

    public delegate void OnSlotCountChange(int val);
    public OnSlotCountChange onSlotCountChange;

    public delegate void OnChangeItem();
    public OnChangeItem onChangeItem;

    public delegate void OnUpdateItemCount();
    public OnUpdateItemCount onUpdateCount;

    public delegate void OnCurPotionUI(Item item);
    public OnCurPotionUI onCurPotionUI;

    public delegate void OffCurPotionUI();
    public OffCurPotionUI offCurPotionUI;

    public List<Item> items = new List<Item>();
    public Dictionary<Item, int> countItems = new();
    bool haveHpPotion = false;
    public int hpPotionSlot = -1;

    public int slotCnt;
    public int SlotCnt
    {
        get => slotCnt;
        set
        {
            slotCnt = value;
            onSlotCountChange?.Invoke(slotCnt);
        }
    }
    void Start()
    {
        if (slotCnt < 20) SlotCnt = 20;
    }
    public List<Item> GetItems()
    {
        return items;
    }
    public Item GetItem(int index)
    {
        if (items.Count < index)
            return null;
        return items[index];
    }
    public int CountItemSave(Item item)
    {
        if (countItems.ContainsKey(item))
            return countItems[item];
        return 0;
    }
    public void LoadItems()
    {
        SaveData saveData = SaveDataManager.Instance.saveData;
        SlotCnt = saveData.inventorySlotCount;
        onSlotCountChange?.Invoke(slotCnt);
        List<Consumable> consums = saveData.inventoryConsumables;
        List<Equip> equips = saveData.inventoryEquips;
        for (int i = 0; i < consums.Count; ++i)
        {
            if (consums[i] is Consumable consum)
            {
                Consumable newcon = ItemDataBase.Instance.FindAndGetConsumable(consum.itemName);
                int count = saveData.countItem[i].count;
                if (consum.consumableType == ConsumableType.HpRecovery)
                    hpPotionSlot = i;
                for (int j = 0; j < count; ++j)
                    AddItem(newcon);
            }
        }
        for (int i = 0; i < equips.Count; ++i)
        {
            if (equips[i] is Equip equip)
            {
                Equip newequ = new Equip(equip);
                ItemEquipEft itemEquipEft = new();
                newequ.SetItemEffect(itemEquipEft);
                AddItem(newequ);
            }

        }
    }
    public bool AddItem(Item item)
    {
        bool isAdd = false;
        if (item is Equip equip)
        {
            if (items.Count < SlotCnt)
            {
                items.Add(equip);
                isAdd = true;
            }
        }
        else if (item is Consumable consumable)
        {
            isAdd = AddConsumable(consumable);
            if(isAdd)onUpdateCount?.Invoke();
        }
        if (onChangeItem != null && isAdd)
            onChangeItem?.Invoke();
        return isAdd;
    }
    bool AddConsumable(Consumable consum)
    {
        if (HaveGetItem(consum))
        {
            countItems[consum]++;
            return true;
        }
        else
        {
            if(items.Count < SlotCnt)
            {
                countItems.Add(consum, 1);
                items.Add(consum);
                if (consum.consumableType == ConsumableType.HpRecovery)
                {
                    haveHpPotion = true;
                    onCurPotionUI?.Invoke(consum);
                }
                return true;
            }
            return false;
        }
    }
    public void UseConsumable(int slotnum)
    {
        if (countItems[items[slotnum]] > 1)
            countItems[items[slotnum]]--;
        else
        {
            RemoveItem(slotnum);
        }
        onUpdateCount?.Invoke();
    }
    public Item GetHpPotion()
    {
        if (haveHpPotion)
        {
            return items[hpPotionSlot];
        }
        return null;
    }
    public void UseHpPotion()
    {
        if (countItems[items[hpPotionSlot]] > 1)
            countItems[items[hpPotionSlot]]--;
        else
        {
            RemoveItem(hpPotionSlot);
        }
        onUpdateCount?.Invoke();
    }

    bool HaveGetItem(Item item)
    {
        bool isHave = false;
        if (items.Contains(item))
        {
            isHave = true;
        }
        return isHave;
    }
    public void RemoveItem(int slotnum)
    {
        if (countItems.ContainsKey(items[slotnum]))
        {
            if (items[slotnum] is Consumable con)
            {
                if (con.consumableType == ConsumableType.HpRecovery)
                {
                    haveHpPotion = false;
                    hpPotionSlot = -1;
                    offCurPotionUI?.Invoke();
                }
            }
            countItems.Remove(items[slotnum]);
        }
        items.RemoveAt(slotnum);
        onChangeItem?.Invoke();
    }
}
