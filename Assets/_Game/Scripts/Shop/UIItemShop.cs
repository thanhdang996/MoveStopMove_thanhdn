using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIItemShop : MonoBehaviour
{
    private int id;
    private ITabItem tabItem;
    private int price;

    [SerializeField] private Image image;
    [SerializeField] private Button button;
    [SerializeField] private Outline outline;
    public Outline Outline => outline;


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

    public void SetTabItem(ITabItem tabItem)
    {
        this.tabItem = tabItem;
    }

    public void SetSprite(Sprite spriteImage)
    {
        image.sprite = spriteImage;
    }

    public void SetPrice(int price)
    {
        this.price = price;
    }


    public void Selected() // de dc chon phan tu dau tien
    {
        tabItem.GetUICShopDress().UI_SetTextPrice(price);

        outline.enabled = true;
        tabItem.PreviewItemOnPlayer(id);
    }

    public void HandleOnClick()
    {
        if (tabItem.GetCurrentUIItemShop().id == id) return;

        tabItem.GetUICShopDress().UI_SetTextPrice(price);

        tabItem.HandleOutLineButton(id);
        tabItem.PreviewItemOnPlayer(id);
    }
}
