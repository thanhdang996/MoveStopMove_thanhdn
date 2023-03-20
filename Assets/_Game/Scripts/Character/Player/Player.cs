using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Player : Character
{
    private PlayerMovement playerMovement;
    public PlayerMovement PlayerMovement => playerMovement;


    private bool isWin;
    public bool IsWin { get => isWin; set => isWin = value; }

    protected override void Awake()
    {
        base.Awake();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Start()
    {
        UIManager.Instance.OnRetryButton += OnRevivePlayer;
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
        if (!IsWin)
        {
            UIManager.Instance.ShowPanelLose();
        }
    }

    public void DisablePlayerMovement()
    {
        playerMovement.StopMoving();
        playerMovement.enabled = false;
    }

    private void OnRevivePlayer()
    {
        ObjectPooling.Instance.ReturnGameObject(gameObject, PoolType.Player);
        LevelManager.Instance.CurrentLevel.RevivePlayer();
        CheckConditonToWin();
    }

    private void Update()
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

    public void CheckTargetNearestAndShowAim()
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
            (TargetNearest as Bot)?.HideAim();
            TargetNearest = currentTargetTmp;
            (TargetNearest as Bot)?.ShowAim();
        }
    }

    private void HandleAnim()
    {
        if (IsWin)
        {
            ChangeAnim("Dance");
            return;
        }

        if (IsDead)
        {
            ChangeAnim("Death");
            return;
        }

        if (PlayerMovement.IsMoving())
        {
            ChangeAnim("Run");
            return;
        }
        if (IsAttack)
        {
            ChangeAnim("Attack");
            return;
        }
        else if (!PlayerMovement.IsMoving())
        {
            ChangeAnim("Idle");
            return;
        }

    }



    public void CreateAllWeaponPlayerOwner()
    {
        List<int> listWeaponOwner = GameManager.Instance.Data.WeaponOwner;
        foreach (int weapon in listWeaponOwner)
        {
            Instantiate(weaponSO.propWeapons[weapon].weaponAvatarPrefabs, weaponHolder).SetActive(false);
        }
    }

    public void AddNewWeapon(int indexWeaponOnShop)
    {
        Instantiate(weaponSO.propWeapons[indexWeaponOnShop].weaponAvatarPrefabs, weaponHolder).SetActive(false);
        //List<int> listWeaponOwner = GameManager.Instance.Data.WeaponOwner;
        //int getIndexInWeaponHolder = listWeaponOwner.IndexOf(indexWeaponOnShop);
        //currentWeaponAvatar.SetActive(false);
        //currentWeaponType = (WeaponType)indexWeaponOnShop;
        //currentWeaponAvatar = weaponHolder.GetChild(getIndexInWeaponHolder).gameObject;
        //currentWeaponAvatar.SetActive(true);
    }

    public void ActiveCurrentWeapon()
    {
        List<int> listWeaponOwner = GameManager.Instance.Data.WeaponOwner;
        int indexcurrentWeapon = GameManager.Instance.Data.CurrentWeapon;
        int getIndexInWeaponHolder = listWeaponOwner.IndexOf(indexcurrentWeapon);

        currentWeaponType = (WeaponType)indexcurrentWeapon;
        currentWeaponAvatar = weaponHolder.GetChild(getIndexInWeaponHolder).gameObject;
    }

    public void HandleCamPlayerBaseOnRangeWeapon()
    {
        GetComponent<CameraFollow>().ChangeOffSetBaseRangeWeapon(attackRangeCurrentWeapon);
    }

    public override void ChangeScalePerKillAndIncreaseLevel() // check win
    {
        base.ChangeScalePerKillAndIncreaseLevel();
        GetComponent<CameraFollow>().ChangeOffSetBaseScale();

        UIManager.Instance.HandUpdateCoinAndText();

        CheckConditonToWin();
    }

    private void CheckConditonToWin()
    {
        if (LevelManager.Instance.CurrentLevel.NoMoreEnemy && !IsDead)
        {
            UIManager.Instance.ShowPanelWin();
            DisablePlayerMovement();
            IsWin = true;
        }
    }
}
