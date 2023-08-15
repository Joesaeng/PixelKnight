using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region SINGLETON
    private static Inventory instance;
    public static Inventory Instance
    {
        get
        {
            if(instance == null)
            {
                return null;
            }
            return instance;
        }
    }
    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    #endregion

    public delegate void OnSlotCountChange(int val);
    public OnSlotCountChange onSlotCountChange;

    public delegate void OnChangeItem();
    public OnChangeItem onChangeItem;

    public List<Item> items = new List<Item>();   

    private int slotCnt;
    public int SlotCnt
    {
        get => slotCnt;
        set
        {
            slotCnt = value;
            onSlotCountChange.Invoke(slotCnt);
        }
    }
    void Start()
    {
        SlotCnt = 20;
    }
    public List<Item> GetItems()
    {
        return items;
    }
    public bool AddItem(Item item)
    {
        if(items.Count < SlotCnt)
        {
            if(item is Equip equip)
            {
                items.Add(equip);
            }
            else
            {
                 items.Add(item);
            }
            if(onChangeItem != null)
                onChangeItem.Invoke();
            return true;
        }
        return false;
    }

    public void RemoveItem(int slotnum)
    {
        items.RemoveAt(slotnum);
        onChangeItem.Invoke();
    }
}
