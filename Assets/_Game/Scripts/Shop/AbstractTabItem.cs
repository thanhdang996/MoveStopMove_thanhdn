using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class AbstractTabItem : MonoBehaviour
{
    [SerializeField] protected int tabIndex;
    public int TabIndex => tabIndex;

    [SerializeField] protected UICShopDress tabRoot;


    protected UIItemShop currentUIItemShop;
    public UIItemShop CurrentUIItemShop => currentUIItemShop;


    [SerializeField] protected Button button;

    [SerializeField] protected Outline outline;


    protected virtual void OnEnable()
    {
        button.onClick.AddListener(HandleOnClick);
    }

    protected virtual void OnDisable()
    {
        button.onClick.RemoveListener(HandleOnClick);
    }

    public abstract void ActiveAllUIItemShop();
    public abstract void PreviewItem(int idUIITemShop);

    protected abstract void ShowPreviewItem();
    public abstract void HidePreviewItem();


    public virtual void ChangeOutLineButtonAndSetNewCurrentUI(int id)
    {
        currentUIItemShop.Outline.enabled = false;
        SetCurrentUIItemShop(id);
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

    public virtual void SetCurrentUIItemShop(int idUIITemShop)
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

    public abstract int GetCurrentItemInData();
    public abstract List<int> GetItemOwnerInData();

    public abstract void ChangeCurrentItemInData(int idUIITemShop);

    public abstract void AttachItemToPlayer();
    public abstract void DeAttachItemToPlayer();

}
