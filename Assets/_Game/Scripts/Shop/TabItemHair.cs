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

    protected override void InitItem()
    {
        for (int i = 0; i < hairSO.propsHair.Length; i++)
        {
            PropsItem propItem = hairSO.propsHair[i];

            UIItemShop itemShop = Instantiate(prefabUIItemShop, contentTF);
            listUIItemShop.Add(itemShop);
            itemShop.SetId(i);
            itemShop.SetTabItem(this);
            itemShop.SetSprite(propItem.spriteImage);
            itemShop.SetPrice(propItem.price);

            if (i == 0)
            {
                SetCurrentUIItemShop(0);
                itemShop.Selected();
            }
        }
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

}