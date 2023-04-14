using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Character : GameUnit
{
    private CapsuleCollider capsuleCollider;

    [SerializeField] protected Transform firePointTF;

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
    public float TimeDelayRespawn => timeDelayRespawn;

    [SerializeField] private bool isDead;
    public bool IsDead { get => isDead; private set => isDead = value; }

    private const float scalePerKill = 0.05f;


    // anim
    [SerializeField] private string currentAnimName;
    [SerializeField] private Animator anim;


    //Current Weapon
    [SerializeField] protected WeaponSO weaponSO;
    [SerializeField] protected WeaponType currentWeaponType;

    [SerializeField] protected Transform weaponHolderTF;
    protected GameObject currentWeaponAvaGO;
    protected int attackRangeCurrentWeapon;
    [SerializeField] private Transform pointRangeWeaponTF;


    // Current Hat
    [SerializeField] protected HatSO hatSO;
    [SerializeField] protected Transform hatHolderTF;
    public Transform HatHolderTF => hatHolderTF;
    [SerializeField] protected GameObject currentHatAvaGOAttach;

    // Current pant
    [SerializeField] protected PantSO pantSO;
    [SerializeField] protected Material currentPantMatAttach;
    [SerializeField] protected Material transparentMat;
    [SerializeField] protected SkinnedMeshRenderer currentSkinPant;


    // Current Shield 
    [SerializeField] protected ShieldSO shieldSO;
    [SerializeField] protected Transform shieldHolderTF;
    public Transform ShieldHolderTF => shieldHolderTF;
    [SerializeField] protected GameObject currentShieldAvaGOAttach;


    // Current Set
    [SerializeField] protected SetSO setSO;
    [SerializeField] protected Material currentSetMatAttach;
    [SerializeField] protected SkinnedMeshRenderer currentSkinSet;


    //wing and tail de trong
    [SerializeField] protected Transform wingHolderTF;
    public Transform WingHolderTF => wingHolderTF;
    [SerializeField] protected Transform tailHolderTF;
    public Transform TailHolderTF => tailHolderTF;

    [SerializeField] protected GameObject currentWingAvaGOAttach;
    [SerializeField] protected GameObject currentTailAvaGOAttach;


    //Current Level Character
    [SerializeField] protected int levelCharacter = 0;
    public int LevelCharacter => levelCharacter;
    [SerializeField] protected CanvasShowLevel canvasShowLevel;
    public CanvasShowLevel CanvasShowLevel => canvasShowLevel;

    // Particle 
    [SerializeField] ParticleSystem bloodExplosionPS;
    protected Color currentColor;


    protected virtual void Awake()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
    }

    public virtual void OnInit()
    {
        canvasShowLevel.gameObject.SetActive(true);
        capsuleCollider.enabled = true;
        attackRangeGO.SetActive(true);
        IsDead = false;
        IsAttack = false;
    }

    public void ResetLevelCharacter()
    {
        levelCharacter = 0;
    }

    public void DeActiveCurrentWeapon()
    {
        currentWeaponAvaGO.SetActive(false);
    }
    public void ActiveCurrentWeapon()
    {
        currentWeaponAvaGO.SetActive(true);
    }

    public void HandleAttackRangeBaseOnRangeWeapon()
    {
        attackRangeCurrentWeapon = weaponSO.ReturnAttackRangeOfWeapon(currentWeaponType);
        float scaleAdjust = attackRangeCurrentWeapon / 10f;

        // attack range base on weaponrange and level
        attackRangeGO.transform.localScale = (Vector3.one * scaleAdjust) + (levelCharacter * scalePerKill * Vector3.one);

        // avatar base on level
        avatarNewGO.transform.localScale = Vector3.one + (levelCharacter * scalePerKill * Vector3.one);
    }

    public virtual void OnDespawn()
    {
        SoundManager.Instance.PlaySoundSFX3D(SoundType.Dead, TF.position);
        //ParticlePool.Play(ParticleType.Hit, transform.position + Vector3.up * 2, Quaternion.identity);

        foreach (ParticleSystem ps in bloodExplosionPS.GetComponentsInChildren<ParticleSystem>())
        {
            MainModule main = ps.main;
            main.startColor = currentColor;
        }
        bloodExplosionPS.Play();
     
        canvasShowLevel.gameObject.SetActive(false);

        SetPropWhenDeath();
    }

    public virtual void SetPropWhenDeath()
    {
        CancelInvoke(); // khi chet cancel invoke het, vd nhu thoi gian ResetAttack
        IsDead = true;
        capsuleCollider.enabled = false;
        attackRangeGO.SetActive(false);
        currentWeaponAvaGO.SetActive(false);
        targetNearest = null;
        RemoveAllTargetRefAfterDeath();
    }

    private void RemoveAllTargetRefAfterDeath()
    {
        for (int i = 0; i < ListTarget.Count; i++)
        {
            ListTarget[i].ListTarget.Remove(this);
            ListTarget[i].listBeAimed.Remove(this);
        }
        for (int i = 0; i < ListBeAimed.Count; i++)
        {
            ListBeAimed[i].ListBeAimed.Remove(this);
            ListBeAimed[i].ListTarget.Remove(this);
        }
        for (int i = 0; i < ListInSpawnPos.Count; i++)
        {
            ListInSpawnPos[i].ListCharacterInSpawnPos.Remove(this);
        }

        //TODO: neu co the thi dung for thay foreach
        //foreach (Character target in ListTarget)
        //{
        //    target.ListTarget.Remove(this);
        //    target.ListBeAimed.Remove(this);
        //}
        //foreach (Character target in ListBeAimed)
        //{
        //    target.ListBeAimed.Remove(this);
        //    target.ListTarget.Remove(this);
        //}
        //foreach (SpawnPosTrigger spawnPos in ListInSpawnPos)
        //{
        //    spawnPos.ListCharacterInSpawnPos.Remove(this);
        //}
        ListTarget.Clear();
        ListBeAimed.Clear();
        ListInSpawnPos.Clear();
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
        if (TargetNearest == null) // fix loi ra khoi tam tan cong van danh dc
        {
            isAttack= false;
            return;
        }
        SoundManager.Instance.PlaySoundSFX3D(SoundType.ThrowWeapon, TF.position, (int)currentWeaponType);

        if (currentWeaponType == WeaponType.Hammer)
        {
            List<Character> listTmp = new List<Character>();
            foreach (Character target in ListTarget)
            {
                Vector3 dir = Vector3.Normalize(target.transform.position - TF.position);
                float res = Vector3.Dot(transform.forward, dir);
                if (res > 0.25f)
                {
                    listTmp.Add(target);
                }
            }

            foreach (Character character in listTmp)
            {
                character.OnDespawn();
                ChangeScalePerKillAndIncreaseLevel();
            }
            return;
        }

        // bullet and boomerang
        currentWeaponAvaGO.SetActive(false);

        //GameObject obj = ObjectPooling.Instance.GetGameObject(Constant.ConvertWeaponTypeeToObjectType(currentWeaponType));
        //Weapon weapon = obj.GetComponent<Weapon>();
        Weapon weapon = SimplePool.Spawn<Weapon>(Constant.ConvertWeaponTypeToObjectType(currentWeaponType));
        weapon.SourceFireCharacter = this;
        weapon.transform.SetPositionAndRotation(firePointTF.position, firePointTF.rotation);
        weapon.transform.localScale = avatarNewGO.transform.localScale;
        weapon.Launch();

    }
    public void ResetAttack()
    {
        //if (isDead) return;
        IsAttack = false;
        currentWeaponAvaGO.SetActive(true);
    }

    // xoay aim toi TargetNearest
    public virtual void RotateToCharacter() // call from animEvent
    {
        if (TargetNearest == null) // fix loi ra khoi tam tan cong van danh dc
        {
            isAttack= false;
            return;
        }
        Vector3 dir = (TargetNearest.transform.position - TF.position).normalized;
        dir.y = 0f;
        TF.rotation = Quaternion.LookRotation(dir, Vector3.up);
    }


    public float GetTimeSecondToStopWeapon(float speedWeapon)
    {
        float distance = Vector3.Distance(TF.position, pointRangeWeaponTF.position);
        return distance / speedWeapon;
    }

    public virtual void ChangeScalePerKillAndIncreaseLevel()
    {
        Vector3 oriScaleAvatarNew = avatarNewGO.transform.localScale;
        avatarNewGO.transform.localScale = oriScaleAvatarNew + (Vector3.one * scalePerKill);

        Vector3 oriScaleAttackRange = attackRangeGO.transform.localScale;
        attackRangeGO.transform.localScale = oriScaleAttackRange + (Vector3.one * scalePerKill);

        levelCharacter++;
        canvasShowLevel.SetTextLevel(levelCharacter);
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
