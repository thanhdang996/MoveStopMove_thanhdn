using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Boomerang : Weapon
{
    [SerializeField] private float speedReturn = 20f;

    public override void Launch()
    {
        base.Launch();
        StartCoroutine(Return());
    }


    private IEnumerator Return()
    {
        yield return new WaitForSeconds(timeStop);
        rb.velocity = Vector3.zero;


        while (Vector3.Distance(transform.position, SourceFireCharacter.transform.position) > 3f)
        {
            transform.position = Vector3.MoveTowards(transform.position, SourceFireCharacter.transform.position, speedReturn * Time.deltaTime);
            yield return null;
        }
        OnDespawn();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Character character))
        {
            if (character == SourceFireCharacter) return;

            character.OnDespawn();
            SourceFireCharacter.ChangeScalePerKill();

            StopCoroutine(Return());
            OnDespawn();
        }
    }
}
