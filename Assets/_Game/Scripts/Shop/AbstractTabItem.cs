using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class AbstractTabItem : MonoBehaviour
{
    [SerializeField] protected int tabIndex;

    [SerializeField] protected UICShopDress tabRoot;

    [SerializeField] protected RectTransform contentTF;
    public RectTransform ContentTF => contentTF;
    [SerializeField] protected UIItemShop prefabUIItemShop;

    private UIItemShop currentUIItemShop;


    [SerializeField] protected Button button;

    [SerializeField] protected Outline outline;

    private void OnEnable()
    {
        button.onClick.AddListener(HandleOnClick);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(HandleOnClick);
    }

    public abstract void ActiveAllUIItemShop();
    public abstract void PreviewItemOnPlayer(int idUIITemShop);

    protected abstract void ActiveItemOnCurrentPlayer();
    public abstract void DeActiveitemOnCurrentPlayer();


    public virtual void ChangeOutLineButton(int id)
    {
        currentUIItemShop.Outline.enabled = false;
        SetCurrentUIItemShop(id);
        currentUIItemShop.Outline.enabled = true;
    }

    public virtual void TurnOnOutLine()
    {
        outline.enabled = true;
    }
    public virtual void TurnOffOutLine()
    {
        outline.enabled = false;
    }

    public virtual UICShopDress GetTabRoot()
    {
        return tabRoot;
    }

    protected virtual void SetCurrentUIItemShop(int idUIITemShop)
    {
        // vi luc Instantiate UIItemShop set id = idUIITemShop cua vong lap
        currentUIItemShop = tabRoot.ListUIItemShop[idUIITemShop];
    }
    public virtual UIItemShop GetCurrentUIItemShop()
    {
        return currentUIItemShop;
    }

    protected virtual void HandleOnClick()
    {
        if (tabRoot.CurrentTabItem.tabIndex == tabIndex) return;
        tabRoot.OpenTab(tabIndex);
    }
}
