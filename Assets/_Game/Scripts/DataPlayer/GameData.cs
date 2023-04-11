using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    [SerializeField] private int coin = 10000;
    public int Coin => coin;


    // weapon
    [SerializeField] private WeaponType currentWeapon = WeaponType.Axe;
    public WeaponType CurrentWeapon { get { return currentWeapon; } }

    [SerializeField] private List<WeaponType> listWeaponOwner = new List<WeaponType>() { WeaponType.Axe };
    public List<WeaponType> ListWeaponOwner { get { return listWeaponOwner; } }



    // hat
    [SerializeField] private int currentHat = -1;
    public int CurrentHat { get { return currentHat; } }

    [SerializeField] private List<int> listHatOwner = new List<int>();
    public List<int> ListHatOwner { get { return listHatOwner; } }


    // pants
    [SerializeField] private int currentPant = -1;
    public int CurrentPant { get { return currentPant; } }

    [SerializeField] private List<int> listPantOwner = new List<int>();
    public List<int> ListPantOwner { get { return listPantOwner; } }

    // shield
    [SerializeField] private int currenShield = -1;
    public int CurrentShield { get { return currenShield; } }

    [SerializeField] private List<int> listShieldOwner = new List<int>();
    public List<int> ListShieldOwner { get { return listShieldOwner; } }


    // set
    [SerializeField] private int currentSet = -1;
    public int CurrentSet { get { return currentSet; } }

    [SerializeField] private List<int> listSetOwner = new List<int>();
    public List<int> ListSetOwner { get { return listSetOwner; } }



    [SerializeField] private int levelId = 1;
    public int LevelId { get { return levelId; } }


    [SerializeField] private float bgMusicVolume = 1f;
    public float BGMusicVolume { get { return bgMusicVolume; } }


    [SerializeField] private float sfxVolume = 1f;
    public float SFXVolume { get { return sfxVolume; } }

    public void AddCoinToData()
    {
        coin++;
    }
    public void SetCoinToData(int coin)
    {
        this.coin = coin;
    }

    public void AddLevelToData()
    {
        levelId++;
    }

    public void SetBGVolumeToData(float value)
    {
        bgMusicVolume = value;
    }
    public void SetSFXVolumeToData(float value)
    {
        sfxVolume = value;
    }

    public void ChangeCurrentWeaponData(WeaponType weaponType)
    {
        currentWeapon = weaponType;
    }
    public void ChangeCurrentHatData(int id)
    {
        currentHat = id;
    }
    public void ChangeCurrentPantData(int id)
    {
        currentPant = id;
    }
    public void ChangeCurrentShieldData(int id)
    {
        currenShield = id;
    }

    public void ChangeCurrentSetData(int id)
    {
        currentSet = id;
    }
}