using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private CapsuleCollider capsuleCollider;

    [SerializeField] protected Transform firePoint;
    [SerializeField] private GameObject attackRange;

    [SerializeField] private List<Character> listTarget = new List<Character>();
    public List<Character> ListTarget { get => listTarget; set => listTarget = value; }


    [SerializeField] private List<Character> listBeAimed = new List<Character>();
    public List<Character> ListBeAimed { get => listBeAimed; set => listBeAimed = value; }

    [SerializeField] private List<SpawnPosTrigger> listInSpawnPos = new List<SpawnPosTrigger>();
    public List<SpawnPosTrigger> ListInSpawnPos { get => listInSpawnPos; set => listInSpawnPos = value; }


    [SerializeField] private Character targetNearest;
    public Character TargetNearest { get => targetNearest; set => targetNearest = value; }


    [SerializeField] protected float timeResetAttack = 1f;
    [SerializeField] private bool isAttack;
    public bool IsAttack { get => isAttack; set => isAttack = value; }

    protected float timeDelayDead = 4f;
    [SerializeField] private bool isDead;
    public bool IsDead { get => isDead; private set => isDead = value; }

    [SerializeField] private float scalePerKill = 0.05f;
    public float ScalePerKill { get => scalePerKill; set => scalePerKill = value; }

    [SerializeField] private Transform pointRange;

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
        RemoveAllTargetRefAfterDeath();

        Invoke(nameof(DelayDead), timeDelayDead);
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

    protected virtual void DelayDead()
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

        // bullet, axe...
        //GameObject bulletObj = ObjectPooling.Instance.GetGameObject(ObjectType.Bullet);
        //Bullet bullet = bulletObj.GetComponent<Bullet>();
        //bullet.SourceFireCharacter = this;
        //bullet.transform.position = firePoint.position;
        //bullet.transform.rotation = firePoint.rotation;
        //bullet.transform.localScale = transform.localScale;
        //bullet.MoveAndAutoDestroy();

        // boomerang
        GameObject boomerangObj = ObjectPooling.Instance.GetGameObject(ObjectType.Boomerang);
        Boomerang boomerang = boomerangObj.GetComponent<Boomerang>();
        boomerang.SourceFireCharacter = this;
        boomerang.transform.position = firePoint.position;
        boomerang.transform.rotation = firePoint.rotation;
        boomerang.transform.localScale = transform.localScale;
        boomerang.MoveAndAutoDestroy();


        // Hammer
        //if (TargetNearest == null) return;
        //List<Character> listTmp = new List<Character>();
        //foreach (Character target in ListTarget)
        //{
        //    Vector3 dir = Vector3.Normalize(target.transform.position - transform.position);
        //    float res = Vector3.Dot(transform.forward, dir);
        //    if (res > 0.25f)
        //    {
        //        listTmp.Add(target);
        //    }
        //}

        //foreach (Character item in listTmp)
        //{
        //    if (item is Player) continue;
        //    item.OnDespawn();
        //}

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
        dir.y = 0f;
        transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
    }

    public float GetTimeSecondToStopWeapon(float speedWeapon)
    {
        float distance = Vector3.Distance(transform.position, pointRange.position);
        return distance / speedWeapon;
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
