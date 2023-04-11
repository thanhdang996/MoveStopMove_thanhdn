using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabItemShield : AbstractTabItem
{
    [SerializeField] private ShieldSO shieldSO;

    [SerializeField] private ItemShop currentItemPreview;
    private List<ItemShop> listItemPreviewContain = new List<ItemShop>();

    public override void ActiveAllUIItemShop()
    {
        PropsShield[] propsShield = shieldSO.propsShields;
        int totalItemData = propsShield.Length;
        int numberItemShow = tabRoot.ListUIItemShop.Count;

        int diff = Mathf.Abs(totalItemData - numberItemShow);
        if (totalItemData >= numberItemShow)
        {
            for (int i = 0; i < diff; i++)
            {
                UIItemShop itemShop = Instantiate(tabRoot.PrefabUIItemShop, tabRoot.ContentTF);
                tabRoot.ListUIItemShop.Add(itemShop);
            }
        }
        else
        {
            for (int i = 0; i < diff; i++)
            {
                tabRoot.ListUIItemShop[tabRoot.ListUIItemShop.Count - i - 1].gameObject.SetActive(false);
            }
        }



        int currentShield = GetCurrentItemInData();
        for (int i = 0; i < propsShield.Length; i++)
        {
            tabRoot.ListUIItemShop[i].gameObject.SetActive(true);
            tabRoot.ListUIItemShop[i].SetData(i, this, propsShield[i].spriteImage, propsShield[i].price);
            if (currentShield == i)
            {
                SetCurrentUIItemShop(currentShield);
                CurrentUIItemShop.ChangeActiveImageLock(false);
                CurrentUIItemShop.ChangeActiveTextEquip(true);
                CurrentUIItemShop.SetUnlock(true);
                CurrentUIItemShop.Selected();
                continue;
            }
            bool isContainItem = DataManager.Instance.Data.ListShieldOwner.Contains(i);
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
        if (currentShield == -1)
        {
            SetCurrentUIItemShop(0);
            CurrentUIItemShop.Selected();
        }
    }

    public override void PreviewItem(int idUIITemShop)
    {
        for (int i = 0; i < listItemPreviewContain.Count; i++)
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
        currentItemPreview = Instantiate(shieldSO.propsShields[idUIITemShop].avatarPrefab, LevelManager.Instance.CurrentPlayer.ShieldHolderTF);
        currentItemPreview.SetId(idUIITemShop);

        listItemPreviewContain.Add(currentItemPreview);
        ShowPreviewItem();
    }

    protected override void ShowPreviewItem()
    {
        currentItemPreview.gameObject.SetActive(true);
    }
    public override void HidePreviewItem()
    {
        if (currentItemPreview != null)
        {
            currentItemPreview.gameObject.SetActive(false);
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        LevelManager.Instance.CurrentPlayer.HideShieldAvaAttach();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        for (int i = 0; i < listItemPreviewContain.Count; i++) // destroy cac list item preview
        {
            Destroy(listItemPreviewContain[i].gameObject);
        }
        listItemPreviewContain.Clear();
        LevelManager.Instance.CurrentPlayer.ShowShieldAvaAttach();
    }

    public override int GetCurrentItemInData()
    {
        return DataManager.Instance.Data.CurrentShield;
    }

    public override List<int> GetItemOwnerInData()
    {
        return DataManager.Instance.Data.ListShieldOwner;
    }
    public override void ChangeCurrentItemInData(int idUIITemShop)
    {
        DataManager.Instance.Data.ChangeCurrentShieldData(idUIITemShop);
    }

    public override void AttachItemToPlayer()
    {
        LevelManager.Instance.CurrentPlayer.AttachShield(currentUIItemShop.Id);
    }

    public override void DeAttachItemToPlayer()
    {
        LevelManager.Instance.CurrentPlayer.DeAttachShield();
    }
}