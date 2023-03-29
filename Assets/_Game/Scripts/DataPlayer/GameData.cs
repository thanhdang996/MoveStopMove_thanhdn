using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    [SerializeField] private int coin = 0;
    public int Coin => coin;


    [SerializeField] private List<WeaponType> weaponOwner = new List<WeaponType>() { WeaponType.Axe };
    public List<WeaponType> WeaponOwner { get { return weaponOwner; } }


    [SerializeField] private WeaponType currentWeapon =  WeaponType.Axe;
    public WeaponType CurrentWeapon { get { return currentWeapon; } }


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
        this.coin= coin;
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

}