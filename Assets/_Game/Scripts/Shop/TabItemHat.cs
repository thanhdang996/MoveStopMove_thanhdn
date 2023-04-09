using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabItemHat : AbstractTabItem
{
    [SerializeField] private HatSO hatSO;

    [SerializeField] private PrefabItemShop currentPrefabItemPreviewOnPlayer;
    [SerializeField] private List<PrefabItemShop> listPrefabItemPreviewPlayerContain = new List<PrefabItemShop>();


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

    public override void PreviewItemOnPlayer(int idUIITemShop)
    {
        for (int i = 0; i < listPrefabItemPreviewPlayerContain.Count; i++)
        {
            if (listPrefabItemPreviewPlayerContain[i].Id == idUIITemShop)
            {
                DeActiveitemOnCurrentPlayer();
                currentPrefabItemPreviewOnPlayer = listPrefabItemPreviewPlayerContain[i];
                ActiveItemOnCurrentPlayer();
                return;
            }
        }
        DeActiveitemOnCurrentPlayer();
        currentPrefabItemPreviewOnPlayer = Instantiate(hatSO.propsHats[idUIITemShop].avatarPrefab, LevelManager.Instance.CurrentPlayer.HatHolderTF);
        currentPrefabItemPreviewOnPlayer.SetId(idUIITemShop);

        listPrefabItemPreviewPlayerContain.Add(currentPrefabItemPreviewOnPlayer);
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
        LevelManager.Instance.CurrentPlayer.HideHatAvaAttach();

    }

    protected override void OnDisable()
    {
        base.OnDisable();
        DeActiveitemOnCurrentPlayer();
        LevelManager.Instance.CurrentPlayer.ShowHatAvaAttach();
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