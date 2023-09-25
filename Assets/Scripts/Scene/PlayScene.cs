using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayScene : MonoBehaviour
{
    public string curSceneName;
    public GameObject playerPrefab;
    [SerializeField]
    VirtualCam virtualCam;
    [SerializeField]
    Transform startPos;
    [SerializeField]
    bool newGameScene;
    [SerializeField]
    Portal[] portals;
    [SerializeField]
    BGMName bgm;
    protected virtual void Awake()
    {
        portals = FindObjectsByType<Portal>(FindObjectsSortMode.None);
        GameObject player = Instantiate(playerPrefab);

        if (GameManager.Instance.PlayScene(player, curSceneName))
            player.transform.localPosition = SaveDataManager.Instance.saveData.playerCurPos;
        else
        {
            if(newGameScene)
                player.transform.localPosition = startPos.transform.position;
            else
            {
                PortalManager pmgr = PortalManager.Instance;
                foreach (Portal portal in portals)
                {
                    int ID = pmgr.GetConnectedPortal(pmgr.curUsePortalID);
                    if(portal.portalID == ID)
                    {
                        player.transform.localPosition = portal.transform.localPosition;
                        break;
                    }
                }
            }
        }
        virtualCam.SetFollow(player.transform);
        SoundManager.Instance.SetBgm(bgm);
    }
}
