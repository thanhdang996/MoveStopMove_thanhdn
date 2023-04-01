using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UICShopDress : UICanvas
{
    private int currentTabIndex = 0;
    public int CurrentTabIndex => currentTabIndex;

    [SerializeField] private TextMeshProUGUI textCoin;
    [SerializeField] private TextMeshProUGUI textPrice;

    [SerializeField] private List<TabItemController> tabs;



    public override void Open()
    {
        base.Open();
        UI_UpdateTextCoin();
        OpenTab(tabIndex: 0);
    }

    public void UI_UpdateTextCoin()
    {
        textCoin.text = DataManager.Instance.Data.Coin.ToString();
    }
    public void UI_SetTextPrice(int price)
    {
        textPrice.text = price.ToString();
    }

    public void OpenTab(int tabIndex)
    {
        tabs[currentTabIndex].DeActiveUIItemShop();
        if (tabs[currentTabIndex].CurrentPrefabItemPlayer != null)
        {
            tabs[currentTabIndex].CurrentPrefabItemPlayer.gameObject.SetActive(false);
        }

        currentTabIndex = tabIndex;
        tabs[currentTabIndex].ActiveUIItemShop();
    }
}
