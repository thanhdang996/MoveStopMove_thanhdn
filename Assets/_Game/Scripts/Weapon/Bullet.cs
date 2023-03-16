using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Weapon
{
    public override void Launch()
    {
        base.Launch();
        Invoke(nameof(OnDespawn), timeStop);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Character character))
        {
            if (character == SourceFireCharacter) return;

            character.OnDespawn();
            SourceFireCharacter.ChangeScalePerKillAndIncreaseLevel();

            CancelInvoke(nameof(OnDespawn));
            OnDespawn();
        }
    }
}
