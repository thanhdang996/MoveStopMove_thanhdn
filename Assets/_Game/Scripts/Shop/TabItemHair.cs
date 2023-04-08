using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabItemHair : AbstractTabItem
{
    [SerializeField] private HairSO hairSO;

    [SerializeField] private PrefabItemShop currentPrefabItemPreviewOnPlayer;
    [SerializeField] private List<PrefabItemShop> listPrefabItemPlayerContain = new List<PrefabItemShop>();


    public override void ActiveAllUIItemShop()
    {
        // check to Instantiate item , deactive item in contentTF
        PropsHair[] propsHair = hairSO.propsHair;
        int totalItemData = propsHair.Length;
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
        int currentHair = GetCurrentItemInData();
        for (int i = 0; i < propsHair.Length; i++)
        {
            tabRoot.ListUIItemShop[i].gameObject.SetActive(true); // bat tat ca 
            tabRoot.ListUIItemShop[i].SetData(i, this, propsHair[i].spriteImage, propsHair[i].price);
            if (currentHair == i)
            {
                SetCurrentUIItemShop(currentHair);
                CurrentUIItemShop.ChangeActiveImageLock(false);
                CurrentUIItemShop.ChangeActiveTextEquip(true);
                CurrentUIItemShop.SetUnlock(true);
                CurrentUIItemShop.Selected();
                continue;
            }
            bool isContainItem = DataManager.Instance.Data.ListHairOwner.Contains(i);
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
        if (currentHair == -1)
        {
            SetCurrentUIItemShop(0); // neu chua co item owner thi hightlight default item 0
            CurrentUIItemShop.Selected();
        }
    }

    public override void PreviewItemOnPlayer(int idUIITemShop)
    {
        for (int i = 0; i < listPrefabItemPlayerContain.Count; i++)
        {
            if (listPrefabItemPlayerContain[i].Id == idUIITemShop)
            {
                DeActiveitemOnCurrentPlayer();
                currentPrefabItemPreviewOnPlayer = listPrefabItemPlayerContain[i];
                ActiveItemOnCurrentPlayer();
                return;
            }
        }
        DeActiveitemOnCurrentPlayer();
        currentPrefabItemPreviewOnPlayer = Instantiate(hairSO.propsHair[idUIITemShop].avatarPrefab, LevelManager.Instance.CurrentPlayer.HairHolderTF);
        currentPrefabItemPreviewOnPlayer.SetId(idUIITemShop);

        listPrefabItemPlayerContain.Add(currentPrefabItemPreviewOnPlayer);
        ActiveItemOnCurrentPlayer();
    }

    protected override void ActiveItemOnCurrentPlayer()
    {
        currentPrefabItemPreviewOnPlayer.gameObject.SetActive(true);
    }
    public override void DeActiveitemOnCurrentPlayer()
    {
        if (currentPrefabItemPreviewOnPlayer != null)
        {
            currentPrefabItemPreviewOnPlayer.gameObject.SetActive(false);
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        if (LevelManager.Instance.CurrentPlayer.CurrentHairAvaGO != null)
        {
            LevelManager.Instance.CurrentPlayer.CurrentHairAvaGO.SetActive(false);
        }
    }

    protected override void OnDisable()
    {
        base.OnEnable();
        DeActiveitemOnCurrentPlayer();

        if (LevelManager.Instance.CurrentPlayer.CurrentHairAvaGO != null)
        {
            LevelManager.Instance.CurrentPlayer.CurrentHairAvaGO.SetActive(true);
        }
    }

    public override int GetCurrentItemInData()
    {
        return DataManager.Instance.Data.CurrentHair;
    }

    public override List<int> GetItemOwnerInData()
    {
        return DataManager.Instance.Data.ListHairOwner;
    }
    public override void ChangeCurrentItemInData(int idUIITemShop)
    {
        DataManager.Instance.Data.ChangeCurrentHairData(idUIITemShop);
    }

    public override void AttachItemToPlayer()
    {
        LevelManager.Instance.CurrentPlayer.AttachHair(currentUIItemShop.Id);
    }

    public override void DeAttachItemToPlayer()
    {
        LevelManager.Instance.CurrentPlayer.DeAttachHair();
    }
}