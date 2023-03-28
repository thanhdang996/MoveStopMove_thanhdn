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
        List<int> listWeaponOwner = DataManager.Instance.Data.WeaponOwner;
        foreach (int weapon in listWeaponOwner)
        {
            Instantiate(weaponSO.propWeapons[weapon].weaponAvatarPrefabs, weaponHolderTF).SetActive(false);
        }
    }

    public void AddNewWeapon(int indexWeaponOnShop)
    {
        Instantiate(weaponSO.propWeapons[indexWeaponOnShop].weaponAvatarPrefabs, weaponHolderTF).SetActive(false);
        List<int> listWeaponOwner = DataManager.Instance.Data.WeaponOwner;
        int getIndexInWeaponHolder = listWeaponOwner.IndexOf(indexWeaponOnShop);
        currentWeaponAvaGO.SetActive(false);
        currentWeaponType = (WeaponType)indexWeaponOnShop;
        currentWeaponAvaGO = weaponHolderTF.GetChild(getIndexInWeaponHolder).gameObject;
        currentWeaponAvaGO.SetActive(true);
    }
    public void DeActiveCurrentWeapon()
    {
        currentWeaponAvaGO.SetActive(false);
    }
    public void ActiveCurrentWeapon()
    {
        currentWeaponAvaGO.SetActive(true);
    }

    public void GetCurrentWeaponDataAndActive()
    {
        List<int> listWeaponOwner = DataManager.Instance.Data.WeaponOwner;
        int indexcurrentWeapon = DataManager.Instance.Data.CurrentWeapon;
        int getIndexInWeaponHolder = listWeaponOwner.IndexOf(indexcurrentWeapon);

        currentWeaponType = (WeaponType)indexcurrentWeapon;
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
}
