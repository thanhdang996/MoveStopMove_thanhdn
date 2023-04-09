using UnityEngine;


public class PropsItem
{
    public int price;
    public Sprite spriteImage;
}

[System.Serializable]
public class PropsHat : PropsItem
{
    public PrefabItemShop avatarPrefab;
}

[System.Serializable]
public class PropsShield : PropsItem
{
    public PrefabItemShop avatarPrefab;
}

[System.Serializable]
public class PropsPant : PropsItem
{
    public Material mat;
}

[System.Serializable]
public class PropsSet : PropsItem
{
    public Material mat;
}




