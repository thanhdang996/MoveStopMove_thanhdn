using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Player : Character
{
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

    private void Start()
    {
        MyUIManager.Instance.OnRetryButton += OnRevivePlayer;
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
            MyUIManager.Instance.ShowPanelLose();
            SoundManager.Instance.StopBGSoundMusic();
        }
    }

    public void DisablePlayerMovement()
    {
        playerMovement.StopMoving();
        playerMovement.enabled = false;
    }

    private void OnRevivePlayer()
    {
        //ObjectPooling.Instance.ReturnGameObject(gameObject, MyPoolType.Player);
        SimplePool.Despawn(this);
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
        //List<int> listWeaponOwner = DataManager.Instance.Data.WeaponOwner;
        //int getIndexInWeaponHolder = listWeaponOwner.IndexOf(indexWeaponOnShop);
        //currentWeaponAvatar.SetActive(false);
        //currentWeaponType = (WeaponType)indexWeaponOnShop;
        //currentWeaponAvatar = weaponHolder.GetChild(getIndexInWeaponHolder).gameObject;
        //currentWeaponAvatar.SetActive(true);
    }

    public void ActiveCurrentWeapon()
    {
        List<int> listWeaponOwner = DataManager.Instance.Data.WeaponOwner;
        int indexcurrentWeapon = DataManager.Instance.Data.CurrentWeapon;
        int getIndexInWeaponHolder = listWeaponOwner.IndexOf(indexcurrentWeapon);

        currentWeaponType = (WeaponType)indexcurrentWeapon;
        currentWeaponAvaGO = weaponHolderTF.GetChild(getIndexInWeaponHolder).gameObject;
    }

    public void HandleCamPlayerBaseOnRangeWeapon()
    {
        cam.ChangeOffSetBaseRangeWeapon(attackRangeCurrentWeapon);
    }

    public override void ChangeScalePerKillAndIncreaseLevel() // check win
    {
        base.ChangeScalePerKillAndIncreaseLevel();
        cam.ChangeOffSetBaseScale();

        MyUIManager.Instance.HandUpdateCoinAndText();

        CheckConditonToWin();
    }

    private void CheckConditonToWin()
    {
        if (LevelManager.Instance.CurrentLevel.NoMoreEnemy && !IsDead)
        {
            MyUIManager.Instance.ShowPanelWin();
            DisablePlayerMovement();
            IsWin = true;
            SoundManager.Instance.PlaySoundSFX2D(SoundType.Win);
            SoundManager.Instance.StopBGSoundMusic();
        }
    }
}
