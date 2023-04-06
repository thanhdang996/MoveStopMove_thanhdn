using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UICShopDress : UICanvas
{
    private enum BuySkinnButtonType { None, Equipped, UnEquipped, Buy }
    private BuySkinnButtonType currentButtonType;

    [SerializeField] private Button buttonBuy;
    [SerializeField] private TextMeshProUGUI textPrice;

    [SerializeField] private Button buttonEquipped;
    [SerializeField] private TextMeshProUGUI textEquipped;


    [SerializeField] private TextMeshProUGUI textCoin;

    private AbstractTabItem currentTabItem;
    public AbstractTabItem CurrentTabItem => currentTabItem;
    [SerializeField] private List<AbstractTabItem> listTabs;



    [SerializeField] private List<UIItemShop> listUIItemShop;
    public List<UIItemShop> ListUIItemShop => listUIItemShop;

    [SerializeField] private RectTransform contentTF;
    public RectTransform ContentTF => contentTF;

    [SerializeField] private UIItemShop prefabUIItemShop;
    public UIItemShop PrefabUIItemShop => prefabUIItemShop;



    public override void Open()
    {
        base.Open();
        UI_UpdateTextCoin();
        //if (LevelManager.Instance.CurrentPlayer.CurrentHairAvaGO != null)
        //{
        //    LevelManager.Instance.CurrentPlayer.CurrentHairAvaGO.SetActive(false);
        //}
        OpenTab(tabIndex: 0);
    }

    public override void CloseDirectly()
    {
        base.CloseDirectly();
        UIManager.Instance.OpenUI<UICMainMenu>();
        UIManager.Instance.GetUI<UICMainMenu>().UI_UpdateTextCoin();
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
            Vector2 currentPos = contentTF.anchoredPosition;
            currentPos.x = 0;
            contentTF.anchoredPosition = currentPos;

            currentTabItem.CurrentUIItemShop.Outline.enabled = false;
            currentTabItem.TurnOffOutLine();
            currentTabItem.DeActiveitemOnCurrentPlayer();
        }
        // vi tabIndex bang thu tu cac TabItemHair trong listTabs( luc keo editor)
        currentTabItem = listTabs[tabIndex];
        currentTabItem.ActiveAllUIItemShop();
        currentTabItem.TurnOnOutLine();
    }

    public void ShowButtonEquipped(int idUIITemShop, int currentItemTab)
    {
        if (idUIITemShop == currentItemTab)
        {
            currentButtonType = BuySkinnButtonType.Equipped;
        }
        else
        {
            currentButtonType = BuySkinnButtonType.UnEquipped;
        }
        buttonBuy.gameObject.SetActive(false);
        buttonEquipped.gameObject.SetActive(true);
        textEquipped.text = currentButtonType.ToString();

    }
    public void ShowButtonBuy()
    {
        currentButtonType = BuySkinnButtonType.Buy;
        buttonEquipped.gameObject.SetActive(false);
        buttonBuy.gameObject.SetActive(true);
    }
}
