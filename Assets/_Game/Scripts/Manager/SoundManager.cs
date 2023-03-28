using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum SoundType { ThrowWeapon, Dead, WeaponCollider, WeaponHit, Win, Lose}

[System.Serializable]
public class SoundAudioClip
{
    public SoundType soundType;
    public AudioClip[] audioClips;
}

public class SoundManager : Singleton<SoundManager>
{
    [Header("Setting")]
    [SerializeField] private AudioMixer audioMixer;


    [Header("Audio Source")]
    [SerializeField] private AudioSource audioSourceMusicBG;
    [SerializeField] private AudioSource audioSourceSFX2D;

    [Header("All Clip")]
    [SerializeField] private SoundAudioClip[] soundAudioClips;


    public void PlayBGSoundMusic()
    {
        audioSourceMusicBG.Play();
    }
    public void StopBGSoundMusic()
    {
        audioSourceMusicBG.Stop();
    }


    // random soundType sfx
    public void PlaySoundSFX3D(SoundType soundType, Vector3 pos)
    {
        for (int i = 0; i < soundAudioClips.Length; i++)
        {
            if (soundAudioClips[i].soundType == soundType)
            {
                SFX sfx = SimplePool.Spawn<SFX>(PoolType.SFX);
                sfx.TF.position = pos;

                int randomIndex = Random.Range(0, soundAudioClips[i].audioClips.Length);
                sfx.AudioSource.clip = soundAudioClips[i].audioClips[randomIndex];
                sfx.PlaySFX();
                return;
            }
        }
    }

    // chi dinh soundType dua vao index
    public void PlaySoundSFX3D(SoundType soundType, Vector3 pos, int index)
    {
        for (int i = 0; i < soundAudioClips.Length; i++)
        {
            if (soundAudioClips[i].soundType == soundType)
            {
                SFX sfx = SimplePool.Spawn<SFX>(PoolType.SFX);
                sfx.TF.position = pos;

                sfx.AudioSource.clip = soundAudioClips[i].audioClips[index];
                sfx.PlaySFX();
                return;
            }
        }
    }

    public void PlaySoundSFX2D(SoundType soundType, int index = 0)
    {
        for (int i = 0; i < soundAudioClips.Length; i++)
        {
            if (soundAudioClips[i].soundType == soundType)
            {
                audioSourceSFX2D.clip = soundAudioClips[i].audioClips[index];
                audioSourceSFX2D.Play();
                return;
            }
        }
    }
}
