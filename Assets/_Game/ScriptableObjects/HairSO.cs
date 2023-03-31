using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum HairType { None, Hoa, MuCS, MuiTen, Rau, Horn, TaiTho, TocXu, VuongMien }

[System.Serializable]
public class PropHair
{
    public GameObject hairAvatarPrefabs;
    public HairType hairType;
    public int price;
    public Sprite spriteImage;
    public bool isUnlock;
}

[CreateAssetMenu(menuName = "Data/Hair")]
public class HairSO : ScriptableObject
{
    public PropHair[] propHair;
}
