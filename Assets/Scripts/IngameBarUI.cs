using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameBarUI : MonoBehaviour
{
    List<Transform> objectList = new List<Transform>();
    List<GameObject> barList = new List<GameObject>();

    GameObject playerPoiseBar;
    Transform playerTransform;
    Camera cam = null;

    private void Start()
    {
        cam = Camera.main;
        Spawner.instance.OnEnemySpawn += InstantiateHPUI;

    }
    public void InstantiateHPUI(GameObject enemy)
    {
        objectList.Add(enemy.transform);
        GameObject hpBar = PoolManager.Instance.Get(PoolType.EnemyHpUI);
        hpBar.transform.position = enemy.transform.position;
        hpBar.transform.SetParent(transform);
        barList.Add(hpBar);
        enemy.GetComponent<EnemyStatus>().SetHPUI(hpBar);
    }
    public void InstantiatePlayerPoiseUI()
    {
        playerPoiseBar = PoolManager.Instance.Get(PoolType.EnemyHpUI);
        playerPoiseBar.transform.position = GameManager.Instance.player.transform.position;
        playerPoiseBar.transform.SetParent(transform);
        playerPoiseBar.GetComponent<BarValueUI>().InitPlayerStatus();
        playerPoiseBar.transform.localScale = new Vector3(2f, 1f, 2f);
        playerTransform = GameManager.Instance.player.transform;
    }

    private void Update()
    {
        for (int i = 0; i < objectList.Count; ++i)
        {
            barList[i].transform.position = cam.WorldToScreenPoint(objectList[i].position + new Vector3(0, 0.7f, 0));
        }
        playerPoiseBar.transform.position = cam.WorldToScreenPoint(playerTransform.position + new Vector3(0, -0.7f, 0));
    }
}
