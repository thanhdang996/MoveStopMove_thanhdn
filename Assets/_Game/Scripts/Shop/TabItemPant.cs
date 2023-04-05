using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabItemPant : AbstractTabItem
{
    [SerializeField] protected PantSO pantSO;

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
                UIItemShop itemShop = Instantiate(prefabUIItemShop, contentTF);
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

        for (int i = 0; i < propPants.Length; i++)
        {
            tabRoot.ListUIItemShop[i].gameObject.SetActive(true);
            tabRoot.ListUIItemShop[i].SetData(i, this, propPants[i].spriteImage, propPants[i].price);
        }
        SetCurrentUIItemShop(0);
        tabRoot.ListUIItemShop[0].Selected();
    }


    public override void PreviewItemOnPlayer(int idUIITemShop)
    {
        ActiveItemOnCurrentPlayer();
        LevelManager.Instance.CurrentPlayer.CurrentSkin.material = pantSO.propsPants[idUIITemShop].mat;
    }
    protected override void ActiveItemOnCurrentPlayer()
    {
        LevelManager.Instance.CurrentPlayer.CurrentSkin.enabled = true;
    }
    public override void DeActiveitemOnCurrentPlayer()
    {
        LevelManager.Instance.CurrentPlayer.CurrentSkin.enabled = false;
    }

    private void OnDisable()
    {
        DeActiveitemOnCurrentPlayer();
    }
}
