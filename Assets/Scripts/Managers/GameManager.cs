using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class PlayTime
{
    public int hour;
    public int minute;
    public float second;
}

public class GameManager : Singleton<GameManager>
{
    [Header("# GameObject")]
    public Player player;
    public PlayerStatus playerStatus;
    public PlayerData selectPlayerData;

    [Header("# preps")]
    public int curGold;

    public Action OnChangedGold;
    public string curScene;

    public float hpPotionCooltime = 10f;
    float curPotionCooltime;

    private float playTime = 0f;
    private int playTimeMinute = 0;
    private int playTimeHour = 0;

    private bool firstScene = true;
    private void Start()
    {
        ModifyGold(0);
    }
    private void Update()
    {
        if (curPotionCooltime >= 0)
            curPotionCooltime -= Time.deltaTime;
        playTime += Time.deltaTime;
        if(playTime >= 60f)
        {
            playTimeMinute++;
            playTime = 0f;
            if(playTimeMinute >= 60)
            {
                playTimeHour++;
                playTimeMinute = 0;
            }    
        }

    }
    public void NewGame()
    {
        SceneManager.LoadScene(1);
    }
    public void LoadGame(int charId)
    {
        selectPlayerData = DataManager.Instance.playerDatas[charId];
        LoadingSceneController.LoadScene(SaveDataManager.Instance.saveData.curSceneName);
    }
    public void SelectCharacter(int charId)
    {
        selectPlayerData = DataManager.Instance.playerDatas[charId];
        SaveDataManager.Instance.saveData.charId = charId;
        LoadingSceneController.LoadScene("Dev");
    }
    public bool PlayScene(GameObject _player, string curSceneName)
    {
        player = _player.GetComponent<Player>();
        playerStatus = _player.GetComponent<PlayerStatus>();
        LoadPlayerData();
        ModifyGold(SaveDataManager.Instance.saveData.curGold);
        LoadPlayTime();
        player.playerStatus.OnPlayerDead += PlayerDead;
        curScene = curSceneName;
        if (firstScene)
        {
            firstScene = false;
            return true;
        }
        else return firstScene;
    }
    public void LoadPlayerData()
    {
        playerStatus.InitSetStatus(selectPlayerData);
        Inventory.Instance.LoadItems();
        playerStatus.equipment.LoadEquip();
        player.skills.LoadEnableSkills();
    }
    public void NextScene(string sceneName)
    {
        LoadingSceneController.LoadScene(sceneName);
    }
    public void ModifyGold(int value)
    {
        curGold += value;
        OnChangedGold?.Invoke();
    }
    public int GetCurGold()
    {
        return curGold;
    }
    public void HpPotionUse()
    {
        curPotionCooltime = hpPotionCooltime;
    }
    public float GetHpPotionCooltime()
    {
        return Utils.Percent(curPotionCooltime, hpPotionCooltime);
    }
    void PlayerDead()
    {
        // TODO
    }
    
    public PlayTime GetPlayTime()
    {
        PlayTime pt = new PlayTime();
        pt.second = playTime;
        pt.hour = playTimeHour;
        pt.minute = playTimeMinute;
        return pt;
    }
    void LoadPlayTime()
    {
        SaveData data = SaveDataManager.Instance.saveData;
        playTime = data.second;
        playTimeMinute = data.minute;
        playTimeHour = data.hour;
    }
    public void GameExit()
    {
        Application.Quit();
    }
    public void GameSave()
    {
        SaveDataManager.Instance.Save();
        SaveDataManager.Instance.SaveToJson();
    }
    public void GameSaveForSceneChange()
    {
        SaveDataManager.Instance.Save();
    }
    public void GameLoad()
    {
        SaveDataManager.Instance.LoadFromJson();
    }

}
