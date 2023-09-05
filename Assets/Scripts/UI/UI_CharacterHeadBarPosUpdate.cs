using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_CharacterHeadBarPosUpdate : MonoBehaviour
{
    // ĳ���� �Ӹ��� ����ٴϴ� Bar�� ��ġ�� �����ϴ� ��ũ��Ʈ�Դϴ�.
    List<Transform> objectList = new List<Transform>();
    List<GameObject> barList = new List<GameObject>();

    GameObject playerPoiseBar;
    Transform playerTransform;
    Camera cam = null;

    private void Start()
    {
        cam = Camera.main;
        // Enemy�� ������ ����ϴ� Spawner�� �̺�Ʈ�� �����մϴ�.
        Spawner.instance.OnEnemySpawn += InitEnemyHpBar;
    }
    public void InitEnemyHpBar(GameObject enemy)
    {
        objectList.Add(enemy.transform);
        GameObject hpBar = PoolManager.Instance.Get(PoolType.EnemyHpUI);
        hpBar.transform.position = enemy.transform.position;
        hpBar.transform.SetParent(transform);
        barList.Add(hpBar);
        enemy.GetComponent<EnemyStatus>().SetHPUI(hpBar);
    }
    public void InitPlayerPoiseBar()
    {
        playerPoiseBar = PoolManager.Instance.Get(PoolType.EnemyHpUI);
        playerPoiseBar.transform.position = GameManager.Instance.player.transform.position;
        playerPoiseBar.transform.SetParent(transform);
        playerPoiseBar.GetComponent<UI_CharacterHeadBarValue>().InitPlayerStatus();
        playerPoiseBar.transform.localScale = new Vector3(2f, 1f, 2f);
        playerTransform = GameManager.Instance.player.transform;
    }

    private void Update()
    {
        for (int i = 0; i < objectList.Count; ++i)
        {
            barList[i].transform.position = cam.WorldToScreenPoint(objectList[i].position + new Vector3(0, 0.7f, 0));
        }
        if(playerPoiseBar == null) return;
        playerPoiseBar.transform.position = cam.WorldToScreenPoint(playerTransform.position + new Vector3(0, -0.7f, 0));
    }
}
