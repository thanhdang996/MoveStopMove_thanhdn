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

    public static MyPoolType ConvertWeaponTypeeToObjectType(WeaponType weaponType)
    {
        return weaponType switch
        {
            WeaponType.Axe => MyPoolType.Axe,
            WeaponType.Boomerang => MyPoolType.Boomerang,
            WeaponType.Cream => MyPoolType.Cream,
            _ => MyPoolType.None,
        };
    }
}
