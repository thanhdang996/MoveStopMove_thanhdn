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
    private AbstractTabItem tabItem;
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

    public void SetTabItem(AbstractTabItem tabItem)
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

    public void SetData(int id, AbstractTabItem tabItem, Sprite spriteImage, int price)
    {
        SetId(id);
        SetTabItem(tabItem);
        SetSprite(spriteImage);
        SetPrice(price);
    }

    public void Selected() // de dc chon phan tu dau tien ma ko check dk trung
    {
        tabItem.GetTabRoot().UI_SetTextPrice(price); // set coin for UIRoot

        outline.enabled = true;
        tabItem.PreviewItemOnPlayer(id);
    }

    public void HandleOnClick()
    {
        if (tabItem.GetCurrentUIItemShop().id == id) return;

        tabItem.GetTabRoot().UI_SetTextPrice(price);

        tabItem.ChangeOutLineButton(id);
        tabItem.PreviewItemOnPlayer(id);
    }
}
