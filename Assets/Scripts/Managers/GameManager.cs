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

    // 씬 이동 관련
    private bool firstScene = true;     // 씬 이동 시 플레이어의 인스턴스 위치를 정하기 위함
    public int curPortalID;

    private void Start()
    {
        curGold = 0;
        LoadingSceneController.LoadScene("TitleScene", false);
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
        firstScene = false;
        //LoadingSceneController.LoadScene("SelectCharacter", false);
        selectPlayerData = DataManager.Instance.playerDatas[0];
        SaveDataManager.Instance.saveData.charId = 0;
        LoadingSceneController.LoadScene("Dev", true);
    }
    public void LoadGame(int charId)
    {
        selectPlayerData = DataManager.Instance.playerDatas[charId];
        LoadingSceneController.LoadScene(SaveDataManager.Instance.saveData.curSceneName,true);
    }
    public void SelectCharacter(int charId)
    {
        selectPlayerData = DataManager.Instance.playerDatas[charId];
        SaveDataManager.Instance.saveData.charId = charId;
        LoadingSceneController.LoadScene("Dev",true);
    }
    public bool PlayScene(GameObject _player, string curSceneName)
    {
        curGold = 0;
        player = _player.GetComponent<Player>();
        playerStatus = _player.GetComponent<PlayerStatus>();
        LoadPlayerData();
        ModifyGold(SaveDataManager.Instance.saveData.curGold,true);
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
        SkillManager.Instance.LoadSkillData();
        playerStatus.LoadDynamincStatus();
    }
    public void ModifyGold(int value, bool isAdd)
    {
        if(isAdd)
            curGold += value;
        else
            curGold -= value;

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
        firstScene = true;
        UI_DeadText.instance.ActiveDeadText();
    }
    public void DeadTextAfterLoading()
    {
        GoToTitleScene();
    }
    public void GoToTitleScene()
    {
        firstScene = true;
        FakeLoading.instance.StartFakeLoding(0.3f, "TitleScene", false);
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
    public void GameDataLoad()
    {
        SaveDataManager.Instance.LoadFromJson();
    }

}
