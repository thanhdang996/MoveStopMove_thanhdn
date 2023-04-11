using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabItemSet : AbstractTabItem
{
    [SerializeField] private SetSO setSO;
    [SerializeField] private Material currentMatPreview;

    [SerializeField] private ItemShop prefabItemHat;
    [SerializeField] private ItemShop prefabItemWing;
    [SerializeField] private ItemShop prefabItemTail;


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
        int currentSet = GetCurrentItemInData();
        for (int i = 0; i < propsSets.Length; i++)
        {
            tabRoot.ListUIItemShop[i].gameObject.SetActive(true); // bat tat ca 
            tabRoot.ListUIItemShop[i].SetData(i, this, propsSets[i].spriteImage, propsSets[i].price);
            if (currentSet == i)
            {
                SetCurrentUIItemShop(currentSet);
                CurrentUIItemShop.ChangeActiveImageLock(false);
                CurrentUIItemShop.ChangeActiveTextEquip(true);
                CurrentUIItemShop.SetUnlock(true);
                CurrentUIItemShop.Selected();
                continue;
            }
            bool isContainItem = DataManager.Instance.Data.ListSetOwner.Contains(i);
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
        if (currentSet == -1)
        {
            SetCurrentUIItemShop(0); // neu chua co item owner thi hightlight default item 0
            CurrentUIItemShop.Selected();
        }
    }

    public override void PreviewItem(int idUIITemShop)
    {
        HidePreviewItem();

        // preview material for set
        currentMatPreview = setSO.propsSets[idUIITemShop].mat;
        LevelManager.Instance.CurrentPlayer.SetSetMat(currentMatPreview);

        //preview for itemPrefab
        if (setSO.HasHat(idUIITemShop))
        {
            prefabItemHat = Instantiate(setSO.propsSets[idUIITemShop].hatPrefab, LevelManager.Instance.CurrentPlayer.HatHolderTF);
        }
        if(setSO.HasWing(idUIITemShop))
        {
            prefabItemWing = Instantiate(setSO.propsSets[idUIITemShop].wingPrefab, LevelManager.Instance.CurrentPlayer.WingHolderTF);
        }
        if (setSO.HasTail(idUIITemShop))
        {
            prefabItemTail = Instantiate(setSO.propsSets[idUIITemShop].tailPrefab, LevelManager.Instance.CurrentPlayer.TailHolderTF);
        }
    }


    protected override void ShowPreviewItem()
    {
        LevelManager.Instance.CurrentPlayer.ShowSetAttach();
    }
    public override void HidePreviewItem()
    {
        // for material
        LevelManager.Instance.CurrentPlayer.HideSetAttach();

        // for itemPrefab
        DestroyItemPreview();

    }

    public void DestroyItemPreview()
    {
        Destroy(prefabItemHat != null ? prefabItemHat.gameObject : null);
        Destroy(prefabItemWing != null ? prefabItemWing.gameObject : null);
        Destroy(prefabItemTail != null ? prefabItemTail.gameObject : null);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        HidePreviewItem();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        DestroyItemPreview();
        ShowPreviewItem();
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
