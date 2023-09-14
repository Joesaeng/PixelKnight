using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
public class SlotSelect : MonoBehaviour
{
    // Title씬에서 세이브된 데이터를 로드할때 사용하는 클래스입니다.
    public GameObject create;
    public GameObject selectSlot;
    public Text[] slotTexts;
    public Text newPlayerName;

    bool[] savefile = new bool[3];
    private void Start()
    {
        create.SetActive(false);
        for (int i = 0; i < 3; ++i)
        {
            if (File.Exists(SaveDataManager.Instance.path + $"{i}"))
            {
                savefile[i] = true;
                SaveDataManager.Instance.nowSlot = i;
                SaveDataManager.Instance.LoadFromJson();
                SaveData saveData = SaveDataManager.Instance.saveData;

                string username = saveData.name;
                string level = (saveData.level + 1).ToString();
                int ptH = saveData.hour;
                int ptM = saveData.minute;
                string time = string.Format("{0:D2}:{1:D2}", ptH, ptM);
                slotTexts[i].text = "USERNAME : " + username
                    + "\nLV : " + level
                    + "\nPLAY TIME : " + time;
            }
            else
            {
                slotTexts[i].text = "NEW GAME";
            }
        }
        SaveDataManager.Instance.DataClear();
    }
    public void Slot(int num)
    {
        SaveDataManager.Instance.nowSlot = num;

        if (savefile[num])
        {
            SaveDataManager.Instance.LoadFromJson();
            GoGame();
        }
        else
        {
            OnCreate();
        }


    }
    public void OnCreate()
    {
        create.SetActive(true);
    }
    public void GoGame()
    {
        if (!savefile[SaveDataManager.Instance.nowSlot])
        {
            string username = newPlayerName.text.Trim();
            if (string.IsNullOrEmpty(username))
            {
                return;
            }
            SaveDataManager.Instance.saveData.name = username;
            GameManager.Instance.NewGame();
        }
        else
        {
            GameManager.Instance.LoadGame(SaveDataManager.Instance.saveData.charId);
        }

    }
    public void GameExit()
    {
        GameManager.Instance.GameExit();
    }
}
