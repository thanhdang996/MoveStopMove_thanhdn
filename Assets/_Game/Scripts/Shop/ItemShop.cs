using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemShop : MonoBehaviour, IDeselectHandler
{
    public int id;
    private UICShopDress uicShopDress;
    [SerializeField] private GameObject itemShopPrefab;
    [SerializeField] private Image image;
    [SerializeField] private HairType hairType;
    [SerializeField] private int price;
    [SerializeField] private Button button;

    [SerializeField] private GameObject tempItemShopPrefab;
    [SerializeField] private ItemShopOnHand tempItem;


    private void OnEnable()
    {
        button.onClick.AddListener(HandleOnClick);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(HandleOnClick);
    }

    public void SetUICShopDress(UICShopDress uicShopDress)
    {
        this.uicShopDress= uicShopDress;
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

    public void SetPrefab(GameObject prefab)
    {
        itemShopPrefab = prefab;
        tempItemShopPrefab = Instantiate(itemShopPrefab);
        tempItemShopPrefab.SetActive(false);
    }

    public void SetSelected()
    {
        button.Select();
        HandleOnClick();
    }

    private void HandleOnClick()
    {
        print($"{hairType} - has price {price}");
        uicShopDress.SetTextPrice(price);
        var tfHairPlayer = LevelManager.Instance.CurrentPlayer.HairHolderTF;
        tempItem = SimplePool.Spawn<ItemShopOnHand>(PoolType.ItemShopOnHand, tfHairPlayer);
        tempItemShopPrefab.SetActive(true);
        tempItemShopPrefab.transform.SetParent(tempItem.transform);
        tempItemShopPrefab.transform.localPosition = Vector3.zero;
        tempItemShopPrefab.transform.rotation = tempItem.transform.rotation;
        tempItemShopPrefab.transform.localScale = Vector3.one;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        SimplePool.Despawn(tempItem);
        tempItemShopPrefab.SetActive(false);
    }
}
