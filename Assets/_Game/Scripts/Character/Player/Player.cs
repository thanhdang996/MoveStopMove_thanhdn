using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Player : Character
{
    public event Action OnDeath;

    private PlayerMovement playerMovement;
    public PlayerMovement PlayerMovement => playerMovement;

    private CameraFollow cam;

    [SerializeField] private List<PrefabItemShop> listContainHat;
    [SerializeField] private List<PrefabItemShop> listContainShield;


    private bool isWin;
    public bool IsWin { get => isWin; set => isWin = value; }


    protected override void Awake()
    {
        base.Awake();
        playerMovement = GetComponent<PlayerMovement>();
        cam = GetComponent<CameraFollow>();
    }

    public override void OnInit()
    {
        base.OnInit();
        playerMovement.enabled = true;
        IsWin = false;
    }

    public override void OnDespawn() // check lose
    {
        (TargetNearest as Bot)?.HideAim();
        base.OnDespawn();
        DisablePlayerMovement();

        OnDeath?.Invoke();
    }

    public void DisablePlayerMovement()
    {
        playerMovement.StopMoving();
        playerMovement.enabled = false;
    }

    private void Update()
    {
        if (GameManager.Instance.IsState(GameState.GamePlay))
        {
            CheckTargetNearestAndShowAim();
            HandleAnim();

            if (PlayerMovement.IsMoving()) return;
            if (ListTarget.Count > 0)
            {
                if (!IsAttack)
                {
                    AttackCharacter();
                }
            }
        }
    }

    public void CheckTargetNearestAndShowAim()
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
            (TargetNearest as Bot)?.HideAim();
            TargetNearest = currentTargetTmp;
            (TargetNearest as Bot)?.ShowAim();
        }
    }

    private void HandleAnim()
    {
        if (IsWin)
        {
            ChangeAnim(Constant.ANIM_DANCE);
            return;
        }

        if (IsDead)
        {
            ChangeAnim(Constant.ANIM_DEATH);
            return;
        }

        if (PlayerMovement.IsMoving())
        {
            ChangeAnim(Constant.ANIM_RUN);
            return;
        }
        if (IsAttack)
        {
            ChangeAnim(Constant.ANIM_ATTACK);
            return;
        }
        if (!PlayerMovement.IsMoving())
        {
            ChangeAnim(Constant.ANIM_IDLE);
            return;
        }

    }



    public void CreateAllWeaponPlayerOwner()
    {
        List<WeaponType> listWeaponOwner = DataManager.Instance.Data.ListWeaponOwner;
        foreach (WeaponType weapon in listWeaponOwner)
        {
            Instantiate(weaponSO.propWeapons[(int)weapon].weaponAvatarPrefabs, weaponHolderTF).SetActive(false);
        }
    }

    public void AddNewWeapon(WeaponType weaponTypeOnShop)
    {
        GameObject newWeaponGO = Instantiate(weaponSO.propWeapons[(int)weaponTypeOnShop].weaponAvatarPrefabs, weaponHolderTF);
        DeActiveCurrentWeapon();
        currentWeaponAvaGO = newWeaponGO;
        ActiveCurrentWeapon();

        currentWeaponType = weaponTypeOnShop;
    }

    public void GetCurrentWeaponDataAndActive()
    {
        List<WeaponType> listWeaponOwner = DataManager.Instance.Data.ListWeaponOwner;
        WeaponType currentWeaponTypeInData = DataManager.Instance.Data.CurrentWeapon;
        int getIndexInWeaponHolder = listWeaponOwner.IndexOf(currentWeaponTypeInData);
        if (getIndexInWeaponHolder == -1)
        {
            print("Ban dang ko so huu vu ki do " + (currentWeaponTypeInData).ToString());
            return;
        }

        currentWeaponType = currentWeaponTypeInData;
        currentWeaponAvaGO = weaponHolderTF.GetChild(getIndexInWeaponHolder).gameObject;
        currentWeaponAvaGO.SetActive(true);
    }

    public void ChangeWeaponOnHand()
    {
        DeActiveCurrentWeapon();
        GetCurrentWeaponDataAndActive();
        HandleAttackRangeBaseOnRangeWeapon();
        HandleCamPlayerBaseOnRangeWeapon();
    }

    public void LoadHat()
    {
        int currentHatInData = DataManager.Instance.Data.CurrentHat;
        if (currentHatInData == -1) return;
        currentHatAvaGOAttach = Instantiate(hatSO.propsHats[currentHatInData].avatarPrefab, hatHolderTF).gameObject;
    }
    public void LoadPant()
    {
        int currentPantInData = DataManager.Instance.Data.CurrentPant;
        if (currentPantInData == -1)
        {
            currentMatAttach = transparentMat;
            currentSkinPant.material = currentMatAttach;
            return;
        }
        currentMatAttach = pantSO.propsPants[currentPantInData].mat;
        currentSkinPant.material = currentMatAttach;

    }

    public void LoadShield()
    {
        int currentShieldInData = DataManager.Instance.Data.CurrentShield;
        if (currentShieldInData == -1) return;
        currenShieldAvaGOAttach = Instantiate(shieldSO.propsShields[currentShieldInData].avatarPrefab, shieldHolderTF).gameObject;
    }

    public void HandleCamPlayerBaseOnRangeWeapon()
    {
        cam.ChangeOffSetBaseRangeWeapon(attackRangeCurrentWeapon);
    }

    public override void ChangeScalePerKillAndIncreaseLevel() // check win
    {
        base.ChangeScalePerKillAndIncreaseLevel();
        cam.ChangeOffSetBaseScale();

        DataManager.Instance.Data.AddCoinToData();
        DataManager.Instance.SaveData();
    }


    // hat
    public void AttachHat(int id)
    {
        for (int i = 0; i < listContainHat.Count; i++)
        {
            if (listContainHat[i].Id == id)
            {
                currentHatAvaGOAttach = listContainHat[i].gameObject;
                currentHatAvaGOAttach.SetActive(false);
                return;
            }
        }

        PrefabItemShop itemHat = Instantiate(hatSO.propsHats[id].avatarPrefab, hatHolderTF);
        itemHat.SetId(id);
        listContainHat.Add(itemHat);
        currentHatAvaGOAttach = itemHat.gameObject;
        currentHatAvaGOAttach.SetActive(false);
    }
    public void DeAttachHat()
    {
        currentHatAvaGOAttach = null;
    }


    // pant
    public void AttachPant(int id)
    {
        currentMatAttach = pantSO.propsPants[id].mat;
        currentSkinPant.material = currentMatAttach;
    }

    public void SetMatAttach()
    {
        currentSkinPant.material = currentMatAttach;
    }

    public void DeAttachPant()
    {
        currentMatAttach = transparentMat;
    }

    // Shield
    public void AttachShield(int id)
    {
        for (int i = 0; i < listContainShield.Count; i++)
        {
            if (listContainShield[i].Id == id)
            {
                currenShieldAvaGOAttach = listContainShield[i].gameObject;
                currenShieldAvaGOAttach.SetActive(false);
                return;
            }
        }

        PrefabItemShop itemShield = Instantiate(shieldSO.propsShields[id].avatarPrefab, shieldHolderTF);
        itemShield.SetId(id);
        listContainShield.Add(itemShield);
        currenShieldAvaGOAttach = itemShield.gameObject;
        currenShieldAvaGOAttach.SetActive(false);
    }
    public void DeAttachShield()
    {
        currenShieldAvaGOAttach = null;
    }



}

