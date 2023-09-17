using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    public PlayerData[] playerDatas;
    public EnemyData[] enemyDatas;
    public SkillData[] skillDatas;
    public BulletData[] bulletDatas;
    [SerializeField]
    List<Sprite> images;
    public Dictionary<string,Sprite> itemImages;
    public PlayerData GetPlayerData(int id)
    {
        return playerDatas[id];
    }
    public EnemyData GetEnemyData(int id)
    {
        return enemyDatas[id];
    }
    public SkillData GetSkillData(int id)
    {
        return skillDatas[id];
    }
    public BulletData GetBulletData(BulletName bulletName)
    {
        return bulletDatas[(int)bulletName];
    }
    private void Start()
    {
        itemImages = new Dictionary<string, Sprite>();
        SetImages();
    }
    private void SetImages()
    {
        for(int i = 0; i < images.Count;++i)
        {
            itemImages.Add(images[i].name,images[i]);
        }
    }
    public Sprite GetImage(string name)
    {
        return itemImages[name];
    }

}
