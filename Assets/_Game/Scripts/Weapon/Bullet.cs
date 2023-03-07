using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Weapon
{
    private float timeDestroy;
    public void MoveAndAutoDestroy()
    {
        timeDestroy = SourceFireCharacter.GetTimeSecondToStopWeapon(speed);
        rb.AddForce(speed * transform.forward, ForceMode.Impulse);
        Invoke(nameof(OnDespawn), timeDestroy);
    }

    private void OnDespawn()
    {
        rb.velocity= Vector3.zero;
        SourceFireCharacter = null;
        ObjectPooling.Instance.ReturnGameObject(gameObject, ObjectType.Bullet);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Character character))
        {
            if (character == SourceFireCharacter) return;
            Vector3 oriScale = SourceFireCharacter.transform.localScale;
            SourceFireCharacter.transform.localScale = oriScale + (Vector3.one * SourceFireCharacter.ScalePerKill);
            character.OnDespawn();

            CancelInvoke(nameof(OnDespawn));
            OnDespawn();
        }
    }
}
