using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] protected Transform firePoint;

    [SerializeField] private List<Character> charactersTargeted = new List<Character>();
    public List<Character> CharactersTargeted { get => charactersTargeted; set => charactersTargeted = value; }


    [SerializeField] private Character targetNearest;
    public Character TargetNearest { get => targetNearest; set => targetNearest = value; }


    [SerializeField] protected float timeResetAttack = 1f;
    [SerializeField] private bool isAttack;
    public bool IsAttack { get => isAttack; set => isAttack = value; }

    [SerializeField] protected float timeDelayDead = 2f;
    [SerializeField] private bool isDead; // dang thua thuoc tinh
    public bool IsDead { get => isDead; private set => isDead = value; }

    protected virtual void Awake()
    {

    }

    public virtual void OnInit()
    {

        IsDead = false;
        IsAttack = false;
    }

    public virtual void OnDespawn()
    {
        IsDead = true;
        foreach (Character target in CharactersTargeted)
        {
            target.CharactersTargeted.Remove(this);
        }
        Invoke(nameof(DelayDead), timeDelayDead);
    }

    private void DelayDead()
    {
        CharactersTargeted.Clear();
        gameObject.SetActive(false);
    }

    public virtual void AttackCharacter()
    {
        IsAttack = true;
        RotateToCharacter();
        Attack();
        Invoke(nameof(ResetAttack), timeResetAttack);
    }
    protected virtual void Attack()
    {
        GameObject bulletObj = ObjectPooling.Instance.GetGameObject();
        Bullet bullet = bulletObj.GetComponent<Bullet>();
        bullet.SourceFireCharacter = this;
        bullet.transform.position = firePoint.position;
        bullet.transform.rotation = firePoint.rotation;
        bullet.MoveAndAutoDestroy();
    }
    private void ResetAttack()
    {
        IsAttack = false;
    }

    protected virtual void RotateToCharacter()
    {
        Vector3 dir = (TargetNearest.transform.position - transform.position).normalized;
        dir.y = 0;
        transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
    }

    public virtual void CheckTargetNearest()
    {
        
    }
}
