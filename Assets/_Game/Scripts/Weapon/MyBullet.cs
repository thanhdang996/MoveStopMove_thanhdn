using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyBullet : Weapon
{
    public override void Launch()
    {
        base.Launch();
        Invoke(nameof(OnDespawn), timeStop);
    }


    private void OnTriggerEnter(Collider other)
    {
        Character character = Cache.GetCharacter(other);
        if (character != null)
        {
            if (character == SourceFireCharacter) return;

            character.OnDespawn();
            SourceFireCharacter.ChangeScalePerKillAndIncreaseLevel();

            CancelInvoke(nameof(OnDespawn));
            OnDespawn();
        }
    }
}
