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
    Player_Dash,
    Player_Judgement,

    // Enemy
    Enemy_Hit,
    GoblinAx_Throw,

    // GoblinHero
    GoblinHero_Roar,
    GoblinHero_Jump,
    GoblinHero_Landing,
    GoblinHero_Attack,
    
    // UI 등 특수효과
    Button,
    Door,
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
    public List<SoundEffect> soundEffectList; // 인스펙터 창에서 사운드와 사운드네임을 설정하기 위함

    Dictionary<SFXName, AudioClip> soundEffects; // 빠른 찾기를 위한 Dictionary 클래스
    public List<BGMusic> bgms;
    public Dictionary<Volume, float> volumeValues;

    public AudioSource bgm;

    private void Start()
    {
        soundEffects = new();
        foreach(SoundEffect soundEffect in soundEffectList) // Dictionary 초기화
        {
            soundEffects.Add(soundEffect.name, soundEffect.audioClip);
        }
        volumeValues = new Dictionary<Volume, float>();
        volumeValues.Add(Volume.Master, 1f);
        volumeValues.Add(Volume.BGM, 1f);
        volumeValues.Add(Volume.SFX, 1f);
        MasterVolumeChange(0.1f);
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
        return soundEffects[name];
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
