using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevScene : MonoBehaviour
{
    public GameObject playerPrefab;
    [SerializeField]
    UI_PlayerInfo playerInfo;
    [SerializeField]
    UI_CharacterHeadBarPosUpdate charHeadBarPos;
    [SerializeField]
    public VirtualCam virtualCam;
    void Start()
    {
        GameObject player = Instantiate(playerPrefab);

        GameManager.Instance.DevScene(player,this.GetComponent<DevScene>());
        player.transform.localPosition = SaveDataManager.Instance.saveData.playerCurPos;
        player.GetComponent<PlayerSkills>().InitUI();
        playerInfo.InitPlayerInfo();
        charHeadBarPos.InitPlayerPoiseBar();
        virtualCam.SetFollow(player.transform);
    }
}
