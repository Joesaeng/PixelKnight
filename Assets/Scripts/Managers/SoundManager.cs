using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum SFXName
{
    //Player
    Player_Attack,
    Player_Hit,
    Player_Jump,
    Player_Land,
    Player_Run1,
    Player_Run2,

    //...
}
public enum BGMName
{
    Basic,
    Boss,
}
[System.Serializable]
public class SoundEffect
{
    public SFXName name;
    public AudioClip audioClip;
}
[System.Serializable]
public class BGMusic
{
    public BGMName name;
    public AudioClip audioClip;
}
public enum Volume
{
    Master,
    BGM,
    SFX,
}
public class SoundManager : Singleton<SoundManager>
{
    public AudioMixer mixer;
    public List<SoundEffect> soundEffects;
    public List<BGMusic> bgms;
    public Dictionary<Volume, float> volumeValues = new();

    public AudioSource bgm;

    private void Start()
    {
        MasterVolumeChange(0.1f);
        volumeValues.Add(Volume.Master, 1f);
        volumeValues.Add(Volume.BGM, 1f);
        volumeValues.Add(Volume.SFX, 1f);
    }
    public void MasterVolumeChange(float value)
    {
        mixer.SetFloat("Master", Mathf.Log10(value) * 20);
        volumeValues[Volume.Master] = value;
    }
    public void BGMVolumeChange(float value)
    {
        mixer.SetFloat("BGM", Mathf.Log10(value) * 20);
        volumeValues[Volume.BGM] = value;
    }
    public void SFXVolumeChange(float value)
    {
        mixer.SetFloat("SFX", Mathf.Log10(value) * 20);
        volumeValues[Volume.SFX] = value;
    }

    public void SFXPlay(SFXName name,Vector2 pos)
    {
        StartCoroutine(CoSFXPlay(FindSFX(name), pos));
    }
    private AudioClip FindSFX(SFXName name)
    {
        foreach(SoundEffect effect in soundEffects)
        {
            if (effect.name == name)
                return effect.audioClip;
        }
        Debug.LogError("SFX를 찾을 수 없습니다.");
        return null;
    }
    IEnumerator CoSFXPlay(AudioClip clip,Vector2 pos)
    {
        GameObject sfx = PoolManager.Instance.Get(PoolType.SFX);
        sfx.transform.position = pos;
        
        sfx.GetComponent<AudioSource>().clip = clip;
        sfx.GetComponent<AudioSource>().outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0];
        sfx.GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(clip.length);

        sfx.SetActive(false);
    }
    public void SetBgm(BGMName name)
    {
        bgm.clip = FindBGM(name);
        bgm.Play();
    }
    private AudioClip FindBGM(BGMName name)
    {
        foreach (BGMusic bgm in bgms)
        {
            if (bgm.name == name)
                return bgm.audioClip;
        }
        Debug.LogError("BGM을 찾을 수 없습니다.");
        return null;
    }
}
