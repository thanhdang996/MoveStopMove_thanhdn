using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PropsItem
{
    public PrefabItemShop avatarPrefab;
    public int price;
    public Sprite spriteImage;
    public Material mat;
}

[CreateAssetMenu(menuName = "Data/Skin")]
public class SkinSO : ScriptableObject
{
    public PropsItem[] propsItems;
}
