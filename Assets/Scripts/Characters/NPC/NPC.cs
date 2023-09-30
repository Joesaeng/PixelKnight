using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    protected bool isInteraction;

    [SerializeField]
    protected GameObject npcNameObj;

    [SerializeField]
    protected RuntimeAnimatorController animcon;

    Animator animator;
    public string npcName;


    private void Start()
    {
        isInteraction = false;
        npcNameObj.GetComponent<TextMesh>().text = "<" + npcName + ">";
        animator = GetComponent<Animator>();
        animator.runtimeAnimatorController = animcon;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            isInteraction = true;
            npcNameObj.SetActive(true);
            
        }
    }
    protected virtual void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isInteraction = false;
            npcNameObj.SetActive(false);
        }
    }

    private void Update()
    {
        if(isInteraction && Input.GetKeyDown(KeySetting.keys[KeyAction.Interaction]))
        {
            UI_WindowMenu[] menus = FindObjectsByType<UI_WindowMenu>(FindObjectsSortMode.None);
            foreach (UI_WindowMenu menu in menus)
            {
                if (menu.activeMenu) menu.ActiveMenu();
            } // 메뉴창이 켜져있는경우 전부 끔
            Interaction();
        }
    }
    protected virtual void Interaction() { }
}
