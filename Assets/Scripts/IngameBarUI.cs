using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameBarUI : MonoBehaviour
{
    List<Transform> objectList = new List<Transform>();
    List<GameObject> barList = new List<GameObject>();

    Camera cam = null;

    private void Start()
    {
        cam = Camera.main;
        Spawner.instance.OnEnemySpawn += InstantiateHPUI;
        
    }
    public void InstantiateHPUI()
    {
        GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < enemys.Length; ++i)
        {
            objectList.Add(enemys[i].transform);
            //GameObject hpBar = Instantiate(barPrefab, enemys[i].transform.position, Quaternion.identity, transform);
            GameObject hpBar = PoolManager.Instance.Get(PoolType.EnemyHpUI);
            hpBar.transform.position = enemys[i].transform.position;
            hpBar.transform.SetParent(transform);
            barList.Add(hpBar);
            enemys[i].GetComponent<EnemyStatus>().SetHPUI(hpBar);
        }
    }

    private void Update()
    {
        for(int i =0;i<objectList.Count; ++i)
        {
            barList[i].transform.position = cam.WorldToScreenPoint(objectList[i].position + new Vector3(0, 0.7f, 0));

        }
    }
}
