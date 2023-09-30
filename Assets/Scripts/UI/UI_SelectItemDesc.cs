using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SelectItemDesc : MonoBehaviour
{
    // �κ��丮���� ���õ� �������� ������ ���� UI �Դϴ�.
    #region SINGLETON
    private static UI_SelectItemDesc instance;
    public static UI_SelectItemDesc Instance
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

    public void SetItem(int _slotNum) // �κ��丮���� �������� ���õǸ� ����Ǵ� �޼��� �Դϴ�.
    {
        useButton.gameObject.SetActive(true);
        foreach (var text in itemAddtionalOption)
        {
            Destroy(text.gameObject);
        }
        itemAddtionalOption.Clear();
        selectItemPanel.SetActive(true);
        slotNum = _slotNum;

        item = Inventory.Instance.GetItem(slotNum);
        if(item == null)
        {
            selectItemPanel.SetActive(false);
            return;
        }
        itemIcon.sprite = item.itemImage;

        Color nameColor = new Color();
        switch (item.itemLevel)
        {
            case ItemLevel.Common:
                nameColor = new Color(1f, 1f, 1f);
                break;
            case ItemLevel.Advanced:
                nameColor = new Color(117f / 255f, 106f / 255f, 238f / 255f); // ����ȭ�� �� ���
                break;
            case ItemLevel.Rare:
                nameColor = new Color(229f / 255f, 246f / 255f, 144f / 255f); // ����ȭ�� �� ���
                break;
            case ItemLevel.Unique:
                nameColor = new Color(233f / 255f, 131f / 255f, 58f / 255f); // ����ȭ�� �� ���
                break;
            default:
                nameColor = new Color(1f, 1f, 1f);
                break;
        } // ������ ������ ���� �̸� ���� ���ϱ�
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
            if (equip.additionalOptions.Count > 0) // �����ۿ� �߰��ɼ��� ���� �� �߰��մϴ�.
            {
                SetAddtionalOption(equip.additionalOptions);
            }
        }
        else if (item is Consumable consumable)
        {
            useButton.gameObject.SetActive(false);
            baseOptionName = consumable.consumableType.ToString();
            baseOptionValue = ((int)consumable.value + 
                                (int)GameManager.Instance.playerStatus.dPlayerFixedStatus[FixedStatusName.HpPotionIncrease]).ToString();
            itemBaseOption.text = string.Format(baseOptionName + " " + baseOptionValue);
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
    public void DestroyButton()
    {
        if (item == null) return;
        Inventory.Instance.RemoveItem(slotNum);
        foreach (var text in itemAddtionalOption)
        {
            Destroy(text.gameObject);
        }
        itemAddtionalOption.Clear();
        selectItemPanel.SetActive(false);
    }
    void SetAddtionalOption(List<AdditionalOption> additionalOptions)
    {
        // �߰��ɼ� �ؽ�Ʈ�� ���������� ���� �߰��մϴ�.
        foreach (var option in additionalOptions)
        {
            string name = option.Key.ToString();
            string value;
            switch (option.Key)
            {
                case AdditionalOptions.AttackSpeed:
                    value = string.Format(option.Value * 100f + "%");
                    break;
                case AdditionalOptions.MoveSpeed:
                    value = string.Format((option.Value * 100f).ToString("F0"));
                    break;
                case AdditionalOptions.CriticalChance:
                    value = string.Format(option.Value * 100f + "%");
                    break;
                case AdditionalOptions.CriticalHitDamage:
                    value = string.Format(option.Value * 100f + "%");
                    break;
                default:
                    name = option.Key.ToString();
                    value = option.Value.ToString();
                    break;
            }
            Text additionalOptionText = Instantiate(addtionalOptionTextPrefab, addtionalPanel.transform);
            additionalOptionText.text = string.Format("�� " + name + " +" + value);
            itemAddtionalOption.Add(additionalOptionText);
        }
    }
}
