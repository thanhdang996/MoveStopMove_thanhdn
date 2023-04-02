using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ITabItem : MonoBehaviour
{
    public abstract void InitItem();
    public abstract void PreviewItemOnPlayer(int idUIITemShop);
    public abstract void ActiveAllUIItemShop();
    public abstract void DeActiveAllUIItemShop();


    public abstract void ActiveItemOnCurrentPlayer();
    public abstract void DeActiveitemOnCurrentPlayer();
    public abstract void HandleOutLineButton(int id);
    public abstract void SetCurrentUIItemShop(int index);
    public abstract void HandleOnClick();

    public abstract void TurnOnOutLine();
    public abstract void TurnOffOutLine();

    public abstract int GetTabIndex();

    public abstract UICShopDress GetUICShopDress();

    public abstract UIItemShop GetCurrentUIItemShop();
}
