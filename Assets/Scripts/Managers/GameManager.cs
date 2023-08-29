using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
public class GameManager : Singleton<GameManager>
{
    [Header("# GameObject")]
    public Player player;
    public PlayerStatus playerStatus;
    public PlayerData selectPlayerData;

    [Header("# preps")]
    public int curGold;

    public Action OnChangedGold;
    public DevScene devScene;
    private void Start()
    {
        ModifyGold(0);
    }
    public void NewGame()
    {
        SceneManager.LoadScene(1);
    }
    public void LoadGame(int charId)
    {
        selectPlayerData = DataManager.Instance.playerDatas[charId];
        SceneManager.LoadScene(2);
    }
    public void SelectCharacter(int charId)
    {
        selectPlayerData = DataManager.Instance.playerDatas[charId];
        SaveDataManager.Instance.saveData.charId = charId;
        SceneManager.LoadScene(2);
    }
    public void DevScene(GameObject _player,DevScene _devScene)
    {
        player = _player.GetComponent<Player>();
        playerStatus = _player.GetComponent<PlayerStatus>();
        player.playerStatus.InitSetStatus(selectPlayerData);
        Inventory.Instance.LoadItems();
        playerStatus.equipment.LoadEquip();
        player.skills.LoadEnableSkills();
        player.playerStatus.OnPlayerDead += PlayerDead;
        devScene = _devScene;
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
    void PlayerDead()
    {
        // TODO
    }
    public void GameExit()
    {
        Application.Quit();
    }
    public void GameSave()
    {
        SaveDataManager.Instance.Save();
    }
    public void GameLoad()
    {
        SaveDataManager.Instance.Load();
    }

}
