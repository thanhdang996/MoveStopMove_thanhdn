using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabItemMat : AbstractTabItem
{
    [SerializeField] protected PantSO pantSO;
    protected override void InitItem()
    {
        for (int i = 0; i < pantSO.propsPants.Length; i++)
        {
            PropsItem propItem = pantSO.propsPants[i];

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
        LevelManager.Instance.CurrentPlayer.CurrentSkin.material = pantSO.propsPants[idUIITemShop].mat;
    }
    protected override void ActiveItemOnCurrentPlayer()
    {
        LevelManager.Instance.CurrentPlayer.CurrentSkin.enabled = true;
    }
    public override void DeActiveitemOnCurrentPlayer()
    {
        LevelManager.Instance.CurrentPlayer.CurrentSkin.enabled = true;
    }
}
