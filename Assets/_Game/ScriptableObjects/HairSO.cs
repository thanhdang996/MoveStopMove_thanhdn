using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public class PropHair
{
    public PrefabItemShop hairAvatarPrefabs;
    public int price;
    public Sprite spriteImage;
    public bool isUnlock;
}

[CreateAssetMenu(menuName = "Data/Hair")]
public class HairSO : ScriptableObject
{
    public PropHair[] propHair;
}
