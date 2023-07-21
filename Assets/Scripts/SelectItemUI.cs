using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectItemUI : MonoBehaviour
{
    #region SINGLETON
    private static SelectItemUI instance;
    public static SelectItemUI Instance
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
        DontDestroyOnLoad(this.gameObject);
    }
    #endregion
    public GameObject selectItemPanel;
    public int slotNum;
    public Item item;
    public Image itemIcon;
    public Text itemName;
    public Text itemBaseOption;

    public Text addtionalOptionTextPrefab;
    public Transform addtionalPanel;

    List<Text> itemAddtionalOption = new List<Text>();

    public Button useButton;

    public void SetItem(int _slotNum)
    {
        selectItemPanel.SetActive(true);
        slotNum = _slotNum;

        item = Inventory.Instance.items[slotNum];
        itemIcon.sprite = item.itemImage;
        Color nameColor = new Color();
        switch (item.itemLevel)
        {
            case ItemLevel.Common:
                nameColor = new Color(1f, 1f, 1f);
                break;
            case ItemLevel.Advanced:
                nameColor = new Color(117f / 255f, 106f / 255f, 238f / 255f); // 정규화된 값 사용
                break;
            case ItemLevel.Rare:
                nameColor = new Color(229f / 255f, 246f / 255f, 144f / 255f); // 정규화된 값 사용
                break;
            case ItemLevel.Unique:
                nameColor = new Color(233f / 255f, 131f / 255f, 58f / 255f); // 정규화된 값 사용
                break;
        } // 아이템 레벨에 따른 이름 색깔 정하기
        itemName.color = nameColor;
        itemName.text = item.itemName;
        string baseOptionName;
        string baseOptionValue;
        if (item is Equip equip)
        {
            if (equip.baseOption == BaseOption.Damage)
                baseOptionName = "Damage";
            else
                baseOptionName = "Defence";
            baseOptionValue = ((int)equip.baseOptionValue).ToString();
            itemBaseOption.text = string.Format(baseOptionName + " " + baseOptionValue);
            if(equip.additionalOptions.Count > 0)
            {
                SetAddtionalOption(equip.additionalOptions);
            }
        }
    }
    public void UseButton()
    {
        if (item == null) return;
        bool isUse = item.Use();
        if (isUse)
        {
            Inventory.Instance.RemoveItem(slotNum);
            foreach (var text in itemAddtionalOption)
            {
                Destroy(text.gameObject);
            }
            itemAddtionalOption.Clear();
            selectItemPanel.SetActive(false);
        }
    }
    void SetAddtionalOption(Dictionary<AdditionalOptions, float> additionalOptions)
    {
        foreach (var text in itemAddtionalOption)
        {
            Destroy(text.gameObject);
        }
        itemAddtionalOption.Clear();
        foreach (var option in additionalOptions)
        {
            string name = "";
            string value = "";
            switch (option.Key)
            {
                case AdditionalOptions.Vitality:
                    name = "Vitality";
                    value = option.Value.ToString("F0");
                    break;
                case AdditionalOptions.Endurance:
                    name = "Endurance";
                    value = option.Value.ToString("F0");
                    break;
                case AdditionalOptions.Strength:
                    name = "Strength";
                    value = option.Value.ToString("F0");
                    break;
                case AdditionalOptions.Dexterity:
                    name = "Dexterity";
                    value = option.Value.ToString("F0");
                    break;
                case AdditionalOptions.Luck:
                    name = "Luck";
                    value = option.Value.ToString("F0");
                    break;
                case AdditionalOptions.MaxHp:
                    name = "MaxHp";
                    value = option.Value.ToString("F0");
                    break;
                case AdditionalOptions.MaxStamina:
                    name = "MaxStamina";
                    value = option.Value.ToString("F0");
                    break;
                case AdditionalOptions.Stagger:
                    name = "Stagger";
                    value = option.Value.ToString("F0");
                    break;
                case AdditionalOptions.Poise:
                    name = "Poise";
                    value = option.Value.ToString("F0");
                    break;
                case AdditionalOptions.AttackSpeed:
                    name = "AttackSpeed";
                    value = string.Format(option.Value * 100f + "%");
                    break;
                case AdditionalOptions.Evade:
                    name = "Evade";
                    value = option.Value.ToString("F0");
                    break;
                case AdditionalOptions.HitRate:
                    name = "HitRate";
                    value = option.Value.ToString("F0");
                    break;
                case AdditionalOptions.MoveSpeed:
                    name = "MoveSpeed";
                    value = string.Format(option.Value * 100f + "%");
                    break;
                case AdditionalOptions.CriticalChance:
                    name = "CriticalChance";
                    value = string.Format(option.Value * 100f + "%");
                    break;
                case AdditionalOptions.CriticalHitDamage:
                    name = "CriticalHitDamage";
                    value = option.Value.ToString("F0");
                    break;
                case AdditionalOptions.IncreasedItemFindingChance:
                    name = "IncreasedItemFindingChance";
                    value = string.Format(option.Value * 100f + "%");
                    break;
                case AdditionalOptions.HpRegen:
                    name = "HpRegen";
                    value = option.Value.ToString("F0");
                    break;
                case AdditionalOptions.StaminaRegen:
                    name = "StaminaRegen";
                    value = option.Value.ToString("F0");
                    break;
                case AdditionalOptions.PoiseRegen:
                    name = "PoiseRegen";
                    value = option.Value.ToString("F0");
                    break;
                default:
                    Debug.LogError("PlayerStatus_ApplyEquip_AddtionalOptions");
                    break;
            }
            Text additionalOptionText = Instantiate(addtionalOptionTextPrefab, addtionalPanel.transform);
            additionalOptionText.text = string.Format(name + ":" + value);
            itemAddtionalOption.Add(additionalOptionText);
        }
    }
}
