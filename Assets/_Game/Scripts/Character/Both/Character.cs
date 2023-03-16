using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Character : MonoBehaviour
{
    private CapsuleCollider capsuleCollider;

    [SerializeField] protected Transform firePoint;

    [SerializeField] private GameObject avatarNewGO;
    [SerializeField] private GameObject attackRangeGO;


    // target
    [SerializeField] private List<Character> listTarget = new List<Character>();
    public List<Character> ListTarget { get => listTarget; set => listTarget = value; }


    [SerializeField] private List<Character> listBeAimed = new List<Character>();
    public List<Character> ListBeAimed { get => listBeAimed; set => listBeAimed = value; }

    [SerializeField] private List<SpawnPosTrigger> listInSpawnPos = new List<SpawnPosTrigger>();
    public List<SpawnPosTrigger> ListInSpawnPos { get => listInSpawnPos; set => listInSpawnPos = value; }


    [SerializeField] private Character targetNearest;
    public Character TargetNearest { get => targetNearest; set => targetNearest = value; }


    // prop
    [SerializeField] protected float timeResetAttack = 1f;
    [SerializeField] private bool isAttack;
    public bool IsAttack { get => isAttack; set => isAttack = value; }

    protected float timeDelayRespawn = 4f;
    [SerializeField] private bool isDead;
    public bool IsDead { get => isDead; private set => isDead = value; }

    private const float scalePerKill = 0.05f;


    // anim
    private string currentAnimName;
    [SerializeField] private Animator anim;

    //id
    [SerializeField] private int id;
    public int Id { get => id; set => id = value; }

    //Current Weapon
    [SerializeField] protected WeaponSO weaponSO;
    [SerializeField] protected WeaponType currentWeaponType;
    public WeaponType CurrentWeaponType { get => currentWeaponType; set => currentWeaponType = value; }

    [SerializeField] protected Transform weaponHolder;
    protected GameObject currentWeaponAvatar;
    protected int attackRangeCurrentWeapon;
    [SerializeField] private Transform pointRangeWeapon;


    //Current Level Character
    [SerializeField] private int levelCharacter = 0;


    protected virtual void Awake()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
    }

    public virtual void OnInit()
    {
        capsuleCollider.enabled = true;
        currentWeaponAvatar.SetActive(true);
        attackRangeGO.SetActive(true);
        IsDead = false;
        IsAttack = false;
    }

    public void HandleAttackRangeBaseOnRangeWeapon()
    {
        attackRangeCurrentWeapon = weaponSO.ReturnAttackRangeOfWeapon(CurrentWeaponType);
        float scaleAdjust = attackRangeCurrentWeapon / 10f;
        attackRangeGO.transform.localScale = (Vector3.one * scaleAdjust) + (Vector3.one * scalePerKill * levelCharacter);
        avatarNewGO.transform.localScale = Vector3.one + (Vector3.one * scalePerKill * levelCharacter);
    }

    public virtual void OnDespawn()
    {
        IsDead = true;
        capsuleCollider.enabled = false;
        attackRangeGO.SetActive(false);
        currentWeaponAvatar.SetActive(false);
        targetNearest = null;
        RemoveAllTargetRefAfterDeath();

        Invoke(nameof(DelayRespawn), timeDelayRespawn);
    }

    private void RemoveAllTargetRefAfterDeath()
    {
        foreach (Character target in ListTarget)
        {
            target.ListTarget.Remove(this);
            target.ListBeAimed.Remove(this);
        }
        foreach (Character target in ListBeAimed)
        {
            target.ListBeAimed.Remove(this);
            target.ListTarget.Remove(this);
        }
        foreach (SpawnPosTrigger spawnPos in ListInSpawnPos)
        {
            spawnPos.ListCharacterInSpawnPos.Remove(this);
        }
        ListTarget.Clear();
        ListBeAimed.Clear();
        ListInSpawnPos.Clear();
    }

    protected virtual void DelayRespawn()
    {

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
        if (currentWeaponType == WeaponType.Hammer)
        {
            List<Character> listTmp = new List<Character>();
            foreach (Character target in ListTarget)
            {
                Vector3 dir = Vector3.Normalize(target.transform.position - transform.position);
                float res = Vector3.Dot(transform.forward, dir);
                if (res > 0.25f)
                {
                    listTmp.Add(target);
                }
            }

            foreach (Character character in listTmp)
            {
                ChangeScalePerKillAndIncreaseLevel();
                character.OnDespawn();
            }
            return;
        }

        // bullet and boomerang
        currentWeaponAvatar.SetActive(false);

        GameObject obj = ObjectPooling.Instance.GetGameObject(Constant.ConvertWeaponTypeeToObjectType(currentWeaponType));
        Weapon weapon = obj.GetComponent<Weapon>();
        weapon.SourceFireCharacter = this;
        weapon.transform.SetPositionAndRotation(firePoint.position, firePoint.rotation);
        weapon.transform.localScale = avatarNewGO.transform.localScale;
        weapon.Launch();
    }
    public void ResetAttack()
    {
        if (isDead) return;
        IsAttack = false;
        currentWeaponAvatar.SetActive(true);
    }

    // xoay aim toi TargetNearest
    public virtual void RotateToCharacter() // call from animEvent
    {
        if (TargetNearest == null) return;
        Vector3 dir = (TargetNearest.transform.position - transform.position).normalized;
        dir.y = 0f;
        transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
    }


    public float GetTimeSecondToStopWeapon(float speedWeapon)
    {
        float distance = Vector3.Distance(transform.position, pointRangeWeapon.position);
        return distance / speedWeapon;
    }

    public virtual void ChangeScalePerKillAndIncreaseLevel()
    {
        Vector3 oriScaleAvatarNew = avatarNewGO.transform.localScale;
        avatarNewGO.transform.localScale = oriScaleAvatarNew + (Vector3.one * scalePerKill);

        Vector3 oriScaleAttackRange = attackRangeGO.transform.localScale;
        attackRangeGO.transform.localScale = oriScaleAttackRange + (Vector3.one * scalePerKill);

        levelCharacter++;
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
