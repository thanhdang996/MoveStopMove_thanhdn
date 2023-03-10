using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType {Axe, Boomerang, Hammer}
public class Weapon : MonoBehaviour
{
    protected float timeStop;
    [SerializeField] protected float speed = 20;

    protected Character sourceFireCharacter;
    public Character SourceFireCharacter { get => sourceFireCharacter; set => sourceFireCharacter = value; }
    protected Rigidbody rb;

    [SerializeField] private WeaponType weaponType;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public virtual void Launch()
    {
        timeStop = SourceFireCharacter.GetTimeSecondToStopWeapon(speed);
        rb.AddForce(speed * transform.forward, ForceMode.Impulse);
    }

    protected virtual void OnDespawn()
    {
        rb.velocity = Vector3.zero;
        SourceFireCharacter = null;
        ObjectPooling.Instance.ReturnGameObject(gameObject, Constant.ConvertWeaponTypeeToObjectType(weaponType));
    }
}
