using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SoundEffect
{
    public string name;
    public AudioClip audioClip;
}
public class SFX : MonoBehaviour
{
    public List<SoundEffect> soundEffects = new List<SoundEffect>();
    public AudioClip GetAudioClip(string name)
    {
        AudioClip res = null;
        foreach(SoundEffect effect in soundEffects)
        {
            if (effect.name == name) res = effect.audioClip;
            else continue;
        }
        return res;
    }
}
