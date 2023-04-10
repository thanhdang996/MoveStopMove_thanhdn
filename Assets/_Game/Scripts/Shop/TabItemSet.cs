using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabItemSet : AbstractTabItem
{
    [SerializeField] private SetSO setSO;
    [SerializeField] private Material currentMatPreview;


    public override void ActiveAllUIItemShop()
    {
        // check to Instantiate item , deactive item in contentTF
        PropsSet[] propsSets = setSO.propsSets;
        int totalItemData = propsSets.Length;
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
        for (int i = 0; i < propsSets.Length; i++)
        {
            tabRoot.ListUIItemShop[i].gameObject.SetActive(true); // bat tat ca 
            tabRoot.ListUIItemShop[i].SetData(i, this, propsSets[i].spriteImage, propsSets[i].price);
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
        currentMatPreview = setSO.propsSets[idUIITemShop].mat;
        LevelManager.Instance.CurrentPlayer.SetSetMat(currentMatPreview);
    }


    protected override void ActiveItemOnCurrentPlayer()
    {
        LevelManager.Instance.CurrentPlayer.SetSetMatCurrent();
    }
    public override void DeActiveitemOnCurrentPlayer()
    {
        LevelManager.Instance.CurrentPlayer.SetTransparentSet();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        DeActiveitemOnCurrentPlayer();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        ActiveItemOnCurrentPlayer();
    }

    public override int GetCurrentItemInData()
    {
        return DataManager.Instance.Data.CurrentSet;
    }

    public override List<int> GetItemOwnerInData()
    {
        return DataManager.Instance.Data.ListSetOwner;
    }
    public override void ChangeCurrentItemInData(int idUIITemShop)
    {
        DataManager.Instance.Data.ChangeCurrentSetData(idUIITemShop);
    }


    public override void AttachItemToPlayer()
    {
        LevelManager.Instance.CurrentPlayer.AttachSet(currentUIItemShop.Id);
    }

    public override void DeAttachItemToPlayer()
    {
        LevelManager.Instance.CurrentPlayer.DeAttachSet();
    }
}
