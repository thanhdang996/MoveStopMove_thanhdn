using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSetting : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] Slider bgMusicSilder;
    [SerializeField] Slider sfxSilder;

    private void Awake()
    {
        bgMusicSilder.onValueChanged.AddListener(SetMusicVolume);
        sfxSilder.onValueChanged.AddListener(SetSFXVolume);
    }

    public void LoadValueMusic()
    {
        bgMusicSilder.value = DataManager.Instance.Data.BGMusicVolume;
        sfxSilder.value = DataManager.Instance.Data.SFXVolume;

        SetMusicVolume(bgMusicSilder.value);
        SetSFXVolume(sfxSilder.value);
    }

    private void OnDisable()
    {
        DataManager.Instance.Data.SetBGVolumeToData(bgMusicSilder.value);
        DataManager.Instance.Data.SetSFXVolumeToData(sfxSilder.value);
        DataManager.Instance.SaveData();
    }

    private void SetMusicVolume(float value)
    {
        audioMixer.SetFloat(Constant.VOLUME_MY_MUSIC, Mathf.Log10(value) * 20);
    }

    private void SetSFXVolume(float value)
    {
        audioMixer.SetFloat(Constant.VOLUME_MY_SFX, Mathf.Log10(value) * 20);
    }
}
