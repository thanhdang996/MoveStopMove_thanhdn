using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabItemMat : ITabItem
{

    [SerializeField] private int tabIndex;

    [SerializeField] private UICShopDress uicShopDress;

    [SerializeField] private Transform contentTF;
    [SerializeField] private UIItemShop prefabUIItemShop;
    [SerializeField] private SkinSO skinSO;


    private UIItemShop currentUIItemShop;
    [SerializeField] private List<UIItemShop> listUIItemShop;

    [SerializeField] private Button button;
    private bool isFirstTimeRender = true;

    [SerializeField] private Outline outline;


    private void OnEnable()
    {
        button.onClick.AddListener(HandleOnClick);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(HandleOnClick);
    }

    public override void InitItem()
    {
        for (int i = 0; i < skinSO.propsItems.Length; i++)
        {
            PropsItem propItem = skinSO.propsItems[i];

            UIItemShop itemShop = Instantiate(prefabUIItemShop, contentTF);
            listUIItemShop.Add(itemShop);
            itemShop.SetId(i);
            itemShop.SetTabItem(this);
            itemShop.SetSprite(propItem.spriteImage);
            itemShop.SetPrice(propItem.price);

            if (i == 0)
            {
                SetCurrentUIItemShop(0);
                itemShop.Selected();
            }
        }
    }

    public override void PreviewItemOnPlayer(int idUIITemShop)
    {
        LevelManager.Instance.CurrentPlayer.CurrentSkin.material = skinSO.propsItems[idUIITemShop].mat;
    }

    public override void ActiveAllUIItemShop()
    {
        if (isFirstTimeRender)
        {
            InitItem();
            isFirstTimeRender = false;
            return;
        }

        for (int i = 0; i < listUIItemShop.Count; i++)
        {
            listUIItemShop[i].gameObject.SetActive(true);
        }
        currentUIItemShop.Selected(); // luon select phan tu dau
    }
    public override void DeActiveAllUIItemShop()
    {
        for (int i = 0; i < listUIItemShop.Count; i++)
        {
            listUIItemShop[i].gameObject.SetActive(false);
        }
    }


    public override void ActiveItemOnCurrentPlayer()
    {
        LevelManager.Instance.CurrentPlayer.CurrentSkin.enabled = true;
    }
    public override void DeActiveitemOnCurrentPlayer()
    {
        LevelManager.Instance.CurrentPlayer.CurrentSkin.enabled = true;
    }


    public override void HandleOutLineButton(int id)
    {
        currentUIItemShop.Outline.enabled = false;
        SetCurrentUIItemShop(id);
        currentUIItemShop.Outline.enabled = true;
    }

    public override void SetCurrentUIItemShop(int index)
    {
        // vi luc Instantiate UIItemShop set id = index cua vong lap
        currentUIItemShop = listUIItemShop[index];
    }

    public override void HandleOnClick()
    {
        if (uicShopDress.CurrentTabItem.GetTabIndex() == tabIndex) return;
        uicShopDress.OpenTab(tabIndex);
    }

    public override void TurnOnOutLine()
    {
        outline.enabled = true;
    }

    public override void TurnOffOutLine()
    {
        outline.enabled = false;
    }

    public override int GetTabIndex()
    {
        return tabIndex;
    }

    public override UICShopDress GetUICShopDress()
    {
        return uicShopDress;
    }

    public override UIItemShop GetCurrentUIItemShop()
    {
        return currentUIItemShop;
    }
}
