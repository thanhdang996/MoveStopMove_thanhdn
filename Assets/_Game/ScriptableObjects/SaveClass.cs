using UnityEngine;


public class PropsItem
{
    public int price;
    public Sprite spriteImage;
}

[System.Serializable]
public class PropsHair : PropsItem
{
    public PrefabItemShop avatarPrefab;
}

[System.Serializable]
public class PropsPant : PropsItem
{
    public Material mat;
}




