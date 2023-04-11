using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabItemPant : AbstractTabItem
{
    [SerializeField] private PantSO pantSO;

    [SerializeField] private Material currentMatPreview;


    public override void ActiveAllUIItemShop()
    {
        PropsPant[] propPants = pantSO.propsPants;
        int totalItemData = propPants.Length;
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




        int currentPant = GetCurrentItemInData();
        for (int i = 0; i < propPants.Length; i++)
        {
            tabRoot.ListUIItemShop[i].gameObject.SetActive(true);
            tabRoot.ListUIItemShop[i].SetData(i, this, propPants[i].spriteImage, propPants[i].price);
            if (currentPant == i)
            {
                SetCurrentUIItemShop(currentPant);
                CurrentUIItemShop.ChangeActiveImageLock(false);
                CurrentUIItemShop.ChangeActiveTextEquip(true);
                CurrentUIItemShop.SetUnlock(true);
                CurrentUIItemShop.Selected();
                continue;
            }
            bool isContainItem = DataManager.Instance.Data.ListPantOwner.Contains(i);
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
        if (currentPant == -1)
        {
            SetCurrentUIItemShop(0);
            CurrentUIItemShop.Selected();
        }
    }


    public override void PreviewItem(int idUIITemShop)
    {
        HidePreviewItem();
        currentMatPreview = pantSO.propsPants[idUIITemShop].mat;
        ShowPreviewItem();
    }
    protected override void ShowPreviewItem() //show tren nguoi player luon
    {
        LevelManager.Instance.CurrentPlayer.SetPantMat(currentMatPreview);
    }
    public override void HidePreviewItem()
    {
        LevelManager.Instance.CurrentPlayer.HidePantAttach(); //hide tren nguoi player luon
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        HidePreviewItem();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        LevelManager.Instance.CurrentPlayer.ShowPantAttach();
    }

    public override int GetCurrentItemInData()
    {
        return DataManager.Instance.Data.CurrentPant;
    }

    public override List<int> GetItemOwnerInData()
    {
        return DataManager.Instance.Data.ListPantOwner;
    }
    public override void ChangeCurrentItemInData(int idUIITemShop)
    {
        DataManager.Instance.Data.ChangeCurrentPantData(idUIITemShop);
    }

    public override void AttachItemToPlayer()
    {
        LevelManager.Instance.CurrentPlayer.AttachPant(currentUIItemShop.Id);
    }

    public override void DeAttachItemToPlayer()
    {
        LevelManager.Instance.CurrentPlayer.DeAttachPant();
    }
}
