using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : GameUnit
{
    protected float timeStop;
    [SerializeField] protected float speed = 20;

    protected Character sourceFireCharacter;
    public Character SourceFireCharacter { get => sourceFireCharacter; set => sourceFireCharacter = value; }
    protected Rigidbody rb;

    [SerializeField] protected WeaponType weaponType;

    [SerializeField] protected RotationSelf rotationSelf;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public virtual void Launch()
    {
        rotationSelf.enabled = true;
        timeStop = SourceFireCharacter.GetTimeSecondToStopWeapon(speed);
        rb.AddForce(speed * TF.forward, ForceMode.Impulse);
    }

    protected void OnHitObstacle()
    {
        SoundManager.Instance.PlaySoundSFX3D(SoundType.WeaponCollider, TF.position, 0);
        rb.velocity = Vector3.zero;
        rotationSelf.enabled = false;
        Invoke(nameof(OnDespawn), 0.5f);
    }

    protected virtual void OnDespawn()
    {
        rb.velocity = Vector3.zero;
        SourceFireCharacter = null;
        SimplePool.Despawn(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constant.TAG_OBSTACLE))
        {
            StopAllCoroutines();
            OnHitObstacle();
            return;
        }

        Character character = Cache.GetCharacter(other);
        if (character != null)
        {
            if (character == SourceFireCharacter) return;
            SoundManager.Instance.PlaySoundSFX3D(SoundType.WeaponHit, TF.position, (int)weaponType);
            character.OnDespawn();
            SourceFireCharacter.ChangeScalePerKillAndIncreaseLevel();

            StopAllCoroutines();
            OnDespawn();
        }
    }
}
