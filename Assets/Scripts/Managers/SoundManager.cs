using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    public void SFXPlay(AudioClip clip,Vector2 pos)
    {
        StartCoroutine(CoSFXPlay(clip, pos));
    }
    IEnumerator CoSFXPlay(AudioClip clip,Vector2 pos)
    {
        GameObject sfx = PoolManager.Instance.Get(PoolType.SFX);
        sfx.transform.position = pos;
        sfx.GetComponent<AudioSource>().clip = clip;
        sfx.GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(clip.length);

        sfx.SetActive(false);
    }
}
