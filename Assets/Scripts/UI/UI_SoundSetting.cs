using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SoundSetting : MonoBehaviour
{
    [SerializeField] Slider masterVolume;
    [SerializeField] Slider bgmVolume;
    [SerializeField] Slider sfxVolume;
    private void Awake()
    {
        masterVolume.onValueChanged.AddListener(MasterVolumeChange);
        bgmVolume.onValueChanged.AddListener(BGMVolumeChange);
        sfxVolume.onValueChanged.AddListener(SFXVolumeChange);
    }
    private void OnEnable()
    {
        masterVolume.value = SoundManager.Instance.volumeValues[Volume.Master];
        bgmVolume.value = SoundManager.Instance.volumeValues[Volume.BGM];
        sfxVolume.value = SoundManager.Instance.volumeValues[Volume.SFX];
    }
    public void MasterVolumeChange(float value)
    {
        SoundManager.Instance.MasterVolumeChange(value);
    }
    public void BGMVolumeChange(float value)
    {
        SoundManager.Instance.BGMVolumeChange(value);
    }
    public void SFXVolumeChange(float value)
    {
        SoundManager.Instance.SFXVolumeChange(value);
    }
}
