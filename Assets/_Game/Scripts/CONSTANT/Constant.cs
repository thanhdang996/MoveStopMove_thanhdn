using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constant : MonoBehaviour
{
    public static PoolType ConvertWeaponTypeeToObjectType(WeaponType weaponType)
    {
        switch (weaponType)
        {
            case WeaponType.Axe:
                return PoolType.Axe;
            case WeaponType.Boomerang:
                return PoolType.Boomerang;
            default:
                return PoolType.None;
        }
    }
}
