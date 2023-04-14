using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class Bot : Character
{
    public event System.Action<Bot> OnDeath;
    [SerializeField] private GameObject aimedGO;
    [SerializeField] private ColorSO colorSO;

    private NavMeshAgent navMeshAgent;
    private IState currentState;

    [SerializeField] Vector3 currentPosTarget;
    public Vector3 CurrentPosTarget { get => currentPosTarget; set => currentPosTarget = value; }

    [SerializeField] private Indicator indicator;
    public Indicator Indicator { get => indicator; set => indicator = value; }


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
        int levelPlayer = LevelManager.Instance.CurrentPlayer.LevelCharacter;
        levelCharacter = Random.Range(levelPlayer, levelPlayer + 5);

        navMeshAgent.enabled = true; // fix when next level not correct pos
        navMeshAgent.ResetPath();
        StopMoving();

        indicator = SimplePool.Spawn<Indicator>(PoolType.Indicator);
        indicator.SetTextLevel(levelCharacter);
        indicator.SetColor(currentColor);
        indicator.HideIndicator(); // fix loi ruoi bay indicator khi moi sinh bot 


        canvasShowLevel.SetTextLevel(levelCharacter);


        Destroy(currentHatAvaGOAttach);
        Destroy(currentShieldAvaGOAttach);


        RandomHatBot();
        RandomPantBot();
        RandomShield();

        LevelManager.Instance.ListBotCurrent.Add(this);
    }


    public override void OnDespawn()
    {
        StopMoving();
        navMeshAgent.enabled = false; // fix when next level not correct pos
        base.OnDespawn();

        LevelManager.Instance.CurrentLevel.MinusOneTotalEnemy();
        UIManager.Instance.GetUI<UICGamePlay>().UI_UpdateEnemeRemainText();

        SimplePool.Despawn(indicator);
        OnDeath?.Invoke(this);

        LevelManager.Instance.ListBotCurrent.Remove(this);
    }


    private void Update()
    {
        HandleAnim();
        if (!IsDead && GameManager.Instance.IsState(GameState.GamePlay))
        {
            currentState?.OnExecute(this);
        }
    }

    public void RandomColorBotAndSetCanvasLevelColor(int index)
    {
        Material colorMat = colorSO.propsColors[index].mat;
        currentSkinSet.material = colorMat;
        currentColor = colorMat.color;

        canvasShowLevel.SetColor(currentColor);
    }
    private void RandomHatBot()
    {
        int randomIndex = Random.Range(-1, hatSO.propsHats.Length);
        if (randomIndex == -1) return;
        currentHatAvaGOAttach = Instantiate(hatSO.propsHats[randomIndex].avatarPrefab, hatHolderTF).gameObject;
    }
    private void RandomPantBot()
    {
        int randomIndex = Random.Range(-1, pantSO.propsPants.Length);
        if (randomIndex == -1) return;

        currentPantMatAttach = pantSO.propsPants[randomIndex].mat;
        currentSkinPant.material = currentPantMatAttach;
    }
    private void RandomShield()
    {
        int randomIndex = Random.Range(-1, shieldSO.propsShields.Length);
        if (randomIndex == -1) return;

        currentShieldAvaGOAttach = Instantiate(shieldSO.propsShields[randomIndex].avatarPrefab, shieldHolderTF).gameObject;
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
        List<WeaponType> listWeaponOwner = DataManager.Instance.Data.ListWeaponOwner;
        foreach (WeaponType weapon in listWeaponOwner)
        {
            Instantiate(weaponSO.propWeapons[(int)weapon].weaponAvatarPrefabs, weaponHolderTF).SetActive(false);
        }
    }

    public void ActiveRandomWeapon()
    {
        List<WeaponType> listWeaponOwner = DataManager.Instance.Data.ListWeaponOwner;
        WeaponType getRandomWeaponTypeInData = listWeaponOwner[Random.Range(0, listWeaponOwner.Count)];
        int getIndexInWeaponHolder = listWeaponOwner.IndexOf(getRandomWeaponTypeInData);
        if (getIndexInWeaponHolder == -1)
        {
            print("Ban dang ko so huu vu ki do " + (getRandomWeaponTypeInData).ToString());
            return;
        }

        currentWeaponAvaGO = weaponHolderTF.GetChild(getIndexInWeaponHolder).gameObject;
        ActiveCurrentWeapon();

        currentWeaponType = getRandomWeaponTypeInData;
    }

    public void CreateNewWeaponBasePlayerJustAdd(WeaponType indexWeaponOnShop)
    {
        Instantiate(weaponSO.propWeapons[(int)indexWeaponOnShop].weaponAvatarPrefabs, weaponHolderTF).SetActive(false);
    }

    public override void ChangeScalePerKillAndIncreaseLevel()
    {
        base.ChangeScalePerKillAndIncreaseLevel();
        indicator.SetTextLevel(levelCharacter);
    }
}