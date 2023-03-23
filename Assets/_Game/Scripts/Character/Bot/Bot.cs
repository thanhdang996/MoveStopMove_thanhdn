using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class Bot : Character
{
    [SerializeField] private GameObject aimedGO;

    private NavMeshAgent navMeshAgent;
    private IState currentState;

    [SerializeField] Vector3 currentPosTarget;
    public Vector3 CurrentPosTarget { get => currentPosTarget; set => currentPosTarget = value; }

    [SerializeField] private GameObject indicatorGO;
    public GameObject IndicatorGO { get => indicatorGO; set => indicatorGO = value; }


    protected override void Awake()
    {
        base.Awake();
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.stoppingDistance = 3;
    }

    public void GetRandomPosTargetInMap()
    {
        List<SpawnPosTrigger> listPos = LevelManager.Instance.CurrentLevel.ListSpawnPosTrigger;
        currentPosTarget = listPos[Random.Range(0, listPos.Count)].TF.position;
        navMeshAgent.SetDestination(currentPosTarget);
    }

    public bool IsReachTarget()
    {
        return Vector3.Distance(CurrentPosTarget, TF.position) < 5f;
    }

    public override void OnInit()
    {
        base.OnInit();
        int levelPlayer = GameManager.Instance.CurrentPlayer.LevelCharacter;
        levelCharacter = Random.Range(levelPlayer, levelPlayer + 3);

        StartMoving();
        navMeshAgent.ResetPath();
        GetRandomPosTargetInMap();
        ChangeState(new PatrolState());

        LevelManager.Instance.CurrentLevel.ListBotCurrent.Add(this);
        IndicatorGO = ObjectPooling.Instance.GetGameObject(MyPoolType.Indicator);
    }


    public override void OnDespawn()
    {
        StopMoving();

        base.OnDespawn();
        UIManager.Instance.UpdateTotalEnemyAndText();
        CheckConditionEnemyRemainToSpawn();

        LevelManager.Instance.CurrentLevel.ListBotCurrent.Remove(this);
        ObjectPooling.Instance.ReturnGameObject(IndicatorGO, MyPoolType.Indicator);
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
        aimedGO.SetActive(true);
    }

    public void HideAim()
    {
        aimedGO.SetActive(false);
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
            float distance = Vector3.Distance(ListTarget[i].TF.position, TF.position);
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
            ChangeAnim(Constant.ANIM_DEATH);
            return;
        }

        if (!navMeshAgent.isStopped)
        {
            ChangeAnim(Constant.ANIM_RUN);
            return;
        }
        if (IsAttack)
        {
            ChangeAnim(Constant.ANIM_ATTACK);
            return;
        }
        if (navMeshAgent.isStopped)
        {
            ChangeAnim(Constant.ANIM_IDLE);
            return;
        }

    }

    public void CreateWeaponBotBaseOnPlayerOwner()
    {
        List<int> listWeaponOwner = GameManager.Instance.Data.WeaponOwner;
        foreach (int weapon in listWeaponOwner)
        {
            Instantiate(weaponSO.propWeapons[weapon].weaponAvatarPrefabs, weaponHolderTF).SetActive(false);
        }
    }

    public void ActiveRandomWeapon()
    {
        List<int> listWeaponOwner = GameManager.Instance.Data.WeaponOwner;
        int indexWeaponType = listWeaponOwner[Random.Range(0, listWeaponOwner.Count)];
        int getIndexInWeaponHolder = listWeaponOwner.IndexOf(indexWeaponType);
        currentWeaponType = (WeaponType)indexWeaponType;

        // neu player moi add weapon thi instantiate
        if (getIndexInWeaponHolder > weaponHolderTF.childCount - 1)
        {
            // trong weaponHolder tao 1 weapon moi o vi tri cuoi, neu nhu getIndexInWeaponHolder vuot qua childCount-1
            Instantiate(weaponSO.propWeapons[indexWeaponType].weaponAvatarPrefabs, weaponHolderTF).SetActive(false);
        }
        currentWeaponAvaGO = weaponHolderTF.GetChild(getIndexInWeaponHolder).gameObject;
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
        ObjectPooling.Instance.ReturnGameObject(gameObject, MyPoolType.Bot);
        LevelManager.Instance.CurrentLevel.RandomOneBot();
    }
    private void ReturnBotToPool()
    {
        ObjectPooling.Instance.ReturnGameObject(gameObject, MyPoolType.Bot);
    }
}
