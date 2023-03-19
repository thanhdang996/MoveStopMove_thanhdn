using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bot : Character
{
    [SerializeField] private GameObject aimed;

    private NavMeshAgent navMeshAgent;
    private IState currentState;

    [SerializeField] Vector3 currentPosTarget;
    public Vector3 CurrentPosTarget { get => currentPosTarget; set => currentPosTarget = value; }

    protected override void Awake()
    {
        base.Awake();
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.stoppingDistance = 3;
    }

    public void GetRandomPosTargetInMap()
    {
        List<Transform> listPos = LevelManager.Instance.CurrentLevel.ListSpawnPos;
        currentPosTarget = listPos[Random.Range(0, listPos.Count)].position;
        navMeshAgent.SetDestination(currentPosTarget);
    }

    public bool IsReachTarget()
    {
        return Vector3.Distance(CurrentPosTarget, transform.position) < 5f;
    }

    public override void OnInit()
    {
        base.OnInit();
        int levelPlayer = GameManager.Instance.CurrentPlayer.LevelCharacter;
        levelCharacter = Random.Range(levelPlayer, levelPlayer + 3);
        StartMoving();
        navMeshAgent.ResetPath();
        GetRandomPosTargetInMap();

        //ChangeAnim("Idle");
        ChangeState(new PatrolState());
        HandleAttackRangeBaseOnRangeWeapon();
    }


    public override void OnDespawn()
    {
        StopMoving();

        base.OnDespawn();
        UIManager.Instance.UpdateTotalEnemyAndText();
        CheckConditionEnemyRemainToSpawn();
    }


    private void Update()
    {
        HandleAnim();
        if (!IsDead)
        {
            currentState?.OnExecute(this);
        }
    }


    public void StopMoving()
    {
        navMeshAgent.velocity = Vector3.zero;
        navMeshAgent.isStopped = true;
    }
    public void StartMoving()
    {
        navMeshAgent.isStopped = false;
    }

    public void ShowAim()
    {
        aimed.SetActive(true);
    }

    public void HideAim()
    {
        aimed.SetActive(false);
    }

    public void MoveToTarget()
    {
        navMeshAgent.SetDestination(currentPosTarget);
    }

    public void ChangeState(IState newState)
    {
        currentState?.OnExit(this);
        currentState = newState;
        currentState?.OnEnter(this);
    }

    public void CheckTargetNearest()
    {
        float minDistance = float.MaxValue;
        Character currentTargetTmp = null;

        for (int i = 0; i < ListTarget.Count; i++)
        {
            float distance = Vector3.Distance(ListTarget[i].transform.position, transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                currentTargetTmp = ListTarget[i];
            }
        }
        if (TargetNearest != currentTargetTmp)
        {
            TargetNearest = currentTargetTmp; // neu CharactersTargeted.Count = 0 thi TargetNearest gan bang null
        }
    }
    private void HandleAnim()
    {
        if (IsDead)
        {
            ChangeAnim("Death");
            return;
        }

        if (!navMeshAgent.isStopped)
        {
            ChangeAnim("Run");
            return;
        }
        if (IsAttack)
        {
            ChangeAnim("Attack");
            return;
        }
        if (navMeshAgent.isStopped)
        {
            ChangeAnim("Idle");
            return;
        }

    }

    public void CreateWeaponBotBaseOnPlayerOwner()
    {
        List<int> listWeaponOwner = GameManager.Instance.Data.WeaponOwner;
        foreach (int indexWeapon in listWeaponOwner)
        {
            Instantiate(weaponSO.propWeapons[indexWeapon].weaponAvatarPrefabs, weaponHolder).SetActive(false);
        }
    }

    public void ActiveRandomWeapon()
    {
        List<int> listWeaponOwner = GameManager.Instance.Data.WeaponOwner;
        int indexWeaponTypeHolder = listWeaponOwner[Random.Range(0, listWeaponOwner.Count)];
        int indexInWeaponHolder = listWeaponOwner.IndexOf(indexWeaponTypeHolder);
        // neu chua co thi instantiate
        currentWeaponType = (WeaponType)indexWeaponTypeHolder;

        if(indexInWeaponHolder > weaponHolder.childCount-1)
        {
            Instantiate(weaponSO.propWeapons[indexWeaponTypeHolder].weaponAvatarPrefabs, weaponHolder).SetActive(false);
        }
        currentWeaponAvatar = weaponHolder.GetChild(indexInWeaponHolder).gameObject;
    }


    private void CheckConditionEnemyRemainToSpawn()
    {
        if (LevelManager.Instance.CurrentLevel.TotalEnemy - LevelManager.Instance.CurrentLevel.NumberBotSpawnInit >= 0)
        {
            Invoke(nameof(SpawnBot), timeDelayRespawn);
        }
        else
        {
            Invoke(nameof(ReturnBotToPool), timeDelayRespawn);
        }
    }
    private void SpawnBot()
    {
        ObjectPooling.Instance.ReturnGameObject(gameObject, PoolType.Bot);
        LevelManager.Instance.CurrentLevel.RandomOneBot();
    }
    private void ReturnBotToPool()
    {
        ObjectPooling.Instance.ReturnGameObject(gameObject, PoolType.Bot);
    }
}
