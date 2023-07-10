using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : Singleton<GameManager>
{
    [Header("# GameObject")]
    public Player player;
    public PlayerData selectPlayerData;
    //public Inventory inventory;
    //public Equipment equipment;

    private void Start()
    {
        SceneManager.LoadScene(1);
    }
    public void GameStart(int val)
    {
        selectPlayerData = DataManager.Instance.playerDatas[val];
        SceneManager.LoadScene(2);
    }
    public void DevScene(GameObject _player)
    {
        player = _player.GetComponent<Player>();
        player.playerStatus.InitSetStatus(selectPlayerData);
        player.playerStatus.OnPlayerDead += PlayerDead;
    }

    void PlayerDead()
    {
        // TODO
    }




}
