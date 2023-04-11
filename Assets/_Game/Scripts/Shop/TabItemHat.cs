using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabItemHat : AbstractTabItem
{
    [SerializeField] private HatSO hatSO;

    [SerializeField] private ItemShop currentItemPreview;
    [SerializeField] private List<ItemShop> listItemPreviewContain = new List<ItemShop>();


    public override void ActiveAllUIItemShop()
    {
        // check to Instantiate item , deactive item in contentTF
        PropsHat[] propsHats = hatSO.propsHats;
        int totalItemData = propsHats.Length;
        int numberItemShow = tabRoot.ListUIItemShop.Count;

        int diff = Mathf.Abs(totalItemData - numberItemShow);
        if (totalItemData >= numberItemShow) // Instantiate
        {
            for (int i = 0; i < diff; i++)
            {
                UIItemShop itemShop = Instantiate(tabRoot.PrefabUIItemShop, tabRoot.ContentTF);
                tabRoot.ListUIItemShop.Add(itemShop);
            }
        }
        else // deactive
        {
            for (int i = 0; i < diff; i++)
            {
                tabRoot.ListUIItemShop[tabRoot.ListUIItemShop.Count - i - 1].gameObject.SetActive(false);
            }
        }


        // check to active, deactive image lock, text equip base on current TabItem
        int currentHat = GetCurrentItemInData();
        for (int i = 0; i < propsHats.Length; i++)
        {
            tabRoot.ListUIItemShop[i].gameObject.SetActive(true); // bat tat ca 
            tabRoot.ListUIItemShop[i].SetData(i, this, propsHats[i].spriteImage, propsHats[i].price);
            if (currentHat == i)
            {
                SetCurrentUIItemShop(currentHat);
                CurrentUIItemShop.ChangeActiveImageLock(false);
                CurrentUIItemShop.ChangeActiveTextEquip(true);
                CurrentUIItemShop.SetUnlock(true);
                CurrentUIItemShop.Selected();
                continue;
            }
            bool isContainItem = DataManager.Instance.Data.ListHatOwner.Contains(i);
            if (isContainItem)
            {
                tabRoot.ListUIItemShop[i].ChangeActiveImageLock(false);
                tabRoot.ListUIItemShop[i].ChangeActiveTextEquip(false);
                tabRoot.ListUIItemShop[i].SetUnlock(true);
                continue;
            }
            tabRoot.ListUIItemShop[i].ChangeActiveImageLock(true);
            tabRoot.ListUIItemShop[i].ChangeActiveTextEquip(false);
            tabRoot.ListUIItemShop[i].SetUnlock(false);
        }
        if (currentHat == -1)
        {
            SetCurrentUIItemShop(0); // neu chua co item owner thi hightlight default item 0
            CurrentUIItemShop.Selected();
        }
    }


    public override void PreviewItem(int idUIITemShop)
    {
        for (int i = 0; i < listItemPreviewContain.Count; i++) // kiem tra item co trong list chua
        {
            if (listItemPreviewContain[i].Id == idUIITemShop)
            {
                HidePreviewItem();
                currentItemPreview = listItemPreviewContain[i];
                ShowPreviewItem();
                return;
            }
        }
        HidePreviewItem();
        currentItemPreview = Instantiate(hatSO.propsHats[idUIITemShop].avatarPrefab, LevelManager.Instance.CurrentPlayer.HatHolderTF);
        currentItemPreview.SetId(idUIITemShop);

        listItemPreviewContain.Add(currentItemPreview);
        ShowPreviewItem();
    }

    protected override void ShowPreviewItem()
    {
        currentItemPreview.gameObject.SetActive(true); //show tren nguoi player nhung la prefab
    }
    public override void HidePreviewItem() //hide tren nguoi player nhung la prefab
    {
        if (currentItemPreview != null)
        {
            currentItemPreview.gameObject.SetActive(false);
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        LevelManager.Instance.CurrentPlayer.HideHatAvaAttach(); //hide tren nguoi player nhung la currentHatAvaGOAttach that

    }

    protected override void OnDisable()
    {
        base.OnDisable();
        for (int i = 0; i < listItemPreviewContain.Count; i++) // destroy cac list item preview
        {
            Destroy(listItemPreviewContain[i].gameObject);
        }
        listItemPreviewContain.Clear();
        LevelManager.Instance.CurrentPlayer.ShowHatAvaAttach(); //show tren nguoi player nhung la currentHatAvaGOAttach that
    }

    public override int GetCurrentItemInData()
    {
        return DataManager.Instance.Data.CurrentHat;
    }

    public override List<int> GetItemOwnerInData()
    {
        return DataManager.Instance.Data.ListHatOwner;
    }
    public override void ChangeCurrentItemInData(int idUIITemShop)
    {
        DataManager.Instance.Data.ChangeCurrentHatData(idUIITemShop);
    }

    public override void AttachItemToPlayer()
    {
        LevelManager.Instance.CurrentPlayer.AttachHat(currentUIItemShop.Id);
    }

    public override void DeAttachItemToPlayer()
    {
        LevelManager.Instance.CurrentPlayer.DeAttachHat();
    }
}