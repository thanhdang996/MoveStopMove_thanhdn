using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSetting : MonoBehaviour
{
    [SerializeField] Slider bgMusicSilder;
    [SerializeField] Slider sfxSilder;

    private void Awake()
    {
        bgMusicSilder.onValueChanged.AddListener(SoundManager.Instance.SetMusicVolume);
        sfxSilder.onValueChanged.AddListener(SoundManager.Instance.SetSFXVolume);
    }

    public void LoadValueMusic()
    {
        bgMusicSilder.value = DataManager.Instance.Data.BGMusicVolume;
        sfxSilder.value = DataManager.Instance.Data.SFXVolume;

        SoundManager.Instance.SetMusicVolume(bgMusicSilder.value);
        SoundManager.Instance.SetSFXVolume(sfxSilder.value);
    }

    private void OnDisable()
    {
        DataManager.Instance.Data.SetBGVolumeToData(bgMusicSilder.value);
        DataManager.Instance.Data.SetSFXVolumeToData(sfxSilder.value);
        DataManager.Instance.SaveData();
    }
}
