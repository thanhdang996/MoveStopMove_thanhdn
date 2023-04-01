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
    public int Id => id;
    private TabItemController tabItemController;

    [SerializeField] private Image image;
    [SerializeField] private HairType hairType;
    [SerializeField] private int price;
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

    public void SetTabItemController(TabItemController tabItemController)
    {
        this.tabItemController = tabItemController;
    }

    public void SetSprite(Sprite spriteImage)
    {
        image.sprite = spriteImage;
    }
    public void SetHairType(HairType hairType)
    {
        this.hairType = hairType;
    }

    public void SetPrice(int price)
    {
        this.price = price;
    }

    public void Selected()
    {
        HandleOnClick();
    }

    private void HandleOnClick()
    {
        //print($"{hairType} - has price {price}");
        tabItemController.UICShopDress.UI_SetTextPrice(price);
        tabItemController.TurnOnOffOutLine(id);

        tabItemController.CheckToAddItemPlayer(id);
    }
}
