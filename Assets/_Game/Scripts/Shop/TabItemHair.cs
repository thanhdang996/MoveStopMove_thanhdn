using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabItemHair : AbstractTabItem
{
    [SerializeField] protected HairSO hairSO;

    [SerializeField] private PrefabItemShop currentPrefabItemPlayer;
    [SerializeField] private List<PrefabItemShop> listPrefabItemPlayerContain;

    public override void ActiveAllUIItemShop()
    {
        PropsHair[] propsHair = hairSO.propsHair;
        int totalItemData = propsHair.Length;
        int numberItemShow = tabRoot.ListUIItemShop.Count;

        int diff = Mathf.Abs(totalItemData - numberItemShow);
        if (totalItemData >= numberItemShow)
        {
            for (int i = 0; i < diff; i++)
            {
                UIItemShop itemShop = Instantiate(prefabUIItemShop, contentTF);
                tabRoot.ListUIItemShop.Add(itemShop);
            }
        }
        else //if (totalItemData < numberItemShow)
        {
            for (int i = 0; i < diff; i++)
            {
                tabRoot.ListUIItemShop[tabRoot.ListUIItemShop.Count - i - 1].gameObject.SetActive(false);
            }
        }

        for (int i = 0; i < propsHair.Length; i++)
        {
            tabRoot.ListUIItemShop[i].gameObject.SetActive(true);
            tabRoot.ListUIItemShop[i].SetData(i, this, propsHair[i].spriteImage, propsHair[i].price);
        }
        SetCurrentUIItemShop(0);
        tabRoot.ListUIItemShop[0].Selected();
    }

    public override void PreviewItemOnPlayer(int idUIITemShop)
    {
        for (int i = 0; i < listPrefabItemPlayerContain.Count; i++)
        {
            if (listPrefabItemPlayerContain[i].Id == idUIITemShop)
            {
                DeActiveitemOnCurrentPlayer();
                currentPrefabItemPlayer = listPrefabItemPlayerContain[i];
                ActiveItemOnCurrentPlayer();
                return;
            }
        }
        DeActiveitemOnCurrentPlayer();
        currentPrefabItemPlayer = Instantiate(hairSO.propsHair[idUIITemShop].avatarPrefab, LevelManager.Instance.CurrentPlayer.HairHolderTF);
        currentPrefabItemPlayer.SetId(idUIITemShop);

        listPrefabItemPlayerContain.Add(currentPrefabItemPlayer);
        ActiveItemOnCurrentPlayer();
    }

    protected override void ActiveItemOnCurrentPlayer()
    {
        currentPrefabItemPlayer.gameObject.SetActive(true);
    }
    public override void DeActiveitemOnCurrentPlayer()
    {
        if (currentPrefabItemPlayer != null)
        {
            currentPrefabItemPlayer.gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        DeActiveitemOnCurrentPlayer();
    }

}