using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Player : Character
{
    private PlayerMovement playerMovement;
    public PlayerMovement PlayerMovement => playerMovement;

    protected override void Awake()
    {
        base.Awake();
        playerMovement = GetComponent<PlayerMovement>();
    }

    public override void OnInit()
    {
        base.OnInit();
        playerMovement.enabled = true;
    }

    public override void OnDespawn()
    {
        (TargetNearest as Bot)?.HideAim();
        base.OnDespawn();
        playerMovement.StopMoving();
        playerMovement.enabled = false;
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
        if(IsDead)
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

    protected override void DelayRespawn()
    {
        ObjectPooling.Instance.ReturnGameObject(gameObject, PoolType.Player);
        LevelManager.Instance.CurrentLevel.SpawnPlayer();
    }

    public void CreateAllWeaponPlayerOwner()
    {
        List<int> listWeaponOwner = GameManager.Instance.Data.WeaponOwner;
        foreach (int indexWeapon in listWeaponOwner)
        {
            Instantiate(weaponSO.listWeaponAvatarPrefabs[indexWeapon], weaponHolder).SetActive(false);
        }
    }

    public void ActiveCurrentWeapon()
    {
        List<int> listWeaponOwner = GameManager.Instance.Data.WeaponOwner;
        int indexcurrentWeapon = GameManager.Instance.Data.CurrentWeapon;
        int indexInWeaponHolder = listWeaponOwner.IndexOf(indexcurrentWeapon);

        currentWeaponType = (WeaponType)indexcurrentWeapon;
        currentWeaponAvatar = weaponHolder.GetChild(indexInWeaponHolder).gameObject;
    }
}
