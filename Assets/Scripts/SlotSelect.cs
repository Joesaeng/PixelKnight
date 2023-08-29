using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
public class SlotSelect : MonoBehaviour
{
    public GameObject create;
    public GameObject selectSlot;
    public Text[] slotTexts;
    public Text newPlayerName;

    bool[] savefile = new bool[3];
    private void Start()
    {
        create.SetActive(false);
        for(int i = 0; i < 3; ++i)
        {
            if(File.Exists(SaveDataManager.Instance.path + $"{i}"))
            {
                savefile[i] = true;
                SaveDataManager.Instance.nowSlot = i;
                SaveDataManager.Instance.Load();
                slotTexts[i].text = SaveDataManager.Instance.saveData.name;
            }
            else
            {
                slotTexts[i].text = "비어 있음";
            }
        }
        SaveDataManager.Instance.DataClear();
    }
    public void Slot(int num)
    {
        SaveDataManager.Instance.nowSlot = num;
        
        if(savefile[num])
        {
            SaveDataManager.Instance.Load();
            GoGame();
        }
        else
        {
            OnCreate();
        }

        
    }
    public void OnCreate()
    {
        create.gameObject.SetActive(true);
    }
    public void GoGame()
    {
        if(!savefile[SaveDataManager.Instance.nowSlot])
        {
            SaveDataManager.Instance.saveData.name = newPlayerName.text;
            SaveDataManager.Instance.Save();
            GameManager.Instance.NewGame();
        }
        else
        {
            GameManager.Instance.LoadGame(SaveDataManager.Instance.saveData.charId);
        }
    }

}
