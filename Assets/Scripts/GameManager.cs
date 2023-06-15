using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : Singleton<GameManager>
{
    [Header("# GameObject")]
    public Player player;

    private void Start()
    {
        SceneManager.LoadScene(1);
    }
    public void GameStart(int val)
    {
        //PlayerData data = 
    }




}
