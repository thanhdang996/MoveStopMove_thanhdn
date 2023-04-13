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
    public CameraFollow Cam => cam;

    [SerializeField] private SpriteRenderer attackRangeSpriteRender;
    public SpriteRenderer AttackRangeSpriteRender => attackRangeSpriteRender;
    [SerializeField] private Material defaultPlayerMat;



    private bool isWin;
    public bool IsWin { get => isWin; set => isWin = value; }

    public bool IsInShopDress { get; set; }


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
        TF.rotation = Quaternion.Euler(0, 180, 0);
        canvasShowLevel.SetTextLevel(levelCharacter);
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
        HandleAnim();
        if (GameManager.Instance.IsState(GameState.GamePlay))
        {
            CheckTargetNearestAndShowAim();

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
        if (IsWin || IsInShopDress)
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
            currentPantMatAttach = transparentMat;
            currentSkinPant.material = currentPantMatAttach;
            return;
        }
        currentPantMatAttach = pantSO.propsPants[currentPantInData].mat;
        currentSkinPant.material = currentPantMatAttach;

    }

    public void LoadShield()
    {
        int currentShieldInData = DataManager.Instance.Data.CurrentShield;
        if (currentShieldInData == -1) return;
        currentShieldAvaGOAttach = Instantiate(shieldSO.propsShields[currentShieldInData].avatarPrefab, shieldHolderTF).gameObject;
    }

    public void LoadSet()
    {
        int currentSetInData = DataManager.Instance.Data.CurrentSet;
        if (currentSetInData == -1)
        {
            currentSetMatAttach = defaultPlayerMat;
            currentSkinSet.material = currentSetMatAttach;
            return;
        }
        currentSetMatAttach = setSO.propsSets[currentSetInData].mat;
        currentSkinSet.material = currentSetMatAttach;

        //load ItemPrefab
        if (setSO.HasHat(currentSetInData))
        {
            currentHatAvaGOAttach = Instantiate(setSO.propsSets[currentSetInData].hatPrefab, hatHolderTF).gameObject;
        }
        if (setSO.HasWing(currentSetInData))
        {
            currentWingAvaGOAttach = Instantiate(setSO.propsSets[currentSetInData].wingPrefab, wingHolderTF).gameObject;
        }
        if (setSO.HasTail(currentSetInData))
        {
            currentTailAvaGOAttach = Instantiate(setSO.propsSets[currentSetInData].tailPrefab, tailHolderTF).gameObject;
        }
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

    
    #region Hat
    public void AttachHat(int idUIITemShop)
    {
        currentHatAvaGOAttach = Instantiate(hatSO.propsHats[idUIITemShop].avatarPrefab, hatHolderTF).gameObject;
        currentHatAvaGOAttach.SetActive(false);
    }
    public void DeAttachHat()
    {
        Destroy(currentHatAvaGOAttach);
    }


    public void ShowHatAvaAttach()
    {
        if (currentHatAvaGOAttach != null)
        {
            currentHatAvaGOAttach.SetActive(true);
        }
    }
    public void HideHatAvaAttach()
    {
        if (currentHatAvaGOAttach != null)
        {
            currentHatAvaGOAttach.SetActive(false);
        }
    }
    #endregion



    #region Pant
    public void AttachPant(int id)
    {
        currentPantMatAttach = pantSO.propsPants[id].mat;
        currentSkinPant.material = currentPantMatAttach;
    }
    public void DeAttachPant()
    {
        currentPantMatAttach = transparentMat;
    }

    public void ShowPantAttach()
    {
        currentSkinPant.material = currentPantMatAttach;
    }
    public void HidePantAttach()
    {
        currentSkinPant.material = transparentMat;
    }
    public void SetPantMat(Material mat)
    {
        currentSkinPant.material = mat;
    }
    #endregion

    #region Shield
    public void AttachShield(int idUIITemShop)
    {
        currentShieldAvaGOAttach = Instantiate(shieldSO.propsShields[idUIITemShop].avatarPrefab, shieldHolderTF).gameObject;
        currentShieldAvaGOAttach.SetActive(false);
    }
    public void DeAttachShield()
    {
        Destroy(currentShieldAvaGOAttach);
    }


    public void ShowShieldAvaAttach()
    {
        if (currentShieldAvaGOAttach != null)
        {
            currentShieldAvaGOAttach.SetActive(true);
        }
    }
    public void HideShieldAvaAttach()
    {
        if (currentShieldAvaGOAttach != null)
        {
            currentShieldAvaGOAttach.SetActive(false);
        }
    }
    #endregion

    #region Set
    public void AttachSet(int id)
    {
        // for material
        currentSetMatAttach = setSO.propsSets[id].mat;
        currentSkinSet.material = currentSetMatAttach;

        // for itemprefab;
        if (setSO.HasHat(id))
        {
            currentHatAvaGOAttach = Instantiate(setSO.propsSets[id].hatPrefab, hatHolderTF).gameObject;
            currentHatAvaGOAttach.SetActive(false);
        }
        else
        {
            Destroy(currentHatAvaGOAttach);
        }

        if (setSO.HasWing(id))
        {
            currentWingAvaGOAttach = Instantiate(setSO.propsSets[id].wingPrefab, wingHolderTF).gameObject;
            currentWingAvaGOAttach.SetActive(false);
        }
        else
        {
            Destroy(currentWingAvaGOAttach);
        }

        if (setSO.HasTail(id))
        {
            currentTailAvaGOAttach = Instantiate(setSO.propsSets[id].tailPrefab, tailHolderTF).gameObject;
            currentTailAvaGOAttach.SetActive(false);
        }
        else
        {
            Destroy(currentTailAvaGOAttach);
        }
    }

    public void DeAttachSet()
    {
        // for material
        currentSetMatAttach = defaultPlayerMat;

        // for itemprefab;
        Destroy(currentHatAvaGOAttach);
        Destroy(currentWingAvaGOAttach);
        Destroy(currentTailAvaGOAttach);
    }

    public void ShowSetAttach()
    {
        // for material
        currentSkinSet.material = currentSetMatAttach;

        // for itemprefab;
        if (currentHatAvaGOAttach != null)
        {
            currentHatAvaGOAttach.SetActive(true);
        }
        if (currentWingAvaGOAttach != null)
        {
            currentWingAvaGOAttach.SetActive(true);
        }
        if (currentTailAvaGOAttach != null)
        {
            currentTailAvaGOAttach.SetActive(true);
        }
    }
    public void HideSetAttach()
    {
        // for material
        currentSkinSet.material = defaultPlayerMat;

        // for itemprefab;
        if (currentHatAvaGOAttach != null)
        {
            currentHatAvaGOAttach.SetActive(false);
        }
        if (currentWingAvaGOAttach != null)
        {
            currentWingAvaGOAttach.SetActive(false);
        }
        if (currentTailAvaGOAttach != null)
        {
            currentTailAvaGOAttach.SetActive(false);
        }
    }
    public void SetSetMat(Material mat)
    {
        currentSkinSet.material = mat;
    }
    #endregion

}

