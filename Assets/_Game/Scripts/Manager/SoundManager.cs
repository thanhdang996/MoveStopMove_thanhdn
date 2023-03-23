using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] private AudioMixer audioMixer;

    [SerializeField] private List<AudioSource> listMusicBG;
    [SerializeField] private List<AudioSource> listEffect;

    private void Start()
    {
        Init(5f, 5f);
        //PlaySoundMusic("bg-music");
    }


    public void Init(float musicVolume, float sfxvolume)
    {
        audioMixer.SetFloat(Constant.VOLUME_MY_MUSIC, musicVolume);
        audioMixer.SetFloat(Constant.VOLUME_MY_SFX, sfxvolume);
    }

    public void PlaySoundMusic(string name)
    {
        for (int i = 0; i < listMusicBG.Count; i++)
            if (listMusicBG[i].clip.name == name)
                listMusicBG[i].Play();
    }
    public void StopSoundMusic(string name)
    {
        for (int i = 0; i < listMusicBG.Count; i++)
            if (listMusicBG[i].clip.name == name)
                listMusicBG[i].Stop();
    }

    public void PlaySoundSFX(string name)
    {
        for (int i = 0; i < listEffect.Count; i++)
            if (listEffect[i].clip.name == name)
                listEffect[i].Play();
    }
    public void StopSoundSFX(string name)
    {
        for (int i = 0; i < listEffect.Count; i++)
            if (listEffect[i].clip.name == name)
                listEffect[i].Stop();
    }

    public void ChangeVolumeMusic(float volume)
    {
        audioMixer.SetFloat(Constant.VOLUME_MY_MUSIC, volume);
    }

    public void ChangeVolumeSFX(float volume)
    {
        audioMixer.SetFloat(Constant.VOLUME_MY_SFX, volume);
    }


}
