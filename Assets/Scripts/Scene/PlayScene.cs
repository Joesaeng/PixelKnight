using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayScene : MonoBehaviour
{
    public string curSceneName;
    public GameObject playerPrefab;
    [SerializeField]
    UI_PlayerInfo playerInfo;
    [SerializeField]
    UI_CharacterHeadBarPosUpdate charHeadBarPos;
    [SerializeField]
    public VirtualCam virtualCam;
    [SerializeField]
    Transform startPos;
    void Start()
    {
        GameObject player = Instantiate(playerPrefab);

        if (GameManager.Instance.PlayScene(player, curSceneName))
            player.transform.localPosition = SaveDataManager.Instance.saveData.playerCurPos;
        else
            player.transform.localPosition = startPos.transform.position;
        player.GetComponent<PlayerSkills>().InitUI();
        playerInfo.InitPlayerInfo();
        charHeadBarPos.InitPlayerPoiseBar();
        virtualCam.SetFollow(player.transform);
    }
}
