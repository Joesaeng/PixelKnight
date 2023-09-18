using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayScene : MonoBehaviour
{
    public string curSceneName;
    public GameObject playerPrefab;
    [SerializeField]
    public VirtualCam virtualCam;
    [SerializeField]
    Transform startPos;
    protected virtual void Awake()
    {
        GameObject player = Instantiate(playerPrefab);

        if (GameManager.Instance.PlayScene(player, curSceneName))
            player.transform.localPosition = SaveDataManager.Instance.saveData.playerCurPos;
        else
            player.transform.localPosition = startPos.transform.position;
        virtualCam.SetFollow(player.transform);
    }
}
