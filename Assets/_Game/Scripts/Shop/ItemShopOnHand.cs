using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemShopOnHand : GameUnit
{
    List<ItemShop> itemShops= new List<ItemShop>();

    public void CheckItem(int id)
    {
        for (int i = 0; i < itemShops.Count; i++)
        {
            if (itemShops[i].id == id)
            {
                ActiveGO();
            } else
            {
                DeActive();
            }
        }
    }

    private void DeActive()
    {
        throw new NotImplementedException();
    }

    private void ActiveGO()
    {
        throw new NotImplementedException();
    }
}
