using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX : GameUnit
{
    [SerializeField] AudioSource audioSource; // setting 2d or 3d da chinh o unity
    public AudioSource AudioSource => audioSource;


    public void PlaySFX()
    {
        audioSource.Play();
        StartCoroutine(WaitPlayDone());

    }
    private IEnumerator WaitPlayDone()
    {
        yield return new WaitUntil(() => !audioSource.isPlaying);
        SimplePool.Despawn(this);
    }
}
