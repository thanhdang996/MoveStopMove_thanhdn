using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constant : MonoBehaviour
{
    public const string ANIM_DANCE = "Dance"; 
    public const string ANIM_DEATH = "Death"; 
    public const string ANIM_RUN = "Run"; 
    public const string ANIM_ATTACK = "Attack"; 
    public const string ANIM_IDLE = "Idle"; 

    public static PoolType ConvertWeaponTypeeToObjectType(WeaponType weaponType)
    {
        return weaponType switch
        {
            WeaponType.Axe => PoolType.Axe,
            WeaponType.Boomerang => PoolType.Boomerang,
            WeaponType.Cream => PoolType.Cream,
            _ => PoolType.None,
        };
    }
}
