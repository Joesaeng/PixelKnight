using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevScene : MonoBehaviour
{
    public GameObject playerPrefab;
    [SerializeField]
    PlayerLevelText levelText;
    [SerializeField]
    PlayerGageBarUI gageBarUI;
    void Start()
    {
        PlayerData playerData = GameManager.Instance.selectPlayerData;

        GameObject player = Instantiate(playerPrefab);

        GameManager.Instance.DevScene(player);
        gageBarUI.InitGageBarUI();
        levelText.InitLevelText();
        ItemDataBase.Instance.CreateFieldItem();
        
    }
}
