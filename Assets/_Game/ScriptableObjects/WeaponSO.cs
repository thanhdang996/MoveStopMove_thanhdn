using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum WeaponType { Axe = 0, Boomerang = 1, Hammer = 2, Cream = 3 }

[System.Serializable]
public struct PropWeapon
{
    public GameObject weaponAvatarPrefabs;
    public int attackRangeWeapons;
    public WeaponType weaponType;
    public int price;
}

[CreateAssetMenu()]
public class WeaponSO : ScriptableObject
{

    public PropWeapon[] propWeapons;

    public int ReturnAttackRangeOfWeapon(WeaponType weaponType)
    {
        for (int i = 0; i < propWeapons.Length; i++)
        {
            if (propWeapons[i].weaponType == weaponType)
            {
                return propWeapons[i].attackRangeWeapons;
            }
        }
        return 0;
    }
}
