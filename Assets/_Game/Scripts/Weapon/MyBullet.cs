using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyBullet : Weapon
{
    public override void Launch()
    {
        base.Launch();
        //Invoke(nameof(OnDespawn), timeStop);
        StartCoroutine(DelayStop());
    }

    private IEnumerator DelayStop()
    {
        yield return new WaitForSeconds(timeStop);
        OnDespawn();
    }


    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag(Constant.TAG_OBSTACLE))
    //    {
    //        StopAllCoroutines();
    //        OnHitObstacle();
    //        return;
    //    }

    //    Character character = Cache.GetCharacter(other);
    //    if (character != null)
    //    {
    //        if (character == SourceFireCharacter) return;
    //        SoundManager.Instance.PlaySoundSFX(SoundType.WeaponHit, TF.position);
    //        character.OnDespawn();
    //        SourceFireCharacter.ChangeScalePerKillAndIncreaseLevel();

    //        StopAllCoroutines();
    //        OnDespawn();
    //    }
    //}
}
