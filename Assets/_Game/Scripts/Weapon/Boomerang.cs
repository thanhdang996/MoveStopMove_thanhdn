using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Boomerang : Weapon
{
    [SerializeField] private float speedReturn = 15f;

    public override void Launch()
    {
        base.Launch();
        StartCoroutine(Return());
    }


    private IEnumerator Return()
    {
        yield return new WaitForSeconds(timeStop);
        rb.velocity = Vector3.zero;


        while (Vector3.Distance(TF.position, SourceFireCharacter.TF.position) > 3f)
        {
            TF.position = Vector3.MoveTowards(TF.position, SourceFireCharacter.TF.position, speedReturn * Time.deltaTime);
            yield return null;
        }
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
