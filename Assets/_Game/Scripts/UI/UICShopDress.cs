using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UICShopDress : UICanvas
{
    [SerializeField] private int currentTabIndex;

    [SerializeField] TextMeshProUGUI textCoin;
    [SerializeField] private Transform contentTF;
    [SerializeField] private ItemShop prefabItemShop;
    [SerializeField] private HairSO hairSO;
    [SerializeField] TextMeshProUGUI textPrice;

    private void Start()
    {
        InitItem();
    }

    public override void Open()
    {
        base.Open();
        UI_UpdateTextCoin();
    }


    public void UI_UpdateTextCoin()
    {
        textCoin.text = DataManager.Instance.Data.Coin.ToString();
    }

    private void InitItem()
    {
        for (int i = 0; i < hairSO.propHair.Length; i++)
        {
            PropHair propItem = hairSO.propHair[i];
            ItemShop itemShop = Instantiate(prefabItemShop, contentTF);
            itemShop.SetUICShopDress(this);
            itemShop.SetPrefab(propItem.hairAvatarPrefabs);
            itemShop.SetSprite(propItem.spriteImage);
            itemShop.SetHairType(propItem.hairType);
            itemShop.SetPrice(propItem.price);

            if (i == 0)
            {
                itemShop.SetSelected();
            }
        }
    }

    public void SetTextPrice(int price)
    {
        textPrice.text = price.ToString();
    }
}
