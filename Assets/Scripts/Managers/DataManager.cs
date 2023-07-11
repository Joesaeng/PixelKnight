using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    public PlayerData[] playerDatas;
    public EnemyData[] enemyDatas;
    public PlayerData GetPlayerData(int id)
    {
        return playerDatas[id];
    }
    public EnemyData GetEnemyData(int id)
    {
        return enemyDatas[id];
    }
}