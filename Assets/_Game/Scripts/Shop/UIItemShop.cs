using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class UIItemShop : MonoBehaviour
{
    private int id;
    public int Id => id;
    private AbstractTabItem tabItem;
    private int price;

    [SerializeField] private Image imageItem;
    [SerializeField] private Image imageLock;
    [SerializeField] private Button button;
    [SerializeField] private Outline outline;
    public Outline Outline => outline;

    [SerializeField] private TextMeshProUGUI textEquip;
    private bool isUnlock;



    private void OnEnable()
    {
        button.onClick.AddListener(HandleOnClick);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(HandleOnClick);
    }

    public void SetId(int id)
    {
        this.id = id;
    }

    public void SetTabItem(AbstractTabItem tabItem)
    {
        this.tabItem = tabItem;
    }

    public void SetSprite(Sprite spriteImage)
    {
        imageItem.sprite = spriteImage;
    }

    public void SetPrice(int price)
    {
        this.price = price;
    }
    public void SetUnlock(bool isUnlock)
    {
        this.isUnlock = isUnlock;
    }

    public void SetData(int id, AbstractTabItem tabItem, Sprite spriteImage, int price)
    {
        SetId(id);
        SetTabItem(tabItem);
        SetSprite(spriteImage);
        SetPrice(price);
    }

    public void Selected() // de dc chon phan tu dau tien ma ko check dk trung
    {
        CheckIfItemUnlockChangeButtonTabRoot();

        outline.enabled = true;
        tabItem.PreviewItemOnPlayer(id);
    }



    public void HandleOnClick()
    {
        if (tabItem.GetCurrentUIItemShop().id == id) return;

        CheckIfItemUnlockChangeButtonTabRoot();

        outline.enabled = true;
        tabItem.ChangeOutLineButtonAndSetNewCurrentUI(id);
        tabItem.PreviewItemOnPlayer(id);
    }

    private void CheckIfItemUnlockChangeButtonTabRoot()
    {
        if (isUnlock)
        {
            tabItem.GetTabRoot().ShowButtonEquipped(id, tabItem.GetCurrentItemInData());
        }
        else
        {
            tabItem.GetTabRoot().ShowButtonBuy();
            tabItem.GetTabRoot().UI_SetTextPrice(price);
        }
    }

    public void ChangeActiveImageLock(bool active)
    {
        imageLock.enabled = active;
    }
    public void ChangeActiveTextEquip(bool active)
    {
        textEquip.enabled = active;
    }
}
