using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum WeaponType { Axe, Boomerang, Hammer }

[CreateAssetMenu()]
public class WeaponSO : ScriptableObject
{
    public List<GameObject> listWeaponAvatarPrefabs;
}
