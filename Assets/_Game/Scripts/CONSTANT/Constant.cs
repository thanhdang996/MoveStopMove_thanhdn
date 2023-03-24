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

    public const string VOLUME_MY_MUSIC = "MyMusic";
    public const string VOLUME_MY_SFX = "MySFX";


    public const string TAG_OBSTACLE = "Obstacle";



    public static PoolType ConvertWeaponTypeToObjectType(WeaponType weaponType)
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
