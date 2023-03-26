using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    [SerializeField] private int coin = 0;
    public int Coin => coin;


    [SerializeField] private List<int> weaponOwner = new List<int>() { 0 };
    public List<int> WeaponOwner { get { return weaponOwner; } }


    [SerializeField] private int currentWeapon = 0;
    public int CurrentWeapon { get { return currentWeapon; } }


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

    public void AddNewItemToData(int indexWeaponOnShop)
    {
        weaponOwner.Add(indexWeaponOnShop);
        DataManager.Instance.SaveData();

        LevelManager.Instance.CurrentPlayer.AddNewWeapon(indexWeaponOnShop);
    }
}