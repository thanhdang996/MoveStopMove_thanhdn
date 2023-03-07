using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomerang : Weapon
{
    private float timeStop;
    [SerializeField] private float speedBoomerangReturn = 20f;


    public void MoveAndAutoDestroy()
    {
        timeStop = SourceFireCharacter.GetTimeSecondToStopWeapon(speed);
        rb.AddForce(speed * transform.forward, ForceMode.Impulse);
        StartCoroutine(Return());
    }

    private void OnDespawn()
    {
        rb.velocity = Vector3.zero;
        SourceFireCharacter = null;
        ObjectPooling.Instance.ReturnGameObject(gameObject, ObjectType.Boomerang);
    }

    private IEnumerator Return()
    {
        yield return new WaitForSeconds(timeStop);
        rb.velocity = Vector3.zero;


        while (Vector3.Distance(transform.position, SourceFireCharacter.transform.position) > 3f)
        {
            transform.position = Vector3.MoveTowards(transform.position, SourceFireCharacter.transform.position, speedBoomerangReturn * Time.deltaTime);
            yield return null;
        }
        OnDespawn();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Character character))
        {
            if (character == SourceFireCharacter) return;
            Vector3 oriScale = SourceFireCharacter.transform.localScale;
            SourceFireCharacter.transform.localScale = oriScale + (Vector3.one * SourceFireCharacter.ScalePerKill);
            character.OnDespawn();

            StopCoroutine(Return());
            OnDespawn();
        }
    }
}
