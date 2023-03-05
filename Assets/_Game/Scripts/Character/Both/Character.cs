using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private CapsuleCollider capsuleCollider;

    [SerializeField] protected Transform firePoint;
    [SerializeField] private GameObject attackRange;

    [SerializeField] private List<Character> charactersTargeted = new List<Character>();
    public List<Character> CharactersTargeted { get => charactersTargeted; set => charactersTargeted = value; }


    [SerializeField] private Character targetNearest;
    public Character TargetNearest { get => targetNearest; set => targetNearest = value; }


    [SerializeField] protected float timeResetAttack = 1f;
    [SerializeField] private bool isAttack;
    public bool IsAttack { get => isAttack; set => isAttack = value; }

    protected float timeDelayDead = 4f;
    [SerializeField] private bool isDead;
    public bool IsDead { get => isDead; private set => isDead = value; }

    // anim
    private string currentAnimName;
    [SerializeField] private Animator anim;

    //id
    [SerializeField] private int id;
    public int Id { get => id; set => id = value; }



    protected virtual void Awake()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
    }

    public virtual void OnInit()
    {
        capsuleCollider.enabled = true;
        attackRange.SetActive(true);
        IsDead = false;
        IsAttack = false;
    }

    public virtual void OnDespawn()
    {
        IsDead = true;
        capsuleCollider.enabled = false;
        attackRange.SetActive(false);
        targetNearest = null;
        // remove all tartget in other character
        foreach (Character target in CharactersTargeted)
        {
            target.CharactersTargeted.Remove(this);
        }
        CharactersTargeted.Clear();

        SpawnerManager.Instance.RandomOneBot();
        Invoke(nameof(DelayDead), timeDelayDead);
    }

    private void DelayDead()
    {
        ObjectPooling.Instance.ReturnGameObject(gameObject, ObjectType.Bot);
    }

    public virtual void AttackCharacter()
    {
        IsAttack = true;
        //RotateToCharacter();
        //Attack();
        Invoke(nameof(ResetAttack), timeResetAttack);
    }


    public virtual void Attack() // call from animEvent
    {
        GameObject bulletObj = ObjectPooling.Instance.GetGameObject(ObjectType.Bullet);
        Bullet bullet = bulletObj.GetComponent<Bullet>();
        bullet.SourceFireCharacter = this;
        bullet.transform.position = firePoint.position;
        bullet.transform.rotation = firePoint.rotation;
        bullet.MoveAndAutoDestroy();
    }
    public void ResetAttack()
    {
        IsAttack = false;
    }

    // xoay aim toi TargetNearest
    public virtual void RotateToCharacter() // call from animEvent
    {
        if (TargetNearest == null) return;
        Vector3 dir = (TargetNearest.transform.position - transform.position).normalized;
        dir.y = 0;
        transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
    }
    protected void ChangeAnim(string animName)
    {
        if (currentAnimName != animName)
        {
            if (!string.IsNullOrEmpty(currentAnimName))
                anim.ResetTrigger(currentAnimName);
            currentAnimName = animName;
            anim.SetTrigger(currentAnimName);
        }
    }
}
