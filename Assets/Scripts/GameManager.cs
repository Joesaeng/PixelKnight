using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [Header("# Managers")]
    public EquipmentManager equipmentManager;

    [Header("# GameObject")]
    public Player player;




    private void Awake()
    {
        instance = this;
    }
}
