using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEffect : MonoBehaviour
{
    public RuntimeAnimatorController[] effects;
    Animator animator;
    float time;
    float curTime = 0f;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        animator.enabled = false;
        
    }
    private void OnEnable()
    {
        animator.enabled = true;
        time = animator.GetCurrentAnimatorStateInfo(0).length;
    }
    private void Update()
    {
        if(curTime > time)
        {
            curTime = 0f;
            animator.enabled = false;
            gameObject.SetActive(false);
        }
        curTime += Time.deltaTime;
    }
    public void SetEffect(int index)
    {
        animator.runtimeAnimatorController = effects[index];
    }
}
