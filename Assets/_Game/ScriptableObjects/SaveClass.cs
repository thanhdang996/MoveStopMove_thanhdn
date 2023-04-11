using UnityEngine;


public class PropsItem
{
    public int price;
    public Sprite spriteImage;
}

[System.Serializable]
public class PropsHat : PropsItem
{
    public ItemShop avatarPrefab;
}

[System.Serializable]
public class PropsShield : PropsItem
{
    public ItemShop avatarPrefab;
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
    public ItemShop hatPrefab;
    public ItemShop wingPrefab;
    public ItemShop tailPrefab;
}




