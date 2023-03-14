using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int Gold = 0;
    public List<int> WeaponOwner = new List<int>() { 0 };
    public int CurrentWeapon = 0;
    public int LevelId = 1;
}