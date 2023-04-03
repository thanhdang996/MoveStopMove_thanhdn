using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class AbstractTabItem : MonoBehaviour
{
    [SerializeField] protected int tabIndex;

    [SerializeField] protected UICShopDress uicShopDress;

    [SerializeField] protected Transform contentTF;
    [SerializeField] protected UIItemShop prefabUIItemShop;
    

    protected UIItemShop currentUIItemShop;
    [SerializeField] protected List<UIItemShop> listUIItemShop;

    [SerializeField] protected Button button;
    protected bool isFirstTimeRender = true;

    [SerializeField] protected Outline outline;

    private void OnEnable()
    {
        button.onClick.AddListener(HandleOnClick);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(HandleOnClick);
    }

    protected abstract void InitItem();
    public abstract void PreviewItemOnPlayer(int idUIITemShop);
    public virtual void ActiveAllUIItemShop()
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
    public virtual void DeActiveAllUIItemShop()
    {
        for (int i = 0; i < listUIItemShop.Count; i++)
        {
            listUIItemShop[i].gameObject.SetActive(false);
        }
    }


    protected abstract void ActiveItemOnCurrentPlayer();
    public abstract void DeActiveitemOnCurrentPlayer();


    public virtual void HandleOutLineButton(int id)
    {
        currentUIItemShop.Outline.enabled = false;
        SetCurrentUIItemShop(id);
        currentUIItemShop.Outline.enabled = true;
    }
    protected virtual void SetCurrentUIItemShop(int index)
    {
        // vi luc Instantiate UIItemShop set id = index cua vong lap
        currentUIItemShop = listUIItemShop[index];
    }
    protected virtual void HandleOnClick()
    {
        if (uicShopDress.CurrentTabItem.tabIndex == tabIndex) return;
        uicShopDress.OpenTab(tabIndex);
    }

    public virtual void TurnOnOutLine()
    {
        outline.enabled = true;
    }
    public virtual void TurnOffOutLine()
    {
        outline.enabled = false;
    }

    public virtual UICShopDress GetUICShopDress()
    {
        return uicShopDress;
    }

    public virtual UIItemShop GetCurrentUIItemShop()
    {
        return currentUIItemShop;
    }
}
