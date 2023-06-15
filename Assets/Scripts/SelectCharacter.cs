using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCharacter : MonoBehaviour
{
    public void SelectCharacterButton(int val)
    {
        GameManager.Instance.GameStart(val);
    }
}
