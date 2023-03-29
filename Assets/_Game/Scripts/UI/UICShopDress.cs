using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICShopDress : UICanvas
{
    [SerializeField] private Transform contentTF;
    [SerializeField] private GameObject prefabItemShop;

    public override void Open()
    {
        base.Open();
        for (int i = 0; i < 10; i++)
        {
            Instantiate(prefabItemShop, contentTF);
        }
    }
}
