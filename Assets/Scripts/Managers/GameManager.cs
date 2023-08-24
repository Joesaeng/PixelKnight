using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
public class GameManager : Singleton<GameManager>
{
    [Header("# GameObject")]
    public Player player;
    public PlayerData selectPlayerData;

    [Header("# preps")]
    public int curGold;

    public Action OnChangedGold;
    public DevScene devScene;
    private void Start()
    {
        SceneManager.LoadScene(1);
        ModifyGold(0);
    }
    public void GameStart(int val)
    {
        selectPlayerData = DataManager.Instance.playerDatas[val];
        SceneManager.LoadScene(2);
    }
    public void DevScene(GameObject _player,DevScene _devScene)
    {
        player = _player.GetComponent<Player>();
        player.playerStatus.InitSetStatus(selectPlayerData);
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




}
