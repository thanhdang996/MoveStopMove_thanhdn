using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UICShopDress : UICanvas
{

    [SerializeField] private TextMeshProUGUI textCoin;
    [SerializeField] private TextMeshProUGUI textPrice;

    private AbstractTabItem currentTabItem;
    public AbstractTabItem CurrentTabItem => currentTabItem;
    [SerializeField] private List<AbstractTabItem> listTabs;



    [SerializeField] private List<UIItemShop> listUIItemShop;
    public List<UIItemShop> ListUIItemShop => listUIItemShop;


    public override void Open()
    {
        base.Open();
        UI_UpdateTextCoin();
        LevelManager.Instance.CurrentPlayer.CurrentHairAvaGO.SetActive(false);
        OpenTab(tabIndex: 0);
    }

    public override void CloseDirectly()
    {
        base.CloseDirectly();
        UIManager.Instance.OpenUI<UICMainMenu>();
        UIManager.Instance.GetUI<UICMainMenu>().UI_UpdateTextCoin();
        LevelManager.Instance.CurrentPlayer.CurrentHairAvaGO.SetActive(true);

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
        if (currentTabItem != null)
        {
            // reset conttent pos to zero x
            Vector2 currentPos = currentTabItem.ContentTF.anchoredPosition;
            currentPos.x = 0;
            currentTabItem.ContentTF.anchoredPosition = currentPos;

            currentTabItem.TurnOffOutLine();

            // khi chuyen tab, tab cu set button select luon la  vi tri 0 la nut dau tien ( do trong ChangeOutLineButton co SetCurrentUIItemShop)
            currentTabItem.DeActiveitemOnCurrentPlayer();
            currentTabItem.ChangeOutLineButton(0); // change UIItemShop to 0
        }
        // vi tabIndex bang thu tu cac TabItemHair trong listTabs( luc keo editor)
        currentTabItem = listTabs[tabIndex]; 
        currentTabItem.ActiveAllUIItemShop();
        currentTabItem.TurnOnOutLine();
    }
}
